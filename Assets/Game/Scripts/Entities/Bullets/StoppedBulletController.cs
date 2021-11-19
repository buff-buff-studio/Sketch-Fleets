using SketchFleets.Enemies;
using UnityEngine;

namespace SketchFleets
{
    public sealed class StoppedBulletController : BulletController
    {
        protected override void DealDamageToTarget(bool directDamage, GameObject target)
        {
            if (target.CompareTag("Player") || target.CompareTag("PlayerSpawn"))
            {
                if (Attributes.IgnorePlayer) return;
                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
            }
            else
            {
                if (target.CompareTag("Enemy"))
                {
                    target.GetComponent<EnemyShip>().Lock(Attributes.HitsLock, Attributes.Downtime);
                }

                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
            }
            
            Submerge();
        }
    }
}