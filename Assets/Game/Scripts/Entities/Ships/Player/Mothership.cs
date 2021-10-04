using System.Collections;
using System.Collections.Generic;
using ManyTools.UnityExtended;
using ManyTools.UnityExtended.Editor;
using ManyTools.UnityExtended.Poolable;
using ManyTools.Variables;
using SketchFleets.Data;
using SketchFleets.General;
using SketchFleets.Inventory;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Random = UnityEngine.Random;

namespace SketchFleets.Entities
{
    /// <summary>
    ///     A class that controls the mothership
    /// </summary>
    public class Mothership : Ship<MothershipAttributes>
    {
        #region Private Fields

        [Header("Mothership Specific")]
        [SerializeField]
        [RequiredField]
        private Transform shipSpawnPoint;

        [SerializeField]
        [RequiredField]
        private GameObject shipSpawnMenu;

        [SerializeField]
        [RequiredField]
        private GameObject hud;

        [SerializeField]
        private SpawnableShipAttributes cyanShipAttributes;
        
        [SerializeField]
        private ColorsInventory colorsInventory;

        private List<ItemEffect> activeEffects;
        private List<StatusEffect> activeSpawnEffects;

        private Camera mainCamera;

        private IEnumerator regenerateRoutine;

        [SerializeField] 
        private float radiusMultiply;
        
        private float radiusSpeed = 2.78f;

        #endregion

        #region Properties

        public MothershipAttributesBonuses AttributesBonuses { get; private set; }

        public UnityDictionary<SpawnableShipAttributes, SpawnMetaData> SpawnMetaDatas { get; } =
            new UnityDictionary<SpawnableShipAttributes, SpawnMetaData>();

        public float AbilityTimer { get; private set; }

        public override FloatReference MaxHealth => new FloatReference(GetMaxHealth());

        public ShootingTarget _ShootingTarget; //ManyTools Vector2 don1t work... Gambiarra bro

        #endregion

        #region Unity Callbacks

        protected void Awake()
        {
            AttributesBonuses = ScriptableObject.CreateInstance<MothershipAttributesBonuses>();
            IngameEffectApplier.OnEffectsChange = OnEffectsChange;
            IngameEffectApplier.Clear();
            EnhancedTouchSupport.Enable();
            TouchSimulation.Enable();
        }

        // Start runs once before the first update
        protected override void Start()
        {
            base.Start();
            // Caches necessary components
            mainCamera = Camera.main;
            regenerateRoutine = RegenerateShips();
        }

        // Start runs once every frame
        protected override void Update()
        {
            base.Update();

            HandlePlayerInput();
            TickTimers();
            Fire();
            
            SetCrystalColor();
        }

        #endregion

        #region IDamageable Overrides

        public override void Heal(float amount)
        {
            currentHealth.Value = Mathf.Min(GetMaxHealth(), currentHealth + amount);

            Transform cachedTransform = transform;

            if (Attributes.HealEffect == null) return;

            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.Request(Attributes.HealEffect)
                    .Emerge(cachedTransform.position, cachedTransform.rotation);
            }
        }

        #endregion

        #region Ship<T> Overrides

        /// <summary>
        ///     Makes the ship die
        /// </summary>
        public override void Die()
        {
            EnableOrDisableSpawnMenu(false);
            base.Die();
        }

        /// <summary>
        ///     Resets all instance-specific variables
        /// </summary>
        protected override void ResetInstanceVariables()
        {
            currentHealth.Value = GetMaxHealth();
            currentShield.Value = GetMaxShield();
            fireTimer = 0;
        }

        /// <summary>
        ///     Fires the ship's weapons
        /// </summary>
        public override void Fire()
        {
            if (!CanFire()) return;

            for (int index = 0, upper = bulletSpawnPoints.Length; index < upper; index++)
            {
                PoolMember bullet = PoolManager.Instance.Request(attributes.Fire.Prefab);
                bullet.Emerge(bulletSpawnPoints[index].position, transform.rotation);

                bullet.transform.Rotate(0f, 0f,
                    Random.Range(Attributes.Fire.AngleJitter * -1f, Attributes.Fire.AngleJitter));

                BulletController bulletController = bullet.GetComponent<BulletController>();
                bulletController.DamageMultiplier = AttributesBonuses.DamageMultiplier;
                bulletController.DamageIncrease = AttributesBonuses.DamageIncrease;
            }

            fireTimer = GetFireCooldown();
        }

        /// <summary>
        ///     Regenerates the ship's shields
        /// </summary>
        protected override void RegenShield()
        {
            // Decrements the regen timer
            shieldRegenTimer -= Time.deltaTime;

            // If the regen timer is not over or there is no regen, end it here
            if (!CanRegenShield()) return;

            // Regen shield
            CurrentShield.Value = Mathf.Min(GetMaxShield(), CurrentShield.Value + GetShieldRegen());
        }

