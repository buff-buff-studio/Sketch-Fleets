using SketchFleets.Systems.DeathContext;

namespace SketchFleets
{
    public interface IDamageable
    {
        public DamageContext LatestDamageContext { get; set; }

        /// <summary>
        /// Damages the Damageable object by the given amount
        /// </summary>
        /// <param name="amount">The amount to damage for</param>
        /// <param name="context">The context of the damage being applies</param>
        /// <param name="makeInvulnerable">Whether the object should be made invulnerable after the damage</param>
        /// <param name="piercing">Whether the damage should ignore defense and shields</param>
        public void Damage(float amount, DamageContext context, bool makeInvulnerable = false, bool piercing = false);

        /// <summary>
        /// Heals the Damageable object by the given amount
        /// </summary>
        /// <param name="amount">The amount to damage for</param>
        public void Heal(float amount);

        /// <summary>
        /// Damages the Damageable object by the given amount
        /// </summary>
        /// <param name="amount">The amount to damage for</param>
        /// <param name="context">The context of the damage being applied</param>
        /// <param name="pulses">Over how many 'pulses' should the damage be applied</param>
        /// <param name="time">Over how long should the pulses be spread</param>
        public void DamageContinually(float amount, DamageContext context, int pulses, float time);
    }
}

// NOTE: In hindsight, this shouldn't have been an interface, but rather a component. Building this as an
// interface has made this too inflexible for our needs, and now I need to abuse the interface's functionalities
// to accomplish certain things, like collision damage.