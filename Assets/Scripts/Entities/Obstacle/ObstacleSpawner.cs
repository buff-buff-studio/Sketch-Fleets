using System.Collections;
using ManyTools.Variables;
using SketchFleets.Data;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class ObstacleSpawner : MonoBehaviour
{
    #region Private Fields

    [Header("Warning")]
    [FormerlySerializedAs("CautionImage")] [SerializeField]
    private GameObject warningImage;
    private FloatReference warningDuration = new FloatReference(1.3f);

    [Header("Obstacles")]
    [SerializeField, Tooltip("The pool of spawnable obstacles")]
    private AttributePool obstaclePool;
    [SerializeField, Tooltip("The area in which obstacles should spawn")]
    private Collider2D spawnArea;
    [SerializeField, Tooltip("The delay between each obstacle spawn")]
    private FloatReference spawnDelay = new FloatReference(20f);

    private AudioSource warningAudioSource;
    private WaitForSeconds cachedWarningTime;

    private IEnumerator spawnObstacleRoutine;
    private IEnumerator showWarningRoutine;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        CacheComponents();

        spawnObstacleRoutine = SpawnObstacles();
        showWarningRoutine = ShowWarning();
        StartCoroutine(spawnObstacleRoutine);
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
            
            StartCoroutine(showWarningRoutine);
            yield return warningPeriod;
            
            SpawnObstacle(DrawObstacleFromPool());
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
        TryGetComponent(out warningAudioSource);
        cachedWarningTime = new WaitForSeconds(warningDuration);
    }

    /// <summary>
    /// Whether to warn the player of incoming obstacles
    /// </summary>
    /// <param name="warn">Whether to set the warning active or inactive</param>
    private void Warn(bool warn)
    {
        if (warn && warningAudioSource.clip != null)
        {
            warningAudioSource.Play();
        }

        warningImage.SetActive(warn);
    }

    /// <summary>
    /// Draws an obstacle attribute from the pool
    /// </summary>
    /// <returns>The drawn obstacle attribute</returns>
    private ObstacleAttributes DrawObstacleFromPool()
    {
        ObstacleAttributes draw = obstaclePool.Draw() as ObstacleAttributes;
        return draw;
    }

    /// <summary>
    /// Spawns an obstacle in a random position at the spawn area
    /// </summary>
    /// <param name="obstacle">The obstacle to spawn</param>
    private void SpawnObstacle(ObstacleAttributes obstacle)
    {
        if (obstacle == null) return;

        Vector3 spawnPoint = new Vector3(spawnArea.transform.position.x, GetRandomYInSpawnArea());
        Instantiate(obstacle.Prefab, spawnPoint, Quaternion.identity);
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