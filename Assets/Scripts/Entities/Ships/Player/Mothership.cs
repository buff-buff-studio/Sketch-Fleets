using System.Collections;
using UnityEngine;
using SketchFleets.Data;
using System.Collections.Generic;
using ManyTools.UnityExtended.Editor;
using ManyTools.UnityExtended.Poolable;
using ManyTools.Variables;

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
        [Tooltip("The speed at which the Mothership moves at permanently")]
        [SerializeField]
        private Vector2Reference backgroundSpeed;

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

        [SerializeField]
        private GameObject circleShips;

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
        protected void Start()
        {
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
            
            // Fires stuff
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Fire();
            }

        }

        #endregion

        #region Ship<T> Overrides

        /// <summary>
        /// Fires the ship's weapons
        /// </summary>
        public override void Fire()
        {
            if (fireTimer > 0f) return;
            
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
            if (!spawnMetaDatas.ContainsKey(shipType))
            {
                spawnMetaDatas.Add(shipType, new SpawnMetaData(shipType));
            }
            
            // If the new spawn would exceed the maximum amount, return
            if (spawnMetaDatas[shipType].CurrentlyActive.Count + 1 > shipType.MaximumShips.Value + extraSpawnSlots ||
                spawnMetaDatas[shipType].SummonTimer.Value > 0)
            {
                return;
            }
            
            Damage(shipType.GraphiteCost + 
                   shipType.GraphiteCostIncrease * spawnMetaDatas[shipType].CurrentlyActive.Count);

            // Spawns the ship
            PoolMember spawn = PoolManager.Instance.Request(shipType.Prefab);
            spawn.Emerge(shipSpawnPoint.position, Quaternion.identity);
            
            SpawnedShip shipController = spawn.GetComponent<SpawnedShip>();
            
            // Adds the cooldown
            spawnMetaDatas[shipType].SummonTimer.Value = shipType.SpawnCooldown.Value * spawnCooldownMultipler;
            spawnMetaDatas[shipType].CurrentlyActive.Add(shipController);

            shipController.SpawnNumber = spawnMetaDatas[shipType].CurrentlyActive.Count;
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
            
            // Caches time-based speed and input
            float timeSpeed = attributes.Speed * Time.deltaTime;

            // Caches transform to avoid repeated marshalling
            Transform transformCache = transform;

            // Translates
            transformCache.Translate((movement * timeSpeed) + backgroundSpeed.Value * Time.deltaTime
                , Space.World);
        }

        /// <summary>
        /// Updates all spawn cooldowns
        /// </summary>
        private void UpdateSummonTimers()
        {
            if (spawnMetaDatas.Count <= 0) return;

            foreach (var metaData in spawnMetaDatas)
            {
                metaData.Value.SummonTimer.Value -= Time.deltaTime * Time.timeScale;
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