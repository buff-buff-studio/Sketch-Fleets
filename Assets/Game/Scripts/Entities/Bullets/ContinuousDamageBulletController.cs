using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// A class that handles a bullet that deals continuous damge
    /// </summary>
    public sealed class ContinuousDamageBulletController : BulletController
    {
        protected override void DealDamageToTarget(bool directDamage, GameObject target)
        {
            if ((target.CompareTag("Player") || target.CompareTag("PlayerSpawn")) && Attributes.IgnorePlayer) return;

            target.GetComponent<IDamageable>()?.DamageContinually(GetDamage(directDamage),
                (int)Attributes.ContinuousDamageTime, Attributes.ContinuousDamageTime);
            
            Submerge();
        }
    }
}