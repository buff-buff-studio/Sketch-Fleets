using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that holds data about a spawnable ship
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.spawnableShipAttributesOrder, 
        fileName = CreateMenus.spawnableShipAttributesFileName, menuName = CreateMenus.spawnableShipAttributesMenuName)]
    public sealed class SpawnableShipAttributes : ShipAttributes
    {
        #region Private Fields

        [Header("Spawn Parameters")]
        [SerializeField]
        [Tooltip("How much graphite it takes to summon a ship")]
        private IntReference graphiteCost;
        [SerializeField]
        [Tooltip("How long the player must wait before invoking another ship")]
        private IntReference spawnCooldown;

        #endregion

        #region Properties

        public IntReference GraphiteCost
        {
            get => graphiteCost;
        }

        public IntReference SpawnCooldown
        {
            get => spawnCooldown;
        }

        #endregion
    }
}