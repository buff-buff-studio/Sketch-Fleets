using System.Collections;
using ManyTools.UnityExtended;
using UnityEngine;
using SketchFleets.Data;
using ManyTools.Variables;
using ManyTools.UnityExtended.Poolable;
using SketchFleets.Entities;
using TMPro;
using UnityEngine.SceneManagement;

namespace SketchFleets.General
{
    /// <summary>
    /// A class that manages a level's flow
    /// </summary>
    public class LevelManager : Singleton<LevelManager>
    {
        #region Private Fields

        [Header("Map Parameters")]
        [SerializeField, Tooltip("The attributes of the current map")]
        private MapAttributes mapAttributes;

        [Header("Spawn Parameters")]
        [SerializeField, Tooltip("The area in which a ship can spawn")]
        private Collider2D spawnArea;
        [SerializeField, Tooltip("The interval between spawning each ship")]
        private FloatReference spawnInterval = new FloatReference(1.5f);
        [SerializeField, Tooltip("The interval between spawning each wave")]
        private FloatReference waveInterval = new FloatReference(1.5f);
        [SerializeField, Tooltip("The variation of the interval between spawning each ship")]
        private FloatReference spawnIntervalDeviation = new FloatReference(0.5f);

        [Header("Variables")]
        [SerializeField]
        private StringReference mapTimer;
        [SerializeField, Tooltip("The seconds passed since the beginning of the level")]
        private IntReference seconds;
        [SerializeField] 
        private TMP_Text pauseShellCount;
        [SerializeField]
        private TMP_Text winShellCount;

        [Header("UI Parameters")]
        [SerializeField, Tooltip("The menu that appears when the game is won")]
        private GameObject victoryMenu;
        [SerializeField, Tooltip("All other UIs that should close when the game is over")]
        private GameObject[] otherUI;

        [Header("Other Parameters")]
        [SerializeField, Tooltip("The total of enemies killed, displayed in the UI")]
        private IntReference totalEnemiesKilled;

        private Mothership player;

        private int activeShips = 0;

        private int mapWaveCount;
        private int currentWave;
        private bool waveStarted;
        private bool gameEnded = false;

        private Coroutine updateTimerRoutine;

        [SerializeField]
        private IntReference pencilShell;

        #endregion

        #region Properties

        public Mothership Player => player;

        public bool GameEnded
        {
            get => gameEnded;
            set => gameEnded = value;
        }

        public TMP_Text PauseShellCount
        {
            get => pauseShellCount;
            set => pauseShellCount = value;
        }

        #endregion

        #region Private Fields

        protected override void Awake()
        {
            base.Awake();
            CacheComponentsAndData();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EndGame();
        }

        private void Start()
        {
            pencilShell.Value = ProfileSystem.Profile.Data.Coins;
            totalEnemiesKilled.Value = ProfileSystem.Profile.Data.Kills;
            if (mapAttributes.Map == 0)
            {
                seconds.Value = 0;
                totalEnemiesKilled.Value = 0;
            }

            updateTimerRoutine = StartCoroutine(UpdateTimer());
            
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
                StartCoroutine(WinGame());
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
                SpawnShip();
                float intervalDeviation = Random.Range(spawnIntervalDeviation * -1f, spawnIntervalDeviation);
                yield return new WaitForSeconds(spawnInterval + intervalDeviation);
            }

            waveStarted = true;
        }

        /// <summary>
        /// Spawn a randomly drawn ship at a random position within the spawn zone
        /// </summary>
        private void SpawnShip()
        {
            ShipAttributes drawnShip = DrawShipAttributeFromPool();
            PoolMember ship = PoolManager.Instance.Request(drawnShip.Prefab);
            ship.Emerge(GetRandomSpawnPoint(), drawnShip.Prefab.transform.rotation);

            activeShips++;
        }

        /// <summary>
        /// Gets a random spawn point within the spawn area
        /// </summary>
        /// <returns>A random spawn point within the spawn area</returns>
        private Vector3 GetRandomSpawnPoint()
        {
            return new Vector3(spawnArea.transform.position.x, GetRandomYInSpawnArea());
        }

        /// <summary>
        /// Gets a random Y coordinate inside the spawn area
        /// </summary>
        /// <returns>A random Y coordinate in the spawn area</returns>
        private float GetRandomYInSpawnArea()
        {
            return spawnArea.transform.position.y + spawnArea.bounds.extents.y * Random.Range(-1f, 1f);
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
        private ShipAttributes DrawShipAttributeFromPool()
        {
            ShipAttributes draw = mapAttributes.EnemyPool[mapAttributes.Difficulty].Draw() as ShipAttributes;

            if (draw == null)
            {
                Debug.LogError("Drawn a null Ship Attribute from Enemy Pool. " +
                               "Make sure all items are ship attributes");
            }

            return draw;
        }

        /// <summary>
        /// Caches all necessary components and data
        /// </summary>
        private void CacheComponentsAndData()
        {
            player = FindObjectOfType<Mothership>();
            mapWaveCount = GenerateMapWaveCount();
        }

        /// <summary>
        /// Freezes and wins the game
        /// </summary>
        private IEnumerator WinGame()
        {
            yield return new WaitForSeconds(3f);
            
            if(mapAttributes.Difficulty == 4)
            {
                SceneManager.LoadScene("EndGame");
                yield break;
            }
            
            ProfileSystem.Profile.Data.Coins = pencilShell.Value;
            ProfileSystem.Profile.Data.TimeSeconds = seconds.Value;
            ProfileSystem.Profile.Data.Kills = totalEnemiesKilled.Value;

            SetOtherMenusActive(false);
            victoryMenu.SetActive(true);
            winShellCount.text = ProfileSystem.Profile.Data.Coins.ToString();

            gameEnded = true;
            Time.timeScale = 0;
        }

        /// <summary>
        /// Closes all other menus
        /// </summary>
        private void SetOtherMenusActive(bool active)
        {
            for (int index = 0, upper = otherUI.Length; index < upper; index++)
            {
                otherUI[index].SetActive(active);
            }
        }

        /// <summary>
        /// Ends the game
        /// </summary>
        private void EndGame()
        {
            Time.timeScale = 1f;
        }

        /// <summary>
        /// Updates the timer
        /// </summary>
        private IEnumerator UpdateTimer()
        {
            WaitForSeconds secondInterval = new WaitForSeconds(1f);

            int start = ProfileSystem.Profile.Data.TimeSeconds;

            while (!gameEnded)
            {
                int seconds = (int) Time.timeSinceLevelLoad + start;
                this.seconds.Value = seconds;
                int minutes = (seconds/60)%60;
                int hours = seconds/(60*60);
                seconds = seconds%60;

                mapTimer.Value = $"{hours:00}:{minutes:00}:{seconds:00}";

                yield return secondInterval;
            }
        }
        

        #endregion
    }
}