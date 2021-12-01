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
        [Tooltip("The special effect to spawn when this ship is summoned")]
        private GameObject spawnEffect;
        [SerializeField]
        [Tooltip("How many active ships of this kind can there be at a time")]
        private IntReference maximumShips = new IntReference(10);
        [SerializeField]
        [Tooltip("How long the player must wait before invoking another ship")]
        private IntReference spawnCooldown = new IntReference(10);
        [SerializeField]
        [Tooltip("The radius of the ship's orbit around the player, if it orbits him")]
        private FloatReference orbitRadius = new FloatReference(3f);
        
        #endregion

        #region Properties

        public IntReference SpawnCooldown => spawnCooldown;
        public IntReference MaximumShips => maximumShips;
        public FloatReference OrbitRadius => orbitRadius;
        public GameObject SpawnEffect => spawnEffect;

        #endregion
    }
}