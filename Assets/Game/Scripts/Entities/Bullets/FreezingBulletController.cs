using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// A class that manages a bullet that freezes ships
    /// </summary>
    public sealed class FreezingBulletController : BulletController
    {
        protected override void DealDamageToTarget(bool directDamage, GameObject target)
        {
            if (target.CompareTag("Player") || target.CompareTag("PlayerSpawn"))
            {
                if (Attributes.IgnorePlayer) return;
            }

            target.GetComponent<IFreezable>()?.Freeze(Attributes.Downtime);
            target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));

            Submerge();
        }
    }
}