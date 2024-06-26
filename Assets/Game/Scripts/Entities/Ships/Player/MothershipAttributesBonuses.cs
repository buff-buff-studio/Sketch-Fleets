using UnityEngine;
using ManyTools.Variables;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that contains bonuses
    /// </summary>
    public class MothershipAttributesBonuses : ScriptableObject
    {
        public FloatReference HealthIncrease = new FloatReference(0);
        public FloatReference DamageIncrease = new FloatReference(0);
        public FloatReference ShieldIncrease = new FloatReference(0);
        public FloatReference SpeedIncrease = new FloatReference(0);
        public IntReference ExtraSpawnSlots = new IntReference(1);

        public FloatReference SpawnCooldownMultiplier = new FloatReference(1f);
        public FloatReference AbilityCooldownMultiplier = new FloatReference(1f);
        public FloatReference DamageMultiplier = new FloatReference(1f);
        public FloatReference SpeedMultiplier = new FloatReference(1f);

        public FloatReference HealthRegen = new FloatReference(0);
        public FloatReference ShieldRegen = new FloatReference(0);
        public FloatReference MaxHealth = new FloatReference(0);
        public FloatReference MaxShield = new FloatReference(0);
        public FloatReference Defense = new FloatReference(0);
    }
}