        #endregion

        #region Public Methods

        public OrbitAndExplodeState GetCyanShip()
        {
            for (int index = 0, max = SpawnMetaDatas[cyanShipAttributes].CurrentlyActive.Count; index < max; index++)
            {
                if (SpawnMetaDatas[cyanShipAttributes].CurrentlyActive[index] != null)
                {
                    return SpawnMetaDatas[cyanShipAttributes].CurrentlyActive[index]
                        .GetComponent<OrbitAndExplodeState>();
                }
            }

            return null;
        }

        /// <summary>
        ///     Gets whether the Mothership can currently regenerate its shields
        /// </summary>
        /// <returns>Whether the mothership can regenerate its shields</returns>
        public bool CanRegenShield()
        {
            return shieldRegenTimer <= 0 &&
                   !Mathf.Approximately(Attributes.ShieldRegen, 0) &&
                   !Mathf.Approximately(CurrentShield.Value, GetMaxShield());
        }

        /// <summary>
        ///     Gets the shield regen for the current point in time
        /// </summary>
        /// <returns>The shield regen for the current point in time</returns>
        public float GetShieldRegen()
        {
            return (Attributes.ShieldRegen + AttributesBonuses.ShieldRegen) * Time.deltaTime * Time.timeScale;
        }

        /// <summary>
        ///     Gets the max health of the mothership
        /// </summary>
        /// <returns>The max health</returns>
        public float GetMaxHealth()
        {
            return Attributes.MaxHealth + AttributesBonuses.MaxHealth + AttributesBonuses.HealthIncrease;
        }

        /// <summary>
        ///     Gets the max shield of the mothership
        /// </summary>
        /// <returns>The max shield</returns>
        public float GetMaxShield()
        {
            return Attributes.MaxShield + AttributesBonuses.MaxShield + AttributesBonuses.ShieldIncrease;
        }

        /// <summary>
        ///     Summons a ship of a given type
        /// </summary>
        /// <param name="shipType">The ship type to spawn</param>
        public void SummonShip(SpawnableShipAttributes shipType)
        {
            // Generate metadata if necessary
            if (!SpawnMetaDatas.ContainsKey(shipType))
            {
                GetSpawnMetaData(shipType);
            }

            // If the new spawn would exceed the maximum amount, return
            if (!CanSpawnShip(shipType)) return;

            Damage(GetSpawnCost(shipType), false, true);

            // Spawns the ship
            PoolMember spawn = PoolManager.Instance.Request(shipType.Prefab);
            spawn.Emerge(shipSpawnPoint.position, Quaternion.identity);
            spawn.GetComponent<SpriteRenderer>().material.SetColor(redMultiplier, colorsInventory.drawColor);
            colorsInventory.UseColor();

            SpawnedShip shipController = spawn.GetComponent<SpawnedShip>();

            // Adds the cooldown
            SpawnMetaDatas[shipType].SummonTimer.Value = GetMaxSpawnCooldown(shipType);
            SpawnMetaDatas[shipType].CurrentlyActive.Add(shipController);

            shipController.SpawnNumber = SpawnMetaDatas[shipType].CurrentlyActive.Count;
        }

        /// <summary>
        ///     Removes a specific active summon
        /// </summary>
        /// <param name="shipToRemove">The active summon to remove</param>
        public void RemoveActiveSummon(SpawnedShip shipToRemove)
        {
            SpawnMetaDatas[shipToRemove.Attributes].CurrentlyActive.Remove(shipToRemove);
        }

        /// <summary>
        ///     Gets the cost of spawning a specific ship
        /// </summary>
        /// <param name="shipType">The type of the ship to get the cost of</param>
        /// <returns>The cost of spawning said ship</returns>
        public float GetSpawnCost(SpawnableShipAttributes shipType)
        {
            return shipType.GraphiteCost +
                   shipType.GraphiteCostIncrease *
                   math.max(GetSpawnMetaData(shipType).CurrentlyActive.Count, 1);
        }

        /// <summary>
        ///     Get the cooldown of spawning a specific ship
        /// </summary>
        /// <param name="shipType">The type of ship to get the cooldown of</param>
        /// <returns>The cooldown of said type of ship</returns>
        public float GetSpawnCooldown(SpawnableShipAttributes shipType)
        {
            return GetSpawnMetaData(shipType).SummonTimer.Value;
        }

        /// <summary>
        ///     Gets whether the ship can be spawned (wouldn't exceed spawn limit)
        /// </summary>
        /// <param name="shipType">The type of ship to check for</param>
        /// <returns>Whether there is remaining space to spawn the ship</returns>
        public bool IsThereSpaceForSpawn(SpawnableShipAttributes shipType)
        {
            return GetSpawnMetaData(shipType).CurrentlyActive.Count + 1 <= GetMaxShipCount(shipType);
        }

