using System.Collections;
using System.Collections.Generic;
using SketchFleets.Enemies;
using SketchFleets.Entities;
using UnityEngine;

namespace SketchFleets
{
    public class ContinuousDamageBulletController : BulletController
    {
        protected override void DealDamageToTarget(bool directDamage, GameObject target)
        {
            if (target.CompareTag("Player") || target.CompareTag("PlayerSpawn"))
            {
                if (Attributes.IgnorePlayer) return;
                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
                if(target.CompareTag("Player"))
                    StartCoroutine(target.GetComponent<Mothership>()?.ContinuousDamage(GetDamage(directDamage), Attributes.ContinuousDamageTime));
                else
                    StartCoroutine(target.GetComponent<SpawnedShip>()?.ContinuousDamage(GetDamage(directDamage), Attributes.ContinuousDamageTime));
            }
            else
            {
                target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
                StartCoroutine(target.GetComponent<EnemyShip>()?.ContinuousDamage(GetDamage(directDamage), Attributes.ContinuousDamageTime));
            }
        }
    }
}
