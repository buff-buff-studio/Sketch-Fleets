using System.Collections;
using UnityEngine;
using SketchFleets.Data;
using ManyTools.Variables;
using System.Collections.Generic;
using ManyTools.UnityExtended.Editor;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that controls the mothership
    /// </summary>
    public class Mothership : Ship<MothershipAttributes>
    {
        #region Private Fields

        [SerializeField, RequiredField()]
        private Transform shipSpawnPoint;
        [SerializeField]
        private FloatReference explosionDamage;

        private Dictionary<SpawnableShipAttributes, SpawnMetaData> spawnMetaDatas =
            new Dictionary<SpawnableShipAttributes, SpawnMetaData>();

        private float extraSpawnSlots = 0f;
        private float abilityCooldownMultiplier = 1f;
        private float spawnCooldownMultipler = 1f;
        private float fireTimerModifier = 1f;

        private List<ItemEffect> activeEffects;
        private List<StatusEffect> activeSpawnEffects;

        private float abilityTimer = 0f;

        private Camera mainCamera;

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

        #endregion

        #region Unity Callbacks

        // Start runs once before the first update
        protected override void Start()
        {
            base.Start();
            
            // Generates spawn meta data
            GenerateSpawnMetaData();

            // Caches necessary components
            mainCamera = Camera.main;
        }

        // Start runs once every frame
        protected override void Update()
        {
            base.Update();

            // Moves and rotates the ship
            Move();
            Look(mainCamera.ScreenToWorldPoint(Input.mousePosition));

            //MothershipCyanShoot();
            
            // Ticks down summon timers
            UpdateSummonTimers();
        }

        #endregion

        #region Ship<T> Overrides

        /// <summary>
        /// Fires the ship's weapons
        /// </summary>
        public override void Fire()
        {
            for (int index = 0, upper = bulletSpawnPoints.Length; index < upper; index++)
            {
                Instantiate(attributes.Fire.Prefab, bulletSpawnPoints[index].position, transform.rotation);
            }

            fireTimer = attributes.FireCooldown * fireTimerModifier;

            FireSpawns();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Summons a ship of a given type
        /// </summary>
        /// <param name="shipType">The ship type to spawn</param>
        public void SummonShip(SpawnableShipAttributes shipType)
        {
            // If the new spawn would exceed the maximum amount, return
            if (spawnMetaDatas[shipType].CurrentlyActive.Count + 1 > shipType.MaximumShips.Value)
            {
                return;
            }

            // Spawns the ship
            GameObject spawn = Instantiate(shipType.Prefab, shipSpawnPoint.position, Quaternion.identity);
            Ship<SpawnableShipAttributes> shipController = GetComponent<Ship<SpawnableShipAttributes>>();

            // Adds the cooldown
            spawnMetaDatas[shipType].SummonTimer.Value += shipType.SpawnCooldown * spawnCooldownMultipler;
            spawnMetaDatas[shipType].CurrentlyActive.Add(shipController);
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Moves and rotates the Mothership
        /// </summary>
        private void Move()
        {
            // Gets movement input
            Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (movement == Vector2.zero) return;

            // Caches time-based speed and input
            float timeSpeed = attributes.Speed * Time.deltaTime;

            // Caches transform to avoid repeated marshalling
            Transform transformCache = transform;

            // Translates
            transformCache.Translate(movement * timeSpeed, Space.World);
        }

        // /// <summary>
        // /// Mouse 1 launch the cyan ship
        // /// </summary>
        // private void MothershipCyanShoot()
        // {
        //     if (!Input.GetKeyDown(KeyCode.Mouse1)) return;
        //
        //     activeCyanShips = shipGenerator.CyanShips;
        //
        //     if (activeCyanShips <= 0) return;
        //
        //     // TODO: remove this GetChild call
        //     Rigidbody2D cyanRigidbody = cyanShipsSpawnPoint.GetChild(2).GetComponent<Rigidbody2D>();
        //
        //     cyanRigidbody.AddForce(cyanRigidbody.transform.GetChild(1).up * 100f, ForceMode2D.Impulse);
        //     cyanRigidbody.transform.parent = transform.parent;
        //     shipGenerator.CyanShips--;
        // }

        /// <summary>
        /// Updates all spawn cooldowns
        /// </summary>
        private void UpdateSummonTimers()
        {
            if (spawnMetaDatas.Count <= 0) return;

            foreach (var metaData in spawnMetaDatas)
            {
                metaData.Value.SummonTimer.Value -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Fires all spawned ship
        /// </summary>
        private void FireSpawns()
        {
            if (spawnMetaDatas.Count <= 0) return;

            foreach (var metaData in spawnMetaDatas)
            {
                for (int index = 0, upper = metaData.Value.CurrentlyActive.Count; index < upper; index++)
                {
                    metaData.Value.CurrentlyActive[index].Fire();
                }
            }
        }

        /// <summary>
        /// Generates spawn metadata for all spawnable ships
        /// </summary>
        private void GenerateSpawnMetaData()
        {
            if (attributes.SpawnableShips.Count <= 0)
            {
                Debug.LogWarning($"There are no spawnable ships in the Mothership's attributes!");
                return;
            }
            
            for (int index = 0, upper = attributes.SpawnableShips.Count; index < upper; index++)
            {
                spawnMetaDatas.Add(attributes.SpawnableShips[index], new SpawnMetaData(attributes
                    .SpawnableShips[index]));
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
                
            }
            
            // Waits interval
            yield return secondInterval;
        }

        #endregion
    }
}