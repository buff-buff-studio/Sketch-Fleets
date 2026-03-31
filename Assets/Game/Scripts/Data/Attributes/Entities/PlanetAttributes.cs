using ManyTools.Variables;
using SketchFleets.Plugins;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SketchFleets.Data
{
    public enum PlanetDifficulty : int
    {
        Special = -2,
        Store = -1,
        None = 0,
        Easy = 1,
        Medium = 2,
        Hard = 3,
        Extreme = 4,
        Boss = 5
    }

    /// <summary>
    /// A class that contains data about a ship's attributes
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.planetAttributesOrder, fileName = CreateMenus.planetAttributesFileName,
        menuName = CreateMenus.planetAttributesMenuName)]
    [Icon("d_SphereCollider Icon")]
    public sealed class PlanetAttributes : Attributes
    {
        #region Protected Fields

        [Header("Planet Info")]
        [Tooltip("The planet's sprite icon.")]
        [SerializeField]
        [VariablePreview]
        private Sprite icon;

        [Tooltip("The planet's background prefab.")]
        [SerializeField]
        [VariablePreview]
        private GameObject background;

        [Tooltip("Map color on the chart.")]
        [SerializeField]
        [ColorHEXCode]
        private Color color;

        [Tooltip("Multiplies the amount of enemies in the map")]
        [SerializeField]
        private PlanetDifficulty difficulty;

        [Tooltip("The range of the spawn point on the map, relative to the percentage of the map")]
        [SerializeField]
        private Vector2Reference spawnOnMapRange;

        [Tooltip("The chance for a special planet to spawn instead of a regular one (0–100)")]
        [SerializeField]
        [Range(0f, 100f)]
        private float specialSpawnChance;

        [Header("Enemy and Obstacle Pools")]
        [SerializeField]
        [Tooltip("The pool of enemies used for this map")]
        private FormationPool enemyPool;

        [SerializeField]
        [Tooltip("The pool of obstacles used for this map")]
        private AttributePool obstaclePool;

        [Header("Wave and Spawn Settings")]
        [SerializeField]
        [Tooltip("The maximum amount of enemies that can spawn at any given time")]
        private IntReference maxEnemies;

        [Tooltip("The minum and maximum number of waves in a map")]
        [SerializeField]
        private Vector2Reference minMaxWaves;

        [SerializeField]
        [Tooltip("The interval in seconds between each enemy spawn and each wave spawn, respectively")]
        private Vector2Reference spawnAndWaveInterval;

        #endregion

        #region Properties

        public PlanetDifficulty PlanetDifficulty => difficulty;

        /// <summary>
        /// Min/max percentage (0–100) along map width where this planet type may appear. Used by constellation generation.
        /// </summary>
        public Vector2 SpawnOnMapRangePercent => spawnOnMapRange != null ? spawnOnMapRange.Value : new Vector2(0f, 100f);

        public Vector2Reference MinMaxWaves => minMaxWaves;
        public Vector2Reference SpawnAndWaveInterval => spawnAndWaveInterval;
        /// <summary>
        /// Chance (0-100) for this special planet to spawn when considered by the generator.
        /// </summary>
        public float SpecialSpawnChance => specialSpawnChance != null ? specialSpawnChance : 0f;
        public Color PlanetColor => color;
        public Sprite PlanetSprite => icon;
        public GameObject PlanetBackground => background;
        public FormationPool EnemyPool => enemyPool;
        public IntReference MaxEnemies => maxEnemies;
        public AttributePool ObstaclePool => obstaclePool;

        #endregion
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(PlanetAttributes), true)]
    [CanEditMultipleObjects]
    public class PlanetAttributesEditor : Editor
    {
        public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width,
            int height)
        {
            PlanetAttributes planetAttributes = (PlanetAttributes)target;
            if (planetAttributes.PlanetSprite == null)
                return base.RenderStaticPreview(assetPath, subAssets, width, height);

            Texture2D texture = new Texture2D((int)planetAttributes.PlanetSprite.textureRect.width,
                (int)planetAttributes.PlanetSprite.textureRect.height);
            Color[] pixels = planetAttributes.PlanetSprite.texture.GetPixels(
                (int)planetAttributes.PlanetSprite.textureRect.x,
                (int)planetAttributes.PlanetSprite.textureRect.y,
                (int)planetAttributes.PlanetSprite.textureRect.width,
                (int)planetAttributes.PlanetSprite.textureRect.height);

            texture.SetPixels(pixels);
            //alpha
            for (int i = 0; i < pixels.Length; i++)
            {
                Color pixelColor = pixels[i];
                if (pixelColor.a < 0.1f)
                    pixels[i] = new Color(0, 0, 0, 0);
            }
            texture.Apply();
            return texture;
        }
    }
    #endif
}