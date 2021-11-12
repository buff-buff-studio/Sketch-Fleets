using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets
{
    public class HealingBulletController : BulletController
    {
        protected IDamageable Mothership;

        protected override void Start()
        {
            base.Start();
            Mothership = GameObject.FindWithTag("Player").GetComponent<IDamageable>();
        }
        protected override void DealDamageToTarget(bool directDamage, GameObject target)
        {
            Mothership.Heal(GetDamage(directDamage));
            if (target.CompareTag("Player") || target.CompareTag("PlayerSpawn"))
            {
                if (Attributes.IgnorePlayer) return;
                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
            }
            else
            {
                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
            }
        }
    }
}
