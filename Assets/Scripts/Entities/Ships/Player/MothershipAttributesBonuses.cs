using UnityEngine;
using ManyTools.Variables;

namespace SketchFleets.Data
{
    public class MothershipAttributesBonuses : ScriptableObject
    {
        public IntReference upgradeLifeIncrease = new IntReference(0);
        public IntReference upgradeDamageIncrease = new IntReference(0);
        public IntReference upgradeShieldIncrease = new IntReference(0);
        public IntReference upgradeSpeedIncrease = new IntReference(0);

        public IntReference spawnSlotBonus = new IntReference(0);
        public IntReference spawnCooldownMultiplierBonus = new IntReference(0);
        public IntReference abilityCooldownMultiplierBonus = new IntReference(0);

        public FloatReference healthRegen = new FloatReference(0);
        public FloatReference shieldRegen = new FloatReference(0);
        public FloatReference maxHealthBonus = new FloatReference(0);
        public FloatReference maxShieldBonus = new FloatReference(0);
        public FloatReference damageMultiplierBonus = new FloatReference(0);
        public FloatReference speedMultiplierBonus = new FloatReference(0);
        public FloatReference defenseBonus = new FloatReference(0);
    }
}