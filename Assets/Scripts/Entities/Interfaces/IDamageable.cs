using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets
{
    public interface IDamageable
    {
        /// <summary>
        /// Damages the Damageable object by the given amount
        /// </summary>
        /// <param name="amount">The amount to damage for</param>
        public void Damage(float amount);
        
        /// <summary>
        /// Heals the Damageable object by the given amount
        /// </summary>
        /// <param name="amount">The amount to damage for</param>
        public void Heal(float amount);
    }
}
