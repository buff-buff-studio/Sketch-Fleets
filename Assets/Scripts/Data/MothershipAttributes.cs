using System.Collections.Generic;
using ManyTools.UnityExtended;
using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that holds attributes relative to the mothership
    /// </summary>
    [CreateAssetMenu(fileName = CreateMenus.motherShipAttributesFileName, 
        menuName = CreateMenus.motherShipAttributesMenuName, order = CreateMenus.motherShipAttributesOrder)]
    public sealed class MothershipAttributes : ShipAttributes
    {
        #region Private Fields

        [Header("Mothership Attributes")]
        [Tooltip("How many of each ship can be spawned.")]
        [SerializeField]
        private UnityDictionary<string, GameObject> shipByType;
        [SerializeField]
        [Tooltip("How many extra spawnable ships the mothership can have at a time.")]
        private IntReference extraSpawnSlots = new IntReference(0);
        [SerializeField]
        [Tooltip("A multiplier for the cooldown generated when spawning a ship.")]
        private FloatReference spawnCooldownMultiplier = new FloatReference(1);
        [SerializeField]
        [Tooltip("A multiplier for the cooldown generated when using an ability.")]
        private FloatReference abilityCooldownMultiplier = new FloatReference(1);

        [Header("Spawned Ship Bonuses")]
        [SerializeField]
        private List<StatusEffect> spawnStatusBonus = new List<StatusEffect>();

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

        public List<StatusEffect> SpawnStatusBonus
        {
            get => spawnStatusBonus;
        }

        #endregion

    }
}