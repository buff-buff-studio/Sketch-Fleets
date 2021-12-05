using System;
using System.Collections;
using ManyTools.UnityExtended.Poolable;
using ManyTools.Variables;
using SketchFleets.Data;
using SketchFleets.Systems.DeathContext;
using Unity.Mathematics;
using UnityEngine;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that handles an obstacle
    /// </summary>
    public class Obstacle : MonoBehaviour, IDamageable
    {
        #region Private Fields

        [SerializeField]
        private ObstacleAttributes attributes;
        
        private FloatReference currentHealth;

        #endregion

        #region Properties

        public ObstacleAttributes Attributes => attributes;

        #endregion

        #region IDamageable Implementation

        public DamageContext LatestDamageContext { get; set; }

        public void Damage(float amount, DamageContext context, bool makeInvulnerable = false, bool piercing = false)
        {
            currentHealth.Value -= amount;
            LatestDamageContext = context;

            if (CheckForDeath())
            {
                Die();
            }
        }

        public void DamageContinually(float amount, DamageContext context, int pulses, float time)
        {
            StartCoroutine(ApplyDamagePulses(amount, context, pulses, time));
        }
        
        /// <summary>
        /// Heals the Damageable object by the given amount
        /// </summary>
        /// <param name="amount">The amount to damage for</param>
        public void Heal(float amount)
        {
            currentHealth.Value += amount;
        }

        #endregion
        
        #region Unity Callbacks

        private void Start()
        {
            Initialize();
            Destroy(gameObject, 30f);
        }

        private void Update()
        {
            Move();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Attributes.CollideWithPlayerOnly)
            {
                if (!other.CompareTag("Player")) return;
            }
            
            DamageOrHealTarget(other.GetComponent<IDamageable>());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the obstacle
        /// </summary>
        private void Initialize()
        {
            // Sets the current health to max health
            currentHealth = new FloatReference(Attributes.MaxHealth.Value);
        }
        
        /// <summary>
        /// Moves the obstacle in a given direction
        /// </summary>
        private void Move()
        {
            Vector2 temporalSpeed = (Time.deltaTime * Time.timeScale) * Attributes.Motion.Value;
            transform.Translate(temporalSpeed);
        }
        
        /// <summary>
        /// Damages or heals the given target
        /// </summary>
        /// <param name="damageable">The damageable target</param>
        private void DamageOrHealTarget(IDamageable damageable)
        {
            if (Attributes.CollisionDamage > 0)
            {
                damageable?.Damage(Attributes.CollisionDamage, DamageContext.ObstacleCollision);
            }
            else
            {
                damageable?.Heal(math.abs(Attributes.CollisionDamage));
            }
        }

        /// <summary>
        /// Checks if the obstacle should die
        /// </summary>
        /// <returns>Whether the obstacle should die</returns>
        private bool CheckForDeath()
        {
            return currentHealth.Value <= 0;    
        }

        /// <summary>
        /// Destroys the obstacle
        /// </summary>
        private void Die()
        {
            PoolManager.Instance.Request(Attributes.DeathEffect).Emerge(transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        /// <summary>
        /// Applies multiple pulses of damage to the ship
        /// </summary>
        /// <param name="totalDamage">The total damage to be applied</param>
        /// <param name="context">The context of the damage to apply</param>
        /// <param name="pulses">The amount of pulses to play</param>
        /// <param name="time">The time to apply the pulses over</param>
        /// <exception cref="ArgumentException">The amount of pulses cannot be negative</exception>
        private IEnumerator ApplyDamagePulses(float totalDamage, DamageContext context, int pulses, float time)
        {
            if (pulses <= 0)
            {
                throw new ArgumentException("Pulses must be greater than 0");
            }

            WaitForSeconds pulseInterval = new WaitForSeconds(time / pulses);
            float damagePerPulse = totalDamage / pulses;
            
            for (int pulse = 0; pulse < pulses; pulse++)
            {
                Damage(damagePerPulse, context);
                yield return pulseInterval;
            }
        }
        
        #endregion
    }
}