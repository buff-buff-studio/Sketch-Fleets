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
        private List<SpawnableShipAttributes> spawnableShips;
        [Tooltip("How long is the regenerate ability cooldown")]
        private FloatReference regenerateCooldown = new FloatReference(30f);

        #endregion

        #region Properties

        public List<SpawnableShipAttributes> SpawnableShips
        {
            get => spawnableShips;
        }

        public FloatReference RegenerateCooldown
        {
            get => regenerateCooldown;
        }

        #endregion

    }
}