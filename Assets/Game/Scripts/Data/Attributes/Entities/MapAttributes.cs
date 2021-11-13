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
    public sealed class MapAttributes : Attributes
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
        private IntReference[] mapDifficulty;

        [FormerlySerializedAs("mapWaves")]
        [Tooltip("The minum and maximum number of waves in a map")]
        [SerializeField]
        private Vector2Reference[] minMaxWaves;

        [SerializeField]
        private FloatReference mapHeight;

        [Tooltip("Map Start Spawn.")]
        [SerializeField]
        private FloatReference mapStartSpawn;

        [Tooltip("Map color on the chart.")]
        [SerializeField]
        [ColorHEXCode]
        private Color[] mapColor;

        [Header("Enemies and Obstacles")]
        [SerializeField]
        [Tooltip("The pool of enemies used for this map")]
        private FormationPool[] enemyPool;

        [SerializeField]
        [Tooltip("The maximum amount of enemies that can spawn at any given time")]
        private IntReference[] maxEnemies;

        [SerializeField]
        [Tooltip("The pool of obstacles used for this map")]
        private AttributePool[] obstaclePool;

        #endregion

        #region Properties

        public IntReference Map => currentMap;
        public IntReference Difficulty => currentDifficulty;

        public IntReference[] MapDifficulty => mapDifficulty;
        public Vector2Reference[] MinMaxWaves => minMaxWaves;
        public FloatReference MapHeight => mapHeight;
        public FloatReference MapStartSpawn => mapStartSpawn;
        public Color[] MapColor => mapColor;

        public FormationPool[] EnemyPool => enemyPool;
        public IntReference[] MaxEnemies => maxEnemies;
        public AttributePool[] ObstaclePool => obstaclePool;

        #endregion
    }
}