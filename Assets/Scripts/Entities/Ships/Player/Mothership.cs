using System.Collections;
using UnityEngine;
using SketchFleets.Data;
using System.Collections.Generic;
using ManyTools.UnityExtended.Editor;
using ManyTools.UnityExtended.Poolable;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using ManyTools.UnityExtended;
using SketchFleets.Inventory;
using SketchFleets.General;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that controls the mothership
    /// </summary>
    public class Mothership : Ship<MothershipAttributes>
    {
        #region Private Fields

        [Header("Mothership Specific")]
        [SerializeField, RequiredField()]
        private Transform shipSpawnPoint;
        [SerializeField, RequiredField()]
        private GameObject shipSpawnMenu;
        [SerializeField, RequiredField()]
        private GameObject hud;

        private UnityDictionary<SpawnableShipAttributes, SpawnMetaData> spawnMetaDatas =
            new UnityDictionary<SpawnableShipAttributes, SpawnMetaData>();

        private MothershipAttributesBonuses attributesBonuses;

        private List<ItemEffect> activeEffects;
        private List<StatusEffect> activeSpawnEffects;

        private float abilityTimer = 0f;

        private Camera mainCamera;

        private IEnumerator regenerateRoutine;
        
        #endregion

        #region Properties

        public MothershipAttributesBonuses AttributesBonuses => attributesBonuses;

        public UnityDictionary<SpawnableShipAttributes, SpawnMetaData> SpawnMetaDatas => spawnMetaDatas;

        public float AbilityTimer => abilityTimer;

        #endregion

        #region Unity Callbacks
        
        protected override void Awake() 
        {
            base.Awake();
            attributesBonuses = ScriptableObject.CreateInstance<MothershipAttributesBonuses>();
            IngameEffectApplier.OnEffectsChange = OnEffectsChange;
            IngameEffectApplier.Clear();
        }

        // Start runs once before the first update
        protected void Start()
        {
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
            
            DebugMonitorVariables();
        }

        #endregion

        #region IDamageable Overrides

        public override void Heal(float amount)
        {
            currentHealth.Value = Mathf.Min(GetMaxHealth(), currentHealth + amount);

            Transform cachedTransform = transform;

            if (Attributes.HealEffect != null)
            {
                PoolManager.Instance.Request(Attributes.HealEffect).
                    Emerge(cachedTransform.position, cachedTransform.rotation);
            }
        }

        #endregion
        
        #region Ship<T> Overrides

        /// <summary>
        /// Resets all instance-specific variables
        /// </summary>
        protected override void ResetInstanceVariables()
        {
            currentHealth.Value = GetMaxHealth();
            currentShield.Value = GetMaxShield();
            fireTimer = 0;
        }

        /// <summary>
        /// Fires the ship's weapons
        /// </summary>
        public override void Fire()
        {
            if (fireTimer > 0f || Time.timeScale == 0) return;

            for (int index = 0, upper = bulletSpawnPoints.Length; index < upper; index++)
            {
                PoolMember bullet = PoolManager.Instance.Request(attributes.Fire.Prefab);
                bullet.Emerge(bulletSpawnPoints[index].position, transform.rotation);

                bullet.transform.Rotate(0f, 0f,
                    Random.Range(Attributes.Fire.AngleJitter * -1f, Attributes.Fire.AngleJitter));
                bullet.GetComponent<BulletController>().DamageMultiplier = AttributesBonuses.DamageMultiplier;
            }

            fireTimer = GetFireCooldown();
        }

        /// <summary>
        /// Regenerates the ship's shields
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

        /// <summary>
        /// Gets whether the Mothership can currently regenerate its shields
        /// </summary>
        /// <returns>Whether the mothership can regenerate its shields</returns>
        public bool CanRegenShield()
        {
            return shieldRegenTimer > 0 &&
                   !Mathf.Approximately(Attributes.ShieldRegen, 0) &&
                   !Mathf.Approximately(CurrentShield.Value, GetMaxShield());
        }
        
        /// <summary>
        /// Gets the shield regen for the current point in time
        /// </summary>
        /// <returns>The shield regen for the current point in time</returns>
        public float GetShieldRegen()
        {
            return Attributes.ShieldRegen + AttributesBonuses.ShieldRegen * Time.deltaTime * Time.timeScale;
        }
        
        /// <summary>
        /// Gets the max health of the mothership
        /// </summary>
        /// <returns>The max health</returns>
        public float GetMaxHealth()
        {
            return Attributes.MaxHealth + AttributesBonuses.MaxHealth;
        }
        
        /// <summary>
        /// Gets the max shield of the mothership
        /// </summary>
        /// <returns>The max shield</returns>
        public float GetMaxShield()
        {
            return Attributes.MaxShield + AttributesBonuses.MaxShield;
        }
        
        /// <summary>
        /// Summons a ship of a given type
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

            SpawnedShip shipController = spawn.GetComponent<SpawnedShip>();

            // Adds the cooldown
            SpawnMetaDatas[shipType].SummonTimer.Value = GetMaxSpawnCooldown(shipType);
            SpawnMetaDatas[shipType].CurrentlyActive.Add(shipController);

            shipController.SpawnNumber = SpawnMetaDatas[shipType].CurrentlyActive.Count;
        }

        /// <summary>
        /// Removes a specific active summon
        /// </summary>
        /// <param name="shipToRemove">The active summon to remove</param>
        public void RemoveActiveSummon(SpawnedShip shipToRemove)
        {
            SpawnMetaDatas[shipToRemove.Attributes].CurrentlyActive.Remove(shipToRemove);
        }

        /// <summary>
        /// Gets the cost of spawning a specific ship
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
        /// Get the cooldown of spawning a specific ship
        /// </summary>
        /// <param name="shipType">The type of ship to get the cooldown of</param>
        /// <returns>The cooldown of said type of ship</returns>
        public float GetSpawnCooldown(SpawnableShipAttributes shipType)
        {
            return GetSpawnMetaData(shipType).SummonTimer.Value;
        }

        /// <summary>
        /// Gets whether the ship can be spawned (wouldn't exceed spawn limit)
        /// </summary>
        /// <param name="shipType">The type of ship to check for</param>
        /// <returns>Whether there is remaining space to spawn the ship</returns>
        public bool IsThereSpaceForSpawn(SpawnableShipAttributes shipType)
        {
            return GetSpawnMetaData(shipType).CurrentlyActive.Count + 1 <= GetMaxShipCount(shipType);
        }

        /// <summary>
        /// Gets the maximum ship count of a given ship type
        /// </summary>
        /// <param name="shipType">The ship type to check for maximum count</param>
        /// <returns>The maximum count of a given ship type</returns>
        public int GetMaxShipCount(SpawnableShipAttributes shipType)
        {
            return shipType.MaximumShips.Value + AttributesBonuses.ExtraSpawnSlots;
        }

        /// <summary>
        /// Gets whether the given ship type can be spawned at the moment
        /// </summary>
        /// <param name="shipType">The ship type to check for spawn ability</param>
        /// <returns>Whether the ship can be spawned</returns>
        public bool CanSpawnShip(SpawnableShipAttributes shipType)
        {
            return IsThereSpaceForSpawn(shipType) && GetSpawnCooldown(shipType) <= 0 && 
                   CurrentHealth > GetSpawnCost(shipType);
        }

        /// <summary>
        /// Gets the maximum spawn cooldown of a given ship type
        /// </summary>
        /// <param name="shipType">The type of the desired ship</param>
        /// <returns>The maximum cooldown of the given ship type</returns>
        public float GetMaxSpawnCooldown(SpawnableShipAttributes shipType)
        {
            return (float)shipType.SpawnCooldown * AttributesBonuses.SpawnCooldownMultiplier;
        }

        /// <summary>
        /// Generates spawn meta data for a given ship type
        /// </summary>
        /// <param name="shipType">The type to which generate a spawn meta data for</param>
        public SpawnMetaData GetSpawnMetaData(SpawnableShipAttributes shipType)
        {
            if (spawnMetaDatas.ContainsKey(shipType))
            {
                return spawnMetaDatas[shipType];
            }
            else
            {
                SpawnMetaDatas.Add(shipType, new SpawnMetaData(shipType));
                return spawnMetaDatas[shipType];
            }
        }

        /// <summary>
        /// Gets the maximum ability cooldown
        /// </summary>
        /// <returns>The maximum ability cooldown</returns>
        public float GetMaxAbilityCooldown()
        {
            return Attributes.RegenerateCooldown * AttributesBonuses.AbilityCooldownMultiplier;
        }

        /// <summary>
        /// Gets whether the ability can be used
        /// </summary>
        /// <returns>Whether the ability can be used</returns>
        public bool IsAbilityAvailable()
        {
            return AbilityTimer <= 0f;
        }

        /// <summary>
        /// Gets the speed of the ship for the current point in time
        /// </summary>
        /// <returns>The speed of the ship for the current point in time</returns>
        public float GetSpeed()
        {   
            return (Attributes.Speed + AttributesBonuses.SpeedIncrease) 
                   * AttributesBonuses.SpeedMultiplier * Time.timeScale * Time.deltaTime;
        }

        #endregion

        #region Private Methods

        private void DebugMonitorVariables()
        {
            Debug.Log($"<color=green>Health: {currentHealth.Value}/{GetMaxHealth()}</color> \n" +
                      $"<color=cyan>Shields: {CurrentShield.Value}/{GetMaxShield()}</color>, Can regen: <b>{CanRegenShield()}</b> \n" +
                      $"<color=orange>Ability: {abilityTimer}/{GetMaxAbilityCooldown()}</color>, Can use: <b>{IsAbilityAvailable()}</b>");
        }
        
        /// <summary>
        /// Ticks all timers related to the mothership
        /// </summary>
        private void TickTimers()
        {
            IngameEffectApplier.TickItems(Time.deltaTime);
            UpdateSummonTimers();
            abilityTimer = AbilityTimer - Time.deltaTime * Time.timeScale;
        }
        
        /// <summary>
        /// Handles player input
        /// </summary>
        private void HandlePlayerInput()
        {
            // Moves and rotates the ship
            Move();
            Look(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            
            // Enables or disables the spawn menu
            if (Input.GetKeyDown(KeyCode.Space) && !IsGameOver())
            {
                EnableOrDisableSpawnMenu(true);
            }

            if (Input.GetKeyUp(KeyCode.Space) && !IsGameOver())
            {
                EnableOrDisableSpawnMenu(false);
            }

            // Fires stuff
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Fire();
            }

            // Uses regen ability
            if (Input.GetKeyDown(KeyCode.R) && IsAbilityAvailable())
            {
                StartCoroutine(RegenerateShips());
            }
        }
        
        /// <summary>
        /// Gets the fire cooldown
        /// </summary>
        /// <returns>The fire cooldown</returns>
        private float GetFireCooldown()
        {
            return Attributes.Fire.Cooldown;
        }
        
        /// <summary>
        /// Gets whether the game has ended
        /// </summary>
        /// <returns>Whether the game has ended</returns>
        private bool IsGameOver()
        {
            return LevelManager.Instance.GameEnded;
        }
        
        /// <summary>
        /// Enables or disables the ship spawn menu
        /// </summary>
        /// <param name="enable">Whether to enable or disable the ship spawn menu</param>
        private void EnableOrDisableSpawnMenu(bool enable)
        {
            hud.SetActive(!enable);
            shipSpawnMenu.SetActive(enable);
        }

        /// <summary>
        /// Moves and rotates the Mothership
        /// </summary>
        private void Move()
        {
            // Gets movement input
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);

            // Translates
        
            transform.Translate(movement * GetSpeed(), Space.World);
        }

        /// <summary>
        /// Updates all spawn cooldowns
        /// </summary>
        private void UpdateSummonTimers()
        {
            if (SpawnMetaDatas.Count <= 0) return;

            foreach (var metaData in SpawnMetaDatas)
            {
                metaData.Value.SummonTimer.Value -= Time.deltaTime * Time.timeScale;
            }
        }

        /// <summary>
        /// Sacrifices all ships for health
        /// </summary>
        private IEnumerator RegenerateShips()
        {
            WaitForSeconds killInterval = new WaitForSeconds(Attributes.RegenerateKillInterval);
            abilityTimer = GetMaxAbilityCooldown();

            // For every spawned ship type
            foreach (var metaData in spawnMetaDatas)
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
            IngameEffectResult result = IngameEffectApplier.GetResult(attributes.ItemRegister,attributes.UpgradeRegister);
            
            attributesBonuses.HealthIncrease.Value = result.upgradeLifeIncrease;
            attributesBonuses.DamageIncrease.Value = result.upgradeDamageIncrease;
            attributesBonuses.ShieldIncrease.Value = result.upgradeShieldIncrease;
            attributesBonuses.SpeedIncrease.Value = result.upgradeSpeedIncrease;

            attributesBonuses.HealthRegen.Value = result.healthRegen;
            attributesBonuses.ShieldRegen.Value = result.shieldRegen;
            attributesBonuses.ExtraSpawnSlots.Value = result.spawnSlotBonus;
            attributesBonuses.SpawnCooldownMultiplier.Value = result.spawnCooldownMultiplierBonus;
            attributesBonuses.AbilityCooldownMultiplier.Value = result.abilityCooldownMultiplierBonus;
            attributesBonuses.MaxHealth.Value = result.maxHealthBonus;
            attributesBonuses.MaxShield.Value = result.maxShieldBonus;
            attributesBonuses.DamageMultiplier.Value = result.damageMultiplierBonus;
            attributesBonuses.SpeedMultiplier.Value = result.speedMultiplierBonus;
            attributesBonuses.Defense.Value = result.defenseBonus;
        }
        
        #endregion
    }
}