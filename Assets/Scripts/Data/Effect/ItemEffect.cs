using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that contains attributes for an item usable by the mothership
    /// </summary>
    [CreateAssetMenu(fileName = CreateMenus.itemEffectFileName, menuName = CreateMenus.itemEffectMenuName,
        order = CreateMenus.itemEffectOrder)]
    public sealed class ItemEffect : StatusEffect
    {
        #region Private Fields
        
        [Header("Item-only Effects")]
        [SerializeField]
        private IntReference spawnSlotBonus;
        [SerializeField]
        private IntReference spawnCooldownMultiplierBonus;
        [SerializeField]
        private IntReference abilityCooldownMultiplierBonus;

        #endregion

        #region Properties

        public IntReference SpawnSlotBonus
        {
            get => spawnSlotBonus;
            set => spawnSlotBonus = value;
        }

        public IntReference SpawnCooldownMultiplierBonus
        {
            get => spawnCooldownMultiplierBonus;
            set => spawnCooldownMultiplierBonus = value;
        }

        public IntReference AbilityCooldownMultiplierBonus
        {
            get => abilityCooldownMultiplierBonus;
            set => abilityCooldownMultiplierBonus = value;
        }

        #endregion

    }
}