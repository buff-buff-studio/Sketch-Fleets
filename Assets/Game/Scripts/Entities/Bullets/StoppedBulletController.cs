using SketchFleets.Enemies;
using SketchFleets.Entities;
using UnityEngine;

namespace SketchFleets
{
    public sealed class StoppedBulletController : BulletController
    {
        protected override void DealDamageToTarget(bool directDamage, GameObject target)
        {
            if (target.CompareTag("Player"))
            {
                if (Attributes.IgnorePlayer) return;
                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
                target.GetComponent<Mothership>().Lock(Attributes.HitsLock, Attributes.Downtime);
            }
            else if (target.CompareTag("PlayerSpawn"))
            {
                if (Attributes.IgnorePlayer) return;
                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
                target.GetComponent<SpawnedShip>().Lock(Attributes.HitsLock, Attributes.Downtime);
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