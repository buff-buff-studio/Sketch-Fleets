using System.Collections;
using System.Collections.Generic;
using SketchFleets.General;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// A class that handles life-stealing bullets
    /// </summary>
    public sealed class HealingBulletController : BulletController
    {
        protected override void DealDamageToTarget(bool directDamage, GameObject target)
        {
            if (target.CompareTag("Player") || target.CompareTag("PlayerSpawn") && Attributes.IgnorePlayer) return;
            
            target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage), Attributes.DamageContext);
            LevelManager.Instance.Player.Heal(GetDamage(directDamage));
        }
    }
}