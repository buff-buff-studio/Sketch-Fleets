using System;
using System.Collections;
using ManyTools.UnityExtended.Poolable;
using ManyTools.Variables;
using SketchFleets.Data;
using SketchFleets.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SketchFleets.Systems
{
    /// <summary>
    /// A class that spawns enemies
    /// </summary>
    public sealed class EnemySpawner : MonoBehaviour
    {
        #region Private Fields

        [Header("Map Parameters")]
        [SerializeField]
        [Tooltip("The attributes of the current map")]
        private MapAttributes mapAttributes;

        [Header("Spawn Parameters")]
        [SerializeField]
        [Tooltip("The area in which a ship can spawn")]
        private Collider2D spawnArea;

        [SerializeField]
        [Tooltip("The interval between spawning each ship")]
        private FloatReference spawnInterval = new FloatReference(1.5f);

        [SerializeField]
        [Tooltip("The interval between spawning each wave")]
        private FloatReference waveInterval = new FloatReference(1.5f);

        [SerializeField]
        [Tooltip("The variation of the interval between spawning each ship")]
        private FloatReference spawnIntervalDeviation = new FloatReference(0.5f);

        [Header("Other Parameters")]
        [SerializeField]
        [Tooltip("The total of enemies killed, displayed in the UI")]
        private IntReference totalEnemiesKilled;

        private int activeShips;
        private int mapWaveCount;
        private int currentWave;
        private bool waveStarted;

        #endregion

        #region Properties

        public Action AllWavesAreOver { get; set; }

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            mapWaveCount = GenerateMapWaveCount();
            StartNextWave();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Counts the death of a ship
        /// </summary>
        public void CountShipDeath()
        {
            activeShips--;
            totalEnemiesKilled.Value++;

#if UNITY_EDITOR
            Debug.Log($"{activeShips} ships left in wave {currentWave} out of wave {mapWaveCount}");
#endif

            if (!IsWaveOver()) return;
            EndWave();

            HandleWaveProgress();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Ends the current wave
        /// </summary>
        private void EndWave()
        {
            waveStarted = false;
        }

        /// <summary>
        /// Handles wave progress by checking when waves are over and progressing accordingly
        /// </summary>
        private void HandleWaveProgress()
        {
            if (AreAllWavesOver())
            {
                AllWavesAreOver?.Invoke();
            }
            else
            {
                StartNextWave();
            }
        }

        /// <summary>
        /// Starts the next wave of the map
        /// </summary>
        private void StartNextWave()
        {
            currentWave++;
            StartCoroutine(SpawnWave());
        }

        /// <summary>
        /// Gets whether all waves were finishes
        /// </summary>
        /// <returns>Whether all waves are over</returns>
        private bool AreAllWavesOver()
        {
            return currentWave >= mapWaveCount;
        }

        /// <summary>
        /// Gets whether the active wave is over
        /// </summary>
        /// <returns>Whether the active wave is over</returns>
        private bool IsWaveOver()
        {
            return activeShips == 0 && waveStarted;
        }

        /// <summary>
        /// Spawns an entire wave of ships
        /// </summary>
        private IEnumerator SpawnWave()
        {
            yield return new WaitForSeconds(waveInterval);

            for (int index = 1; index <= mapAttributes.MaxEnemies[mapAttributes.Difficulty]; index++)
            {
                SpawnFormation();
                float intervalDeviation = Random.Range(spawnIntervalDeviation * -1f, spawnIntervalDeviation);
                yield return new WaitForSeconds(spawnInterval + intervalDeviation);
            }

            waveStarted = true;
        }

        /// <summary>
        /// Spawns an entire formation of ships
        /// </summary>
        private void SpawnFormation()
        {
            ShipFormation drawnFormation = DrawFormationFromPool();

            Vector3 spawnCenterPoint = GetRandomSpawnPoint(drawnFormation.Formation.GetBounds());

            for (int index = 0; index < drawnFormation.Ships.Length; index++)
            {
                SpawnShip(drawnFormation.Ships[index],
                    spawnCenterPoint + drawnFormation.Formation.FormationPoints[index].position);
            }
        }

        /// <summary>
        /// Spawns an individual ship in the formation
        /// </summary>
        private void SpawnShip(ShipAttributes shipToSpawn, Vector3 spawnPosition)
        {
            PoolMember ship = PoolManager.Instance.Request(shipToSpawn.Prefab);
            ship.Emerge(spawnPosition, Quaternion.identity);
            ((EnemyShip)ship).Spawner = this;
            activeShips++;
        }

        /// <summary>
        /// Gets a random spawn point within the spawn area
        /// </summary>
        /// <returns>A random spawn point within the spawn area</returns>
        private Vector3 GetRandomSpawnPoint(Vector2 formationExtents)
        {
            return new Vector3(spawnArea.transform.position.x, GetRandomYInSpawnArea(formationExtents));
        }

        /// <summary>
        /// Gets a random Y coordinate inside the spawn area
        /// </summary>
        /// <returns>A random Y coordinate in the spawn area</returns>
        private float GetRandomYInSpawnArea(Vector2 formationExtents)
        {
            return spawnArea.transform.position.y + spawnArea.bounds.extents.y * Random.Range(-1f + formationExtents.x,
                1f + formationExtents.y);
        }

        /// <summary>
        /// Gets the maximum number of waves for the map
        /// </summary>
        /// <returns>The maximum number of waves for the map</returns>
        private int GenerateMapWaveCount()
        {
            return (int)Random.Range(mapAttributes.MinMaxWaves[mapAttributes.Difficulty].Value.x, mapAttributes
                .MinMaxWaves[mapAttributes.Difficulty].Value.y);
        }

        /// <summary>
        /// Draws a ship attribute from the map's enemy pool
        /// </summary>
        /// <returns>A drawn ship attribute</returns>
        private ShipFormation DrawFormationFromPool()
        {
            ShipFormation draw = mapAttributes.EnemyPool[mapAttributes.Difficulty].Draw();

            if (draw == null)
            {
                Debug.LogError("Drawn a null Ship Attribute from Enemy Pool. " +
                               "Make sure all items are ship attributes");
            }

            return draw;
        }

        #endregion
    }
}