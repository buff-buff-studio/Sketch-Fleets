using ManyTools.Events;
using ManyTools.UnityExtended.Editor;
using ManyTools.UnityExtended.Poolable;
using ManyTools.Variables;
using SketchFleets.Data;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// A class that controls 
    /// </summary>
    /// <typeparam name="T">An attribute data structure that inherits from ShipAttributes</typeparam>
    [RequireComponent(typeof(AudioSource))]
    public class Ship<T> : PoolMember, IDamageable where T : ShipAttributes
    {
        #region Protected Fields

        [Header("Ship Properties")]
        [SerializeField, RequiredField()]
        protected T attributes;
        [SerializeField, RequiredField()]
        protected Transform[] bulletSpawnPoints;
        [SerializeField]
        private GameEvent deathEvent;
        [SerializeField]
        protected AudioSource soundSource;

        protected float fireTimer;

        #endregion

        #region Private Fields

        private FloatReference currentHealth = new FloatReference(0f);
        private FloatReference currentShield = new FloatReference(0f);

        #endregion

        #region Properties

        public FloatReference CurrentShield => currentShield;

        public FloatReference CurrentHealth => currentHealth;

        public T Attributes => attributes;

        #endregion

        #region IDamageable Implementation

        public void Damage(float amount)
        {
            currentHealth.Value -= amount / Attributes.Defense;

            soundSource.clip = Attributes.HitSound;

            if (soundSource.clip != null)
            {
                soundSource.Play();
            }

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

        #region PoolMember Overrides

        /// <summary>
        /// Emerges the Poolable object from the pool
        /// </summary>
        /// <param name="position">The position at which to emerge the object</param>
        /// <param name="rotation">The rotation to emerge the object with</param>
        public override void Emerge(Vector3 position, Quaternion rotation)
        {
            currentHealth.Value = attributes.MaxHealth.Value;
            currentShield.Value = attributes.MaxShield.Value;
            fireTimer = 0;
            
            base.Emerge(position, rotation);
        }

        #endregion
        
        #region Unity Callbacks

        // Start is called before the first update
        protected virtual void Start()
        {
            currentHealth.Value = attributes.MaxHealth.Value;
            currentShield.Value = attributes.MaxShield.Value;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            fireTimer -= Time.deltaTime;
        }

        protected void Reset()
        {
            soundSource = GetComponent<AudioSource>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Fires the ship's weapons
        /// </summary>
        public virtual void Fire()
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

            fireTimer = Attributes.Fire.Cooldown;
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
            if (Attributes.DeathEffect != null)
            {
                Instantiate(Attributes.DeathEffect, transform.position, Quaternion.identity);
            }

            if (deathEvent != null)
            {
                deathEvent.Invoke();
            }

            Destroy(gameObject);
        }

        #endregion
    }
}