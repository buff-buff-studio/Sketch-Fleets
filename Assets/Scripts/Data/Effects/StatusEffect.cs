using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that holds data relative to a status effect
    /// </summary>
    [CreateAssetMenu(fileName = CreateMenus.statusEffectFileName, menuName = CreateMenus.statusEffectMenuName,
        order = CreateMenus.satusEffectOrder)]
    public class StatusEffect : Attributes
    {
        #region Private Fields

        [Header("Status Effect Bonus")]
        [SerializeField]
        [Tooltip("How much health will be regenerated over the duration of the effect")]
        protected FloatReference healthRegen = new FloatReference(0f);
        [SerializeField]
        [Tooltip("How much shield will be regenerated over the duration of the effect")]
        protected FloatReference shieldRegen = new FloatReference(0f);
        [SerializeField]
        protected FloatReference maxHealthBonus;
        [SerializeField]
        protected FloatReference maxShieldBonus;
        [SerializeField]
        protected FloatReference damageMultiplierBonus;
        [SerializeField]
        protected FloatReference speedMultiplierBonus;
        [SerializeField]
        protected FloatReference defenseBonus;
        [SerializeField]
        [Tooltip("How long the status effect will endure. Please note status effects are only updated every" +
                 " 1 second, to increase performance. A duration of -1 means the effect is infinite.")]
        protected FloatReference duration = new FloatReference(-1f);

        #endregion

        #region Properties

        public FloatReference MaxHealthBonus => maxHealthBonus;

        public FloatReference MaxShieldBonus => maxShieldBonus;

        public FloatReference DamageMultiplierBonus => damageMultiplierBonus;

        public FloatReference SpeedMultiplierBonus => speedMultiplierBonus;

        public FloatReference DefenseBonus => defenseBonus;

        public FloatReference Duration => duration;

        public FloatReference HealthRegen => healthRegen;

        public FloatReference ShieldRegen => shieldRegen;

        #endregion
    }
}