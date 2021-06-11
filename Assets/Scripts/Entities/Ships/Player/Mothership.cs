using System.Collections;
using UnityEngine;
using SketchFleets.Data;
using System.Collections.Generic;
using ManyTools.UnityExtended.Editor;
using ManyTools.UnityExtended.Poolable;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using ManyTools.UnityExtended;

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

        private UnityDictionary<SpawnableShipAttributes, SpawnMetaData> spawnMetaDatas =
            new UnityDictionary<SpawnableShipAttributes, SpawnMetaData>();

        private float extraSpawnSlots = 0f;
        private float abilityCooldownMultiplier = 1f;
        private float spawnCooldownMultipler = 1f;
        private float fireTimerModifier = 1f;

        private List<ItemEffect> activeEffects;
        private List<StatusEffect> activeSpawnEffects;

        private float abilityTimer = 0f;

        private Camera mainCamera;

        private IEnumerator regenerateRoutine;

        #endregion

        #region Properties

        public List<ItemEffect> ActiveEffects
        {
            get => activeEffects;
            set => activeEffects = value;
        }

        public List<StatusEffect> ActiveSpawnEffects
        {
            get => activeSpawnEffects;
            set => activeSpawnEffects = value;
        }

        public UnityDictionary<SpawnableShipAttributes, SpawnMetaData> SpawnMetaDatas => spawnMetaDatas;

        public float AbilityTimer => abilityTimer;

        #endregion

        #region Unity Callbacks

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

            // Moves and rotates the ship
            Move();
            Look(mainCamera.ScreenToWorldPoint(Input.mousePosition));

            // Ticks down timers
            UpdateSummonTimers();
            abilityTimer = AbilityTimer - Time.deltaTime * Time.timeScale;

            // Enables or disables the spawn menu
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EnableOrDisableSpawnMenu(true);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                EnableOrDisableSpawnMenu(false);
            }

            // Fires stuff
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Fire();
            }

            if (Input.GetKeyDown(KeyCode.R) && IsAbilityAvailable())
            {
                StartCoroutine(regenerateRoutine);
            }
        }

        #endregion

        #region Ship<T> Overrides

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
                bullet.GetComponent<BulletController>().BarrelAttributes = Attributes;
            }

            fireTimer = attributes.Fire.Cooldown * fireTimerModifier;
        }

        #endregion

        #region Public Methods

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
            SpawnMetaDatas[shipType].SummonTimer.Value = shipType.SpawnCooldown.Value * spawnCooldownMultipler;
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
            return GetSpawnMetaData(shipType).CurrentlyActive.Count + 1 <= shipType.MaximumShips.Value + extraSpawnSlots;
        }

        /// <summary>
        /// Gets whether the given ship type can be spawned at the moment
        /// </summary>
        /// <param name="shipType">The ship type to check for spawn ability</param>
        /// <returns>Whether the ship can be spawned</returns>
        public bool CanSpawnShip(SpawnableShipAttributes shipType)
        {
            return IsThereSpaceForSpawn(shipType) && GetSpawnCooldown(shipType) <= 0;
        }

        /// <summary>
        /// Gets the maximum spawn cooldown of a given ship type
        /// </summary>
        /// <param name="shipType">The type of the desired ship</param>
        /// <returns>The maximum cooldown of the given ship type</returns>
        public float GetMaxSpawnCooldown(SpawnableShipAttributes shipType)
        {
            return (float)shipType.SpawnCooldown * spawnCooldownMultipler;
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
            return Attributes.RegenerateCooldown * abilityCooldownMultiplier;
        }

        /// <summary>
        /// Gets whether the ability can be used
        /// </summary>
        /// <returns>Whether the ability can be used</returns>
        public bool IsAbilityAvailable()
        {
            return AbilityTimer <= 0f;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Enables or disables the ship spawn menu
        /// </summary>
        /// <param name="enable">Whether to enable or disable the ship spawn menu</param>
        private void EnableOrDisableSpawnMenu(bool enable)
        {
            shipSpawnMenu.SetActive(enable);
        }

        /// <summary>
        /// Moves and rotates the Mothership
        /// </summary>
        private void Move()
        {
            // Gets movement input
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);

            // Caches time-based speed and input
            float timeSpeed = attributes.Speed * Time.deltaTime * Time.timeScale;

            // Caches transform to avoid repeated marshalling
            Transform transformCache = transform;

            // Translates
            transformCache.Translate(movement * timeSpeed, Space.World);
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

        /// <summary>
        /// Applies status effects to the mothership
        /// </summary>
        /// <returns></returns>
        private IEnumerator TickStatusEffects()
        {
            // Caches tick interval
            WaitForSeconds secondInterval = new WaitForSeconds(1f);

            for (int index = 0, upper = ActiveEffects.Count; index < upper; index++)
            {
                //ActiveEffects[index]
            }

            // Waits interval
            yield return secondInterval;
        }

        #endregion
    }
}