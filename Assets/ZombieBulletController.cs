using SketchFleets.AI;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// A class that controls the zombie 'mind control' bullet
    /// </summary>
    public sealed class ZombieBulletController : BulletController
    {
        protected override void DealDamageToTarget(bool directDamage, GameObject target)
        {
            if (target.CompareTag("Player"))
            {
                if (Attributes.IgnorePlayer) return;
                ApplyDamage(directDamage, target);
            }
            else if (target.CompareTag("PlayerSpawn") || target.CompareTag("Enemy"))
            {
                ApplyDamage(directDamage, target);
                FlipTargetsFaction(target);
            }
            
            Submerge();
        }

        /// <summary>
        /// Flips the target's faction
        /// </summary>
        /// <param name="target">The target to flip the faction of</param>
        private static void FlipTargetsFaction(GameObject target)
        {
            if (target.TryGetComponent(out IShipAI targetAI))
            {
                targetAI.FlipFaction();
            }
        }

        /// <summary>
        /// Applies damage to the target
        /// </summary>
        /// <param name="directDamage">Whether damage is direct</param>
        /// <param name="target">The target to apply damage to</param>
        private void ApplyDamage(bool directDamage, GameObject target)
        {
            target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
        }
    }
}