using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that holds attributes relative to the mothership
    /// </summary>
    [CreateAssetMenu(fileName = CreateMenus.motherShipAttributesFileName, 
        menuName = CreateMenus.motherShipAttributesMenuName, order = CreateMenus.motherShipAttributesOrder)]
    public sealed class MotherShipAttributes : ShipAttributes
    {
        #region Private Fields

        [Header("Mothership Attributes")]
        [SerializeField]
        [Tooltip("How many extra spawnable ships the mothership can have at a time.")]
        private IntReference extraSpawnSlots;
        [SerializeField]
        [Tooltip("A multiplier for the cooldown generated when spawning a ship.")]
        private FloatReference spawnCooldownMultiplier;
        [SerializeField]
        [Tooltip("A multiplier for the cooldown generated when using an ability.")]
        private FloatReference abilityCooldownMultiplier;

        [Header("Spawned Ship Bonuses")]
        [SerializeField]
        private StatusEffect spawnStatusBonus;

        #endregion

        #region Properties

        public FloatReference AbilityCooldownMultiplier
        {
            get => abilityCooldownMultiplier;
        }

        public FloatReference SpawnCooldownMultiplier
        {
            get => spawnCooldownMultiplier;
        }

        public IntReference ExtraSpawnSlots
        {
            get => extraSpawnSlots;
        }

        public StatusEffect SpawnStatusBonus
        {
            get => spawnStatusBonus;
        }

        #endregion

    }
}