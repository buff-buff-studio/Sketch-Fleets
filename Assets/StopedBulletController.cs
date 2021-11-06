using System.Collections;
using System.Collections.Generic;
using SketchFleets.Enemies;
using UnityEngine;

namespace SketchFleets
{
    public class StopedBulletController : BulletController
    {
        [SerializeField] 
        private bool isAreaDamage;

        protected override void DealDamageToTarget(bool directDamage, GameObject target)
        {
            if (target.CompareTag("Player") || target.CompareTag("PlayerSpawn"))
            {
                if (Attributes.IgnorePlayer) return;
                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
            }
            else
            {
                if(target.CompareTag("Enemy"))
                    target.GetComponent<EnemyShip>().Lock(Attributes.HitsLock, Attributes.Downtime);
                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
            }
        }
    }
}
