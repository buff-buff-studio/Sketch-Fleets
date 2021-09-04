using ManyTools.Variables;
using UnityEngine;
using UnityEngine.Serialization;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that contains data about a ship's attributes
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.difficultyAttributesOrder, fileName = CreateMenus.difficultyAttributesFileName,
        menuName = CreateMenus.difficultyAttributesMenuName)]
    public class MapAttributes : Attributes
    {
        #region Protected Fields

        [Header("Level")]
        [SerializeField]
        private IntReference currentMap;
        [SerializeField]
        private IntReference currentDifficulty;

        [Header("Attributes")]
        [Tooltip("Multiplies the amount of enemies in the map")]
        [SerializeField]
        protected IntReference[] mapDifficulty;
        [FormerlySerializedAs("mapWaves")]
        [Tooltip("The minum and maximum number of waves in a map")]
        [SerializeField]
        protected Vector2Reference[] minMaxWaves;
        [SerializeField]
        protected FloatReference mapHeight;
        [Tooltip("Map Start Spawn.")]
        [SerializeField]
        protected FloatReference mapStartSpawn;
        [Tooltip("Map color on the chart.")]
        [SerializeField]
        [ColorHEXCode]
        protected Color[] mapColor;

        [Header("Enemies and Obstacles")]
        [SerializeField, Tooltip("The pool of enemies used for this map")]
        protected AttributePool[] enemyPool;
        [SerializeField, Tooltip("The maximum amount of enemies that can spawn at any given time")]
        protected IntReference[] maxEnemies;
        [SerializeField, Tooltip("The pool of obstacles used for this map")]
        protected AttributePool[] obstaclePool;

        #endregion

        #region Properties

        public IntReference Map => currentMap;
        public IntReference Difficulty => currentDifficulty;

        public IntReference[] MapDifficulty => mapDifficulty;
        public Vector2Reference[] MinMaxWaves => minMaxWaves;
        public FloatReference MapHeight => mapHeight;
        public FloatReference MapStartSpawn => mapStartSpawn;
        public Color[] MapColor => mapColor;

        public AttributePool[] EnemyPool => enemyPool;
        public IntReference[] MaxEnemies => maxEnemies;
        public AttributePool[] ObstaclePool => obstaclePool;


        #endregion
    }
}
