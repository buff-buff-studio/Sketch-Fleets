using System.Collections;
using ManyTools.UnityExtended;
using ManyTools.Variables;
using SketchFleets.Entities;
using SketchFleets.ProfileSystem;
using SketchFleets.Systems;
using TMPro;
using UnityEngine;

namespace SketchFleets.General
{
    /// <summary>
    /// A class that manages a level's flow
    /// </summary>
    public sealed class LevelManager : Singleton<LevelManager>
    {
        #region Private Fields

        [Header("Variables")]
        [SerializeField]
        private StringReference mapTimer;

        [SerializeField]
        [Tooltip("The seconds passed since the beginning of the level")]
        private IntReference seconds;

        [SerializeField]
        private TMP_Text pauseShellCount;

        [SerializeField]
        private TMP_Text winShellCount;

        [Header("UI Parameters")]
        [SerializeField]
        [Tooltip("The menu that appears when the game is won")]
        private GameObject victoryMenu;

        [SerializeField]
        [Tooltip("All other UIs that should close when the game is over")]
        private GameObject[] otherUI;

        [Header("Other Parameters")]
        [SerializeField]
        [Tooltip("The total of enemies killed, displayed in the UI")]
        private IntReference totalEnemiesKilled;

        [SerializeField]
        private IntReference pencilShell;

        private Coroutine updateTimerRoutine;

        #endregion

        #region Properties

        public Mothership Player { get; private set; }

        public bool GameEnded { get; set; }

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
            GetComponent<ShipSpawner>().AllWavesAreOver -= WinGame;
            EndGame();
        }

        private void Start()
        {
            pencilShell.Value = Profile.Data.Coins;
            totalEnemiesKilled.Value = Profile.Data.Kills;

            updateTimerRoutine = StartCoroutine(UpdateTimer());
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Caches all necessary components and data
        /// </summary>
        private void CacheComponentsAndData()
        {
            Player = FindObjectOfType<Mothership>();
            GetComponent<ShipSpawner>().AllWavesAreOver += WinGame;
        }

        /// <summary>
        /// 
        /// </summary>
        private void WinGame()
        {
            StartCoroutine(ShowPostGame());
        }
        
        /// <summary>
        /// Freezes and wins the game
        /// </summary>
        private IEnumerator ShowPostGame()
        {
            yield return new WaitForSeconds(3f);

            SavePlayerProgress();
            ShowWinUI();

            GameEnded = true;
            Time.timeScale = 0;
        }

        /// <summary>
        /// Shows the win UI
        /// </summary>
        private void ShowWinUI()
        {
            SetOtherMenusActive(false);
            victoryMenu.SetActive(true);
            winShellCount.text = Profile.Data.Coins.ToString();

            StartCoroutine(LerpGameOverScreen());
        }

        IEnumerator LerpGameOverScreen()
        {
            float timer = Time.unscaledTime;
            float actual = 0;
            CanvasGroup canvasGroup = victoryMenu.GetComponent<CanvasGroup>();
            while ((actual = Time.unscaledTime - timer) < 1.5f)
            {
                canvasGroup.alpha = actual / 1.5f;
                yield return 0;
            }
        }

        /// <summary>
        /// Saves player progress
        /// </summary>
        private void SavePlayerProgress()
        {
            Profile.Data.Coins = pencilShell.Value;
            Profile.Data.TimeSeconds = seconds.Value;
            Profile.Data.Kills = totalEnemiesKilled.Value;
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
        private static void EndGame()
        {
            Time.timeScale = 1f;
        }

        /// <summary>
        /// Updates the timer
        /// </summary>
        private IEnumerator UpdateTimer()
        {
            WaitForSeconds secondInterval = new WaitForSeconds(1f);

            int start = Profile.Data.TimeSeconds;

            while (!GameEnded)
            {
                int secondsSinceStart = (int)Time.timeSinceLevelLoad + start;
                seconds.Value = secondsSinceStart;
                int minutes = secondsSinceStart / 60 % 60;
                int hours = secondsSinceStart / (60 * 60);
                secondsSinceStart %= 60;

                mapTimer.Value = $"{hours:00}:{minutes:00}:{secondsSinceStart:00}";

                yield return secondInterval;
            }
        }

        #endregion
    }
}