        /// <summary>
        ///     Gets the maximum ship count of a given ship type
        /// </summary>
        /// <param name="shipType">The ship type to check for maximum count</param>
        /// <returns>The maximum count of a given ship type</returns>
        public int GetMaxShipCount(SpawnableShipAttributes shipType)
        {
            return shipType.MaximumShips.Value + AttributesBonuses.ExtraSpawnSlots;
        }

        /// <summary>
        ///     Gets whether the given ship type can be spawned at the moment
        /// </summary>
        /// <param name="shipType">The ship type to check for spawn ability</param>
        /// <returns>Whether the ship can be spawned</returns>
        public bool CanSpawnShip(SpawnableShipAttributes shipType)
        {
            return IsThereSpaceForSpawn(shipType) && GetSpawnCooldown(shipType) <= 0 &&
                   CurrentHealth > GetSpawnCost(shipType);
        }

        /// <summary>
        ///     Gets the maximum spawn cooldown of a given ship type
        /// </summary>
        /// <param name="shipType">The type of the desired ship</param>
        /// <returns>The maximum cooldown of the given ship type</returns>
        public float GetMaxSpawnCooldown(SpawnableShipAttributes shipType)
        {
            return (float)shipType.SpawnCooldown * AttributesBonuses.SpawnCooldownMultiplier;
        }

        /// <summary>
        ///     Generates spawn meta data for a given ship type
        /// </summary>
        /// <param name="shipType">The type to which generate a spawn meta data for</param>
        public SpawnMetaData GetSpawnMetaData(SpawnableShipAttributes shipType)
        {
            if (SpawnMetaDatas.ContainsKey(shipType))
            {
                return SpawnMetaDatas[shipType];
            }

            SpawnMetaDatas.Add(shipType, new SpawnMetaData(shipType));
            return SpawnMetaDatas[shipType];
        }

        /// <summary>
        ///     Gets the maximum ability cooldown
        /// </summary>
        /// <returns>The maximum ability cooldown</returns>
        public float GetMaxAbilityCooldown()
        {
            return Attributes.RegenerateCooldown * AttributesBonuses.AbilityCooldownMultiplier;
        }

        /// <summary>
        ///     Gets whether the ability can be used
        /// </summary>
        /// <returns>Whether the ability can be used</returns>
        public bool IsAbilityAvailable()
        {
            return AbilityTimer <= 0f;
        }

