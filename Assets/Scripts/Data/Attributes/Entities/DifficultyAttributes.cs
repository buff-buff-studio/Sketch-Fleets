using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that contains data about a ship's attributes
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.difficultyAttributesOrder, fileName = CreateMenus.difficultyAttributesFileName,
        menuName = CreateMenus.difficultyAttributesMenuName)]
    public class DifficultyAttributes : Attributes
    {
        #region Protected Fields

        [Header("Level")]
        [SerializeField]
        private IntReference currentMap;
        [SerializeField]
        private IntReference currentDifficulty;

        [Header("Attributes")]
        [Tooltip("Enemy multiplier for the generation.")]
        [SerializeField]
        protected IntReference[] mapDifficulty;
        [Tooltip("Average map size.")]
        [SerializeField]
        protected Vector2Reference[] mapSize;
        [Tooltip("Map Height.")]
        [SerializeField]
        protected FloatReference mapHeight;
        [Tooltip("Map Start Spawn.")]
        [SerializeField]
        protected FloatReference mapStartSpawn;
        [Tooltip("Map color on the chart.")]
        [SerializeField]
        [ColorHEXCode]
        protected Color[] mapColor;

        [Header("Prefabs")]
        [SerializeField]
        protected GameObject purpleShip;
        [SerializeField]
        protected GameObject orangeShip;
        [SerializeField]
        protected GameObject limeShip;

        [SerializeField]
        protected GameObject eraserObstacle;
        [SerializeField]
        protected GameObject graphiteObstacle;
        [SerializeField]
        protected GameObject meteorObstacle;
        [SerializeField]
        protected GameObject splashObstacle;

        [Tooltip("Boss Prefabs.")]
        [SerializeField]
        protected GameObject[] bossShip;

        #endregion

        #region Properties

        public IntReference Map
        {
            get => currentMap;
            set => currentMap.Value = value;
        }
        public IntReference Difficulty
        {
            get => currentDifficulty;
            set => currentDifficulty.Value = value;
        }

        public IntReference[] MapDifficulty
        {
            get => mapDifficulty;
        }
        public Vector2Reference[] MapSize
        {
            get => mapSize;
        }
        public FloatReference MapHeight
        {
            get => mapHeight;
        }
        public FloatReference MapStartSpawn
        {
            get => mapStartSpawn;
        }
        public Color[] MapColor
        {
            get => mapColor;
        }

        public GameObject Purple
        {
            get => purpleShip;
        }
        public GameObject Orange
        {
            get => orangeShip;
        }
        public GameObject Lime
        {
            get => limeShip;
        }

        public GameObject Eraser
        {
            get => eraserObstacle;
        }
        public GameObject Graphite
        {
            get => graphiteObstacle;
        }
        public GameObject Meteor
        {
            get => meteorObstacle;
        }
        public GameObject Splash
        {
            get => splashObstacle;
        }

        public GameObject[] Boss
        {
            get => bossShip;
        }

        #endregion
    }
}
