using System.Collections;
using System.Collections.Generic;
using SketchFleets.AI;
using SketchFleets.Data;
using UnityEngine;

namespace SketchFleets
{
    public class ZombieBulletController : BulletController
    {
        protected override void DealDamageToTarget(bool directDamage, GameObject target)
        {
            if (target.CompareTag("Player") || target.CompareTag("PlayerSpawn"))
            {
                if (Attributes.IgnorePlayer) return;
                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
                if(target.GetComponent<IShipAI>().Faction == ShipAttributes.Faction.Friendly)
                    target.GetComponent<IShipAI>().FlipFaction();
            }
            else
            {
                if (target.CompareTag("Enemy"))
                {
                    if(target.GetComponent<IShipAI>().Faction == ShipAttributes.Faction.Hostile)
                        target.GetComponent<IShipAI>().FlipFaction();
                }

                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
            }
            
            Submerge();
        }
    }
}
