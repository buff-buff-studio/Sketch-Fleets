using System;
using System.Collections.Generic;
using ManyTools.Variables;
using SketchFleets.Plugins;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace SketchFleets.Data
{
    /// <summary>
    /// Procedural constellation layout: column count, spacing, jitter (all edited on the MapAttributes asset).
    /// </summary>
    [Serializable]
    public sealed class ConstellationMapGenerationConfig
    {
        [Min(3)]
        [Tooltip("Minimum 3: Easy | … | Store | Boss")]
        public int columns = 12;

        [Min(1)]
        public int maxNodesPerColumn = 5;

        public float spaceBetweenY = 150f;
        public float spaceBetweenX = 180f;
        public float randomOffsetX = 50f;
        public float randomOffsetY = 50f;
        public float columnWidthMultiplierMax = 1.5f;
        public float columnWidthMultiplierMin = 1.25f;
        public int endColumnNodes = 1;
        public int startColumnNodes = 1;
        public float margin = 200f;
        public float itemHalfSize = 25f;
    }

    /// <summary>
    /// Picks <see cref="PlanetAttributes"/> using <see cref="PlanetAttributes.SpawnOnMapRangePercent"/>.
    /// </summary>
    public static class ConstellationMapPlanetPicker
    {
        /// <summary>
        /// Horizontal progress for column <paramref name="columnIndex"/> in <paramref name="columns"/> (0–100).
        /// </summary>
        public static float ColumnToMapPercent(int columnIndex, int columns)
        {
            if (columns <= 1)
                return 0f;
            return columnIndex / (float)(columns - 1) * 100f;
        }

        public static bool IsPercentInSpawnRange(float mapPercent, Vector2 minMaxPercent)
        {
            float a = Mathf.Min(minMaxPercent.x, minMaxPercent.y);
            float b = Mathf.Max(minMaxPercent.x, minMaxPercent.y);
            return mapPercent >= a && mapPercent <= b;
        }

        public static PlanetAttributes FindFirst(PlanetAttributes[] planets, PlanetDifficulty type)
        {
            if (planets == null)
                return null;
            for (int i = 0; i < planets.Length; i++)
            {
                if (planets[i] != null && planets[i].PlanetDifficulty == type)
                    return planets[i];
            }

            return null;
        }

        public static int IndexOf(PlanetAttributes[] planets, PlanetAttributes planet)
        {
            if (planets == null || planet == null)
                return -1;
            for (int i = 0; i < planets.Length; i++)
            {
                if (planets[i] == planet)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Planets whose spawn range contains <paramref name="mapPercent"/>, excluding <paramref name="exclude"/> types.
        /// </summary>
        public static List<PlanetAttributes> MatchingSpawnRange(PlanetAttributes[] planets, float mapPercent,
            HashSet<PlanetDifficulty> exclude)
        {
            var list = new List<PlanetAttributes>();
            if (planets == null)
                return list;

            for (int i = 0; i < planets.Length; i++)
            {
                PlanetAttributes p = planets[i];
                if (p == null)
                    continue;
                if (exclude != null && exclude.Contains(p.PlanetDifficulty))
                    continue;
                if (IsPercentInSpawnRange(mapPercent, p.SpawnOnMapRangePercent))
                    list.Add(p);
            }

            return list;
        }

        public static PlanetAttributes PickRandom(List<PlanetAttributes> candidates, PlanetAttributes fallback)
        {
            if (candidates != null && candidates.Count > 0)
                return candidates[UnityEngine.Random.Range(0, candidates.Count)];
            return fallback;
        }

        /// <summary>
        /// Any combat-tier planet (Medium/Hard/Extreme) for this column, using spawn range; if none, first Medium+ in array.
        /// </summary>
        public static PlanetAttributes PickCombatForColumn(PlanetAttributes[] planets, float mapPercent,
            PlanetAttributes fallbackMedium, int columnIndex, int columns, MapAttributes mapAttributes, int columnPlanetCount, Dictionary<PlanetAttributes, int> usedCountsInColumn)
        {
            var exclude = new HashSet<PlanetDifficulty>
            {
                PlanetDifficulty.Store,
                PlanetDifficulty.Boss,
                PlanetDifficulty.None
            };

            // First, try to get planets in spawn range
            var inRangePlanets = MatchingSpawnRange(planets, mapPercent, exclude);

            var candidates = new List<PlanetAttributes>();

            if (inRangePlanets.Count > 0)
            {
                // Only consider planets whose spawn range contains this column's mapPercent.
                // Specials are allowed here but must also pass their specialSpawnChance roll.
                foreach (PlanetAttributes p in inRangePlanets)
                {
                    if (p == null)
                        continue;

                    if (p.PlanetDifficulty == PlanetDifficulty.Special)
                    {
                        // Apply special chance only when the planet is in its spawn range
                        float chance = p.SpecialSpawnChance; // 0-100
                        if (Random.value * 100f <= chance)
                            candidates.Add(p);
                    }
                    else
                    {
                        candidates.Add(p);
                    }
                }
            }
            else
            {
                // No planets explicitly in range -> fallback to combat planets but
                // exclude Special here so Special only ever appears when in-range.
                if (planets != null)
                {
                    foreach (PlanetAttributes p in planets)
                    {
                        if (p == null)
                            continue;
                        if (exclude.Contains(p.PlanetDifficulty))
                            continue;
                        if (p.PlanetDifficulty == PlanetDifficulty.Special)
                            continue; // specials respect their range and won't be used as fallback

                        candidates.Add(p);
                    }
                }
            }

            // Remove planets that have reached the max duplicates limit
            int maxDuplicatas = Mathf.FloorToInt(columnPlanetCount * 2f / 3f);
            candidates.RemoveAll(p => usedCountsInColumn.ContainsKey(p) && usedCountsInColumn[p] >= maxDuplicatas);

            if (candidates.Count == 0)
            {
                return fallbackMedium;
            }

            // Apply weighted selection based on map progression
            var weightedPlanets = new List<(PlanetAttributes planet, float weight)>();
            float totalWeight = 0f;

            foreach (PlanetAttributes p in candidates)
            {
                float weight = 1f;

                // Weight based on map progression and difficulty, using mapAttributes weights
                switch (p.PlanetDifficulty)
                {
                    case PlanetDifficulty.Easy:
                        weight = Mathf.Lerp(mapAttributes.EasyWeights.x, mapAttributes.EasyWeights.y, mapPercent / 100f);
                        break;
                    case PlanetDifficulty.Medium:
                        weight = Mathf.Lerp(mapAttributes.MediumWeights.x, mapAttributes.MediumWeights.y, mapPercent / 100f);
                        break;
                    case PlanetDifficulty.Hard:
                        weight = Mathf.Lerp(mapAttributes.HardWeights.x, mapAttributes.HardWeights.y, mapPercent / 100f);
                        break;
                    case PlanetDifficulty.Extreme:
                        weight = Mathf.Lerp(mapAttributes.ExtremeWeights.x, mapAttributes.ExtremeWeights.y, mapPercent / 100f);
                        break;
                }

                // If using fallback (not in range), apply penalty
                if (inRangePlanets.Count == 0 || !inRangePlanets.Contains(p))
                {
                    weight *= 0.3f; // Strong penalty for out-of-range planets
                }

                weight = Mathf.Max(0.1f, weight);
                weightedPlanets.Add((p, weight));
                totalWeight += weight;
            }

            if (totalWeight <= 0.001f)
            {
                return candidates[UnityEngine.Random.Range(0, candidates.Count)];
            }

            // Weighted random selection
            float randomPoint = UnityEngine.Random.value * totalWeight;
            foreach (var entry in weightedPlanets)
            {
                if (randomPoint < entry.weight)
                {
                    return entry.planet;
                }
                randomPoint -= entry.weight;
            }

            return candidates[UnityEngine.Random.Range(0, candidates.Count)];
        }
    }

    /// <summary>
    /// Map-level data: constellation layout, planet list, and spawn references.
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.difficultyAttributesOrder, fileName = CreateMenus.difficultyAttributesFileName,
        menuName = CreateMenus.difficultyAttributesMenuName)]
    public sealed class MapAttributes : ScriptableObject
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
        private PlanetAttributes[] planetAttributes;

        [SerializeField]
        private FloatReference mapHeight;

        [Tooltip("Map Start Spawn.")]
        [SerializeField]
        private FloatReference mapStartSpawn;

        [Header("Constellation map generation")]
        [SerializeField]
        private ConstellationMapGenerationConfig constellationGeneration = new ConstellationMapGenerationConfig();

        [Header("Difficulty Weights")]
        [Tooltip("Weights for Easy planets: X=start weight, Y=end weight")]
        [SerializeField]
        private Vector2 easyWeights = new Vector2(8f, 0.5f);
        [Tooltip("Weights for Medium planets: X=start weight, Y=end weight")]
        [SerializeField]
        private Vector2 mediumWeights = new Vector2(3f, 4f);
        [Tooltip("Weights for Hard planets: X=start weight, Y=end weight")]
        [SerializeField]
        private Vector2 hardWeights = new Vector2(1f, 6f);
        [Tooltip("Weights for Extreme planets: X=start weight, Y=end weight")]
        [SerializeField]
        private Vector2 extremeWeights = new Vector2(0.1f, 10f);

        #endregion

        #region Properties

        public IntReference Map => currentMap;
        public IntReference Difficulty => currentDifficulty;
        public PlanetAttributes[] Planets => planetAttributes;
        public PlanetAttributes Planet => planetAttributes[currentDifficulty.Value];
        public PlanetDifficulty MapDifficulty => Planet.PlanetDifficulty;
        public Vector2Reference MinMaxWaves => Planet.MinMaxWaves;
        public Vector2Reference SpawnAndWaveInterval => Planet.SpawnAndWaveInterval;
        public FloatReference MapHeight => mapHeight;
        public FloatReference MapStartSpawn => mapStartSpawn;
        public Color MapColor => Planet.PlanetColor;

        public FormationPool EnemyPool => Planet.EnemyPool;
        public IntReference MaxEnemies => Planet.MaxEnemies;
        public AttributePool ObstaclePool => Planet.ObstaclePool;

        /// <summary>Layout columns, spacing, and jitter for this map asset.</summary>
        public ConstellationMapGenerationConfig ConstellationGeneration
        {
            get
            {
                if (constellationGeneration == null)
                    constellationGeneration = new ConstellationMapGenerationConfig();
                return constellationGeneration;
            }
        }

        public Vector2 EasyWeights => easyWeights;
        public Vector2 MediumWeights => mediumWeights;
        public Vector2 HardWeights => hardWeights;
        public Vector2 ExtremeWeights => extremeWeights;

        #endregion

        private void OnEnable()
        {
            if (constellationGeneration == null)
                constellationGeneration = new ConstellationMapGenerationConfig();
        }
    }
}
