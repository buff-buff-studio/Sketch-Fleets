using System;
using System.Collections;
using System.Collections.Generic;
using ManyTools.Events;
using ManyTools.UnityExtended.Editor;
using ManyTools.UnityExtended.Poolable;
using ManyTools.Variables;
using SketchFleets.Data;
using SketchFleets.Enemies;
using SketchFleets.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SketchFleets.Systems
{
    /// <summary>
    /// A class that spawns enemies
    /// </summary>
    public sealed class ShipSpawner : MonoBehaviour
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

        [Header("Other Parameters")]
        [SerializeField]
        [Tooltip("The total of enemies killed, displayed in the UI")]
        private IntReference totalEnemiesKilled;
        
        [SerializeField]
        private Collider2D enemyTeleportArea;

        [Header("Events")]
        [SerializeField]
        [RequiredField]
        private GameEvent onWaveEnd;

        private int mapWaveCount;
        private int currentWave;
        private int pendingSpawns;

        #endregion

        #region Properties

        public List<Transform> ActiveEnemyShips { get; } = new List<Transform>(10);
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
        public void CountShipDeath(Ship<ShipAttributes> ship)
        {
            ActiveEnemyShips.Remove(ship.transform);
            totalEnemiesKilled.Value++;

            if (!IsWaveOver()) return;
            HandleWaveProgress();
        }

        #endregion

        #region Private Methods

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
                onWaveEnd.Invoke();
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
            PurgeNullShips();
            return ActiveEnemyShips.Count <= 0 && pendingSpawns <= 0;
        }

        /// <summary>
        /// Removes any null entries from the active ships list
        /// </summary>
        private void PurgeNullShips()
        {
            if (ActiveEnemyShips.Count <= 0) return;

            for (int index = ActiveEnemyShips.Count - 1; index >= 0; index--)
            {
                if (ActiveEnemyShips[index] != null) continue;
                ActiveEnemyShips.RemoveAt(index);
            }
        }

        /// <summary>
        /// Spawns an entire wave of ships
        /// </summary>
        private IEnumerator SpawnWave()
        {
            WaitForSeconds spawnWait = new WaitForSeconds(spawnInterval);
            yield return new WaitForSeconds(waveInterval);
            pendingSpawns = mapAttributes.MaxEnemies[mapAttributes.Difficulty];

            for (int index = 0; index < mapAttributes.MaxEnemies[mapAttributes.Difficulty]; index++)
            {
                SpawnFormation();
                pendingSpawns--;
                yield return spawnWait;
            }
        }

        /// <summary>
        /// Spawns an entire formation of ships
        /// </summary>
        private void SpawnFormation()
        {
            ShipFormation drawnFormation = DrawFormationFromPool();

            Vector3 spawnPoint = GetRandomSpawnPoint(drawnFormation.Formation.GetBounds());

            GameObject formationObject = Instantiate(drawnFormation.GameObject, spawnPoint,
                Quaternion.Euler(0f, 0f, 90f));

            for (int index = 0, max = formationObject.transform.childCount; index < max; index++)
            {
                Transform formationPoint = formationObject.transform.GetChild(index).transform;
                SpawnShip(drawnFormation.Ships[index], formationPoint.position, formationPoint.rotation);
            }

            Destroy(formationObject);
        }

        /// <summary>
        /// Spawns an individual ship in the formation
        /// </summary>
        private void SpawnShip(ShipAttributes shipToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            if (shipToSpawn == null) return;
            
            PoolMember ship = PoolManager.Instance.Request(shipToSpawn.Prefab);
            ship.Emerge(spawnPosition, spawnRotation);
            RegisterEnemyShip((EnemyShip)ship);
        }

        /// <summary>
        /// Registers the enemy ship
        /// </summary>
        /// <param name="ship">The ship to register</param>
        private void RegisterEnemyShip(EnemyShip ship)
        {
            ship.Spawner = this;

            if (ship.TryGetComponent(out ShipTeleporter teleporter))
            {
                teleporter.TeleportBounds = enemyTeleportArea.bounds;
            }
            
            ActiveEnemyShips.Add(ship.transform);
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
            Bounds bounds = spawnArea.bounds;
            return Random.Range(bounds.min.y + formationExtents.x, bounds.max.y - formationExtents.y);
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