        /// <summary>
        ///     Gets the speed of the ship for the current point in time
        /// </summary>
        /// <returns>The speed of the ship for the current point in time</returns>
        public float GetSpeed()
        {
            return (Attributes.Speed + AttributesBonuses.SpeedIncrease)
                   * AttributesBonuses.SpeedMultiplier * Time.timeScale * Time.deltaTime;
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Checks whether the Mothership can fire
        /// </summary>
        /// <returns>Whether the Mothership can fire</returns>
        private bool CanFire()
        {
            return fireTimer <= 0f && Time.timeScale != 0;
        }

        /// <summary>
        ///     Ticks all timers related to the mothership
        /// </summary>
        private void TickTimers()
        {
            IngameEffectApplier.TickItems(Time.deltaTime);
            UpdateSummonTimers();
            AbilityTimer = AbilityTimer - Time.deltaTime * Time.timeScale;
        }

        /// <summary>
        ///     Handles player input
        /// </summary>
        private void HandlePlayerInput()
        {
            // Moves and rotates the ship
            if (Time.timeScale != 1) return;
            //Move();
            RadiusLook(_ShootingTarget.targetPoint.position);
        }
        
        private void RadiusLook(Vector2 target)
        {
            // Workaround, this should be done using a proper Pausing interface
            if (Mathf.Approximately(0f, Time.timeScale)) return;

            // This doesn't really solve the problem. The transform should be a member variable
            // to avoid the constant marshalling
            Transform transformCache = transform.parent;
            transformCache.up = (Vector3)target - transformCache.position;
        }

        /// <summary>
        ///     Gets the fire cooldown
        /// </summary>
        /// <returns>The fire cooldown</returns>
        private float GetFireCooldown()
        {
            return Attributes.Fire.Cooldown;
        }

        /// <summary>
        ///     Gets whether the game has ended
        /// </summary>
        /// <returns>Whether the game has ended</returns>
        private bool IsGameOver()
        {
            return LevelManager.Instance.GameEnded;
        }

        /// <summary>
        ///     Enables or disables the ship spawn menu
        /// </summary>
        /// <param name="enable">Whether to enable or disable the ship spawn menu</param>
        public void EnableOrDisableSpawnMenu(bool enable)
        {
            //hud.SetActive(!enable);
            Debug.Log("eee");
            //shipSpawnMenu.SetActive(enable);
        }

        /// <summary>
        ///     Moves and rotates the Mothership
        /// </summary>
        /// 
        
        public void JoystickMove(Vector2 moveDir)
        {
            Transform transformCache = transform;
            Transform parent = transformCache.parent;

            transformCache.localPosition = Vector2.zero;
            parent.Translate(moveDir * GetSpeed(), Space.World);
        }
        
        public void Move(Vector2 movePos, Vector2 moveRad)
        {
            // Gets movement input
            Transform transformCache = transform;
            Transform parent = transformCache.parent;

            transform.localPosition = Vector2.MoveTowards(transformCache.localPosition, GetRadiusPosition(moveRad), radiusSpeed*Time.deltaTime);
            parent.position = Vector2.MoveTowards(parent.position, GetMovePosition(movePos), GetSpeed());
        }
        
        private Vector2 GetMovePosition(Vector2 movePos)
        {
            return mainCamera.ViewportToWorldPoint(mainCamera.ScreenToViewportPoint(movePos));
        }
        
        private Vector2 GetRadiusPosition(Vector2 moveRad)
        {
            float radius = (float)System.Math.Round(Mathf.Lerp(moveRad.x,moveRad.y,.5f),3);
            return new Vector2(0, 1.5f + radius * radiusMultiply);
        }

        /// <summary>
        ///     Updates all spawn cooldowns
        /// </summary>
        private void UpdateSummonTimers()
        {
            if (SpawnMetaDatas.Count <= 0) return;

            foreach (var metaData in SpawnMetaDatas)
            {
                metaData.Value.SummonTimer.Value -= Time.deltaTime * Time.timeScale;
            }
        }
        
        private void SetCrystalColor()
        {
            if (colorsInventory.drawColor == Color.black)
                spriteRenderer.material.SetColor(blueMultiplier, Color.black);
            else
                spriteRenderer.material.SetColor(blueMultiplier, colorsInventory.drawColor);
        }

        /// <summary>
        ///     Sacrifices all ships for health
        /// </summary>
        private IEnumerator RegenerateShips()
        {
            WaitForSeconds killInterval = new WaitForSeconds(Attributes.RegenerateKillInterval);
            AbilityTimer = GetMaxAbilityCooldown();

            // For every spawned ship type
            foreach (var metaData in SpawnMetaDatas)
            {
                // For every ship
                for (int index = metaData.Value.CurrentlyActive.Count - 1; index >= 0; index--)
                {
                    // Ignore dead ships
                    if (metaData.Value.CurrentlyActive[index] == null)
                    {
                        metaData.Value.CurrentlyActive.RemoveAt(index);
                    }
                    else
                    {
                        // Heal player for the spawn cost of the ship
                        Heal(GetSpawnCost(metaData.Key));
                        metaData.Value.CurrentlyActive[index].Die();

                        yield return killInterval;
                    }
                }
            }
        }

        #endregion

        #region Effects

        private void OnEffectsChange()
        {
            IngameEffectResult result =
                IngameEffectApplier.GetResult(attributes.ItemRegister, attributes.UpgradeRegister);

            AttributesBonuses.HealthIncrease.Value = result.upgradeLifeIncrease;
            AttributesBonuses.DamageIncrease.Value = result.upgradeDamageIncrease;
            AttributesBonuses.ShieldIncrease.Value = result.upgradeShieldIncrease;
            AttributesBonuses.SpeedIncrease.Value = result.upgradeSpeedIncrease;

            AttributesBonuses.HealthRegen.Value = result.healthRegen;
            AttributesBonuses.ShieldRegen.Value = result.shieldRegen;
            AttributesBonuses.ExtraSpawnSlots.Value = result.spawnSlotBonus;
            AttributesBonuses.SpawnCooldownMultiplier.Value = result.spawnCooldownMultiplierBonus;
            AttributesBonuses.AbilityCooldownMultiplier.Value = result.abilityCooldownMultiplierBonus;
            AttributesBonuses.MaxHealth.Value = result.maxHealthBonus;
            AttributesBonuses.MaxShield.Value = result.maxShieldBonus;
            AttributesBonuses.DamageMultiplier.Value = result.damageMultiplierBonus;
            AttributesBonuses.SpeedMultiplier.Value = result.speedMultiplierBonus;
            AttributesBonuses.Defense.Value = result.defenseBonus;

            Heal(AttributesBonuses.HealthRegen);
            CurrentShield.Value = Mathf.Min(GetMaxShield(), CurrentShield.Value + AttributesBonuses.ShieldRegen);
        }

        #endregion
    }
}