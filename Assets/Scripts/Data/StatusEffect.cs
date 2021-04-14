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
        protected FloatReference maxHealthBonus;
        [SerializeField]
        protected FloatReference maxShieldBonus;
        [SerializeField]
        protected FloatReference damageMultiplierBonus;
        [SerializeField]
        protected FloatReference speedMultiplierBonus;
        [SerializeField]
        protected FloatReference defenseBonus;

        #endregion

        #region Properties

        public FloatReference MaxHealthBonus
        {
            get => maxHealthBonus;
        }

        public FloatReference MaxShieldBonus
        {
            get => maxShieldBonus;
        }

        public FloatReference DamageMultiplierBonus
        {
            get => damageMultiplierBonus;
        }

        public FloatReference SpeedMultiplierBonus
        {
            get => speedMultiplierBonus;
        }

        public FloatReference DefenseBonus
        {
            get => defenseBonus;
        }

        #endregion

    }
}