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
        /// <param name="makeInvulnerable">Whether the object should be made invulnerable after the damage</param>
        public void Damage(float amount, bool makeInvulnerable = false);
        
        /// <summary>
        /// Heals the Damageable object by the given amount
        /// </summary>
        /// <param name="amount">The amount to damage for</param>
        public void Heal(float amount);
    }
}

// NOTE: In hindsight, this shouldn't have been an interface, but rather a component. Building this as an
// interface has made this too inflexible for our needs, and now I need to abuse the interface's functionalities
// to accomplish certain things, like collision damage.
