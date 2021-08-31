using ManyTools.UnityExtended.Poolable;
using ManyTools.Variables;
using SketchFleets.Data;
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

        /// <summary>
        /// Damages the Damageable object by the given amount
        /// </summary>
        /// <param name="amount">The amount to damage for</param>
        /// <param name="makeInvulnerable">Whether the object should be made invulnerable after the damage</param>
        /// <param name="piercing">Whether the damage should ignore defense and shields</param>
        public void Damage(float amount, bool makeInvulnerable = false, bool piercing = false)
        {
            currentHealth.Value -= amount;

            if (CheckForDeath())
            {
                Die();
            }
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
                damageable?.Damage(Attributes.CollisionDamage, true);
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
        
        #endregion
    }
}