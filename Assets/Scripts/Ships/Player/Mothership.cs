using UnityEngine;
using SketchFleets.Data;
using UnityEngine.Serialization;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that controls the mothership
    /// </summary>
    [RequireComponent(typeof(ShipGenerator))]
    public class Mothership : MonoBehaviour
    {
        #region Private Fields

        [SerializeField]
        private MothershipAttributes attributes;
        [SerializeField, FormerlySerializedAs("BulletSpawn")] 
        private Transform bulletSpawnPoint;
        [SerializeField, FormerlySerializedAs("CyanShipsSpawner")] 
        private Transform cyanShipsSpawnPoint;
        
        private float currentHealth;
        private float currentSpeed;
        private float currentShield;
        private float fireTimer = 0;
        private int activeCyanShips;

        private Camera mainCamera;
        private ShipGenerator shipGenerator;

        #endregion

        #region Properties
        
        public Transform CyanShipSpawnPoint
        {
            get => cyanShipsSpawnPoint;
        }

        public Transform BulletSpawnPoint
        {
            get => bulletSpawnPoint;
        }

        public float CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = value;
        }

        public float CurrentShield
        {
            get => currentShield;
            set => currentShield = value;
        }

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            // Caches necessary components
            mainCamera = Camera.main;
            shipGenerator = GetComponent<ShipGenerator>();
            
            // Gets
            CurrentHealth = attributes.MaxHealth;
            currentSpeed = attributes.Speed;
            CurrentShield = attributes.MaxShield;
        }

        private void Update()
        {
            Movement();
            Fire();
            MothershipCyanShoot();

            // Decrements fire timer
            fireTimer -= Time.deltaTime;
        }

        #endregion

        #region Commands Mothership

        /// <summary>
        /// Moves and rotates the Mothership
        /// </summary>
        private void Movement()
        {
            // Gets movement input
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            if (movement == Vector3.zero) return;

            // Caches time-based speed and input
            float timeSpeed = attributes.Speed * Time.deltaTime;

            // Caches transform to avoid repeated marshalling
            Transform transformCache = transform;
            
            // Translates
            transformCache.Translate(movement * timeSpeed, Space.World);
            // Rotates
            Vector3 dir = Input.mousePosition - mainCamera.WorldToScreenPoint(transformCache.position);
            float angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        /// <summary>
        /// Fires with the mothership
        /// </summary>
        private void Fire()
        {
            // TODO: Add remappable controls
            if (!Input.GetKey(KeyCode.Mouse0)) return;
            if (!(fireTimer <= 0)) return;
            
            Instantiate(attributes.Fire.Prefab, bulletSpawnPoint.position, transform.rotation);
            fireTimer = attributes.FireCooldown;
        }

        /// <summary>
        /// Mouse 1 launch the cyan ship
        /// </summary>
        private void MothershipCyanShoot()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse1)) return;
            
            activeCyanShips = shipGenerator.CyanShips;

            if (activeCyanShips <= 0) return;
            
            // TODO: remove this GetChild call
            Rigidbody2D cyanRigidbody = cyanShipsSpawnPoint.GetChild(2).GetComponent<Rigidbody2D>();
            
            cyanRigidbody.AddForce(cyanRigidbody.transform.GetChild(1).up * 100f, ForceMode2D.Impulse);
            cyanRigidbody.transform.parent = transform.parent;
            shipGenerator.CyanShips--;
        }

        #endregion
    }
}
