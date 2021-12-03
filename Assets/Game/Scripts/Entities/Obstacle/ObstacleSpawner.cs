using System.Collections;
using ManyTools.Variables;
using SketchFleets.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace SketchFleets.Systems
{
    public sealed class ObstacleSpawner : MonoBehaviour
    {
        #region Private Fields

        [Header("Warning")]
        [FormerlySerializedAs("CautionImage")]
        [SerializeField]
        private RectTransform warningImage;

        private FloatReference warningDuration = new FloatReference(1.3f);
        private Vector3 spawnPoint;

        [Header("Obstacles")]
        [SerializeField, Tooltip("The pool of spawnable obstacles")]
        private MapAttributes mapAttributes;

        [SerializeField, Tooltip("The area in which obstacles should spawn")]
        private Collider2D spawnArea;

        [SerializeField, Tooltip("The delay between each obstacle spawn")]
        private FloatReference spawnDelay = new FloatReference(20f);

        private WaitForSeconds cachedWarningTime;
        private Camera mainCameraCache;

        #endregion

        #region Properties

        private AttributePool ObstaclePool => mapAttributes.ObstaclePool[mapAttributes.Difficulty];

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            CacheComponents();
            StartCoroutine(SpawnObstacles());
        }

        #endregion

        #region Obstacle Spawner

        /// <summary>
        /// Every 5 minutes there is a 25% chance that an obstacle will be generated.
        /// 2 obstacles do not coexist at the same time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpawnObstacles()
        {
            WaitForSeconds spawnWait = new WaitForSeconds(spawnDelay - warningDuration);
            WaitForSeconds warningPeriod = new WaitForSeconds(warningDuration);

            while (true)
            {
                yield return spawnWait;

                ObstacleAttributes drawnObstacle = DrawObstacleFromPool();
                bool isDrawnObstacleValid = drawnObstacle != null;

                if (isDrawnObstacleValid && drawnObstacle.WarnOnSpawn)
                {
                    spawnPoint = new Vector3(spawnArea.transform.position.x, GetRandomYInSpawnArea());

                    StartCoroutine(ShowWarning());
                }

                yield return warningPeriod;

                if (isDrawnObstacleValid)
                {
                    spawnPoint = new Vector3(spawnArea.transform.position.x, GetRandomYInSpawnArea());
                    SpawnObstacle(drawnObstacle);
                }
            }
        }

        /// <summary>
        /// Shows the obstacle warning 
        /// </summary>
        private IEnumerator ShowWarning()
        {
            Warn(true);

            yield return cachedWarningTime;

            Warn(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Caches all components necessary for functionality
        /// </summary>
        private void CacheComponents()
        {
            mainCameraCache = Camera.main;
            cachedWarningTime = new WaitForSeconds(warningDuration);
        }

        /// <summary>
        /// Whether to warn the player of incoming obstacles
        /// </summary>
        /// <param name="warn">Whether to set the warning active or inactive</param>
        private void Warn(bool warn)
        {
            warningImage.anchoredPosition = new Vector2(warningImage.anchoredPosition.x, GetObstaclePos.y);
            warningImage.gameObject.SetActive(warn);
        }

        private Vector2 GetObstaclePos =>
            mainCameraCache.WorldToViewportPoint(mainCameraCache.ViewportToScreenPoint(new Vector2(0, spawnPoint.y)));

        /// <summary>
        /// Draws an obstacle attribute from the pool
        /// </summary>
        /// <returns>The drawn obstacle attribute</returns>
        private ObstacleAttributes DrawObstacleFromPool()
        {
            ObstacleAttributes draw = ObstaclePool.Draw() as ObstacleAttributes;
            return draw;
        }

        /// <summary>
        /// Spawns an obstacle in a random position at the spawn area
        /// </summary>
        /// <param name="obstacle">The obstacle to spawn</param>
        private void SpawnObstacle(ObstacleAttributes obstacle)
        {
            Quaternion spawnRotation = Quaternion.identity;

            if (obstacle.IsStatic)
            {
                spawnRotation = Quaternion.Euler(0f, 0, Random.Range(0f, 360f));
            }

            Instantiate(obstacle.Prefab, spawnPoint, spawnRotation);
        }

        /// <summary>
        /// Gets a random Y coordinate inside the spawn area
        /// </summary>
        /// <returns>A random Y coordinate in the spawn area</returns>
        private float GetRandomYInSpawnArea()
        {
            return spawnArea.transform.position.y + spawnArea.bounds.extents.y * Random.Range(-1f, 1f);
        }

        #endregion
    }
}