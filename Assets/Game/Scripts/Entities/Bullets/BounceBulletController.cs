using UnityEngine;
using Random = UnityEngine.Random;

namespace SketchFleets
{
    /// <summary>
    /// A class that controls a bouncy bullet
    /// </summary>
    public sealed class BounceBulletController : BulletController
    {
        #region Private Fields

        [SerializeField]
        private float maxBounceLife = 4;
        [SerializeField]
        private GameObject bounceEffect;

        private float bounceLife;
        private Vector3 originalScale;

        #endregion

        #region IPoolable Overrides

        public override void Emerge(Vector3 position, Quaternion rotation)
        {
            bounceLife = maxBounceLife;
            transform.localScale = originalScale;

            base.Emerge(position, rotation);
        }

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            originalScale = transform.localScale;
        }

        protected override void Update()
        {
            Move(Vector3.up * Attributes.Speed, Space.Self);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Bounce(other.gameObject.CompareTag("bullet"));
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            Bounce(col.gameObject.CompareTag("bullet"));
            DealDamageToTarget(false, col.gameObject);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Deals damage to a specified target
        /// </summary>
        /// <param name="directDamage">Whether the damage should ignore armor</param>
        /// <param name="target">The target to deal damage to</param>
        protected override void DealDamageToTarget(bool directDamage, GameObject target)
        {
            if (IsPlayerOrSpawnedShip(target))
            {
                if (Attributes.IgnorePlayer) return;
                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage), Attributes.DamageContext);
            }
            else
            {
                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage), Attributes.DamageContext);
            }
        }

        /// <summary>
        /// Checks whether a target is a player or a spawned ship
        /// </summary>
        /// <param name="target">The target to check</param>
        /// <returns>Whether a target is a player or a spawned ship</returns>
        private static bool IsPlayerOrSpawnedShip(GameObject target)
        {
            return target.CompareTag("Player") || target.CompareTag("PlayerSpawn");
        }

        /// <summary>
        /// Bounces the bullet
        /// </summary>
        /// <param name="ignoreCooldown">Whether it should ignore cooldown</param>
        private void Bounce(bool ignoreCooldown)
        {
            if (!gameObject.activeSelf || ignoreCooldown) return;

            transform.Rotate(new Vector3(0, 0, Random.Range(80, 101)));
            bounceLife--;

            if (CanBounceAgain())
            {
                transform.localScale *= .85f;
            }

            if (BouncesHaveEnded())
            {
                Submerge();
            }
            
            PlayHitEffect();
        }

        /// <summary>
        /// Checks whether all the bullet's bounces have ended.
        /// </summary>
        /// <returns>Whether all bounces have ended</returns>
        private bool BouncesHaveEnded()
        {
            return bounceLife <= 0 && gameObject.activeSelf;
        }

        /// <summary>
        /// Checks whether the bullet can bounce again.
        /// </summary>
        /// <returns>Whether the bullet can bounce again.</returns>
        private bool CanBounceAgain()
        {
            return bounceLife < maxBounceLife - 1;
        }

        #endregion
    }
}