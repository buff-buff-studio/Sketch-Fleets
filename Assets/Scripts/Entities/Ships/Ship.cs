using ManyTools.Events;
using ManyTools.UnityExtended.Editor;
using ManyTools.Variables;
using SketchFleets.Data;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// A class that controls 
    /// </summary>
    /// <typeparam name="T">An attribute data structure that inherits from ShipAttributes</typeparam>
    public class Ship<T> : MonoBehaviour, IDamageable where T : ShipAttributes
    {
        #region Protected Fields

        [Header("Ship Properties")]
        [SerializeField, RequiredField()]
        protected T attributes;
        [SerializeField, RequiredField()]
        protected Transform[] bulletSpawnPoints;
        [SerializeField]
        private GameEvent deathEvent;

        protected float fireTimer;

        #endregion

        #region Private Fields

        private FloatReference currentHealth;
        private FloatReference currentShield;

        #endregion

        #region Properties

        public FloatReference CurrentShield
        {
            get => currentShield;
            set => currentShield = value;
        }

        public FloatReference CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = value;
        }

        protected T Attributes
        {
            get => attributes;
            set => attributes = value;
        }

        #endregion

        #region IDamageable Implementation

        public void Damage(float amount)
        {
            currentHealth.Value -= amount;

            if (currentHealth <= 0f)
            {
                Die();
            }

        }
        public void Heal(float amount)
        {
            currentHealth.Value = Mathf.Min(attributes.MaxHealth, currentHealth + amount);
        }

        #endregion

        #region Unity Callbacks

        // Start is called before the first update
        protected virtual void Start()
        {
            currentHealth = attributes.MaxHealth;
            currentShield = attributes.MaxShield;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            fireTimer -= Time.deltaTime;

            if (Input.GetKey(KeyCode.Mouse0) && fireTimer <= 0)
            {
                Fire();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Fires the ship's weapons
        /// </summary>
        public virtual void Fire()
        {
            for (int index = 0, upper = bulletSpawnPoints.Length; index < upper; index++)
            {
                Instantiate(attributes.Fire.Prefab, bulletSpawnPoints[index].position, transform.rotation);
            }

            fireTimer += attributes.FireCooldown;
        }

        /// <summary>
        /// Makes the ship look at a point
        /// </summary>
        /// <param name="target">Where to look at</param>
        public virtual void Look(Vector2 target)
        {
            Transform transformCache = transform;
            transformCache.up = (Vector3)target - transformCache.position;
        }

        #endregion


        #region Protected Methods

        /// <summary>
        /// Makes the ship die
        /// </summary>
        protected virtual void Die()
        {
            Destroy(gameObject);

            if (deathEvent != null)
            {
                deathEvent.Invoke();
            }
        }

        #endregion
    }
}