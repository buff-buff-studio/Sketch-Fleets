using UnityEngine;
using UnityEngine.SceneManagement;

namespace SketchFleets
{
    /// <summary>
    ///     A script that runs on the application's initialize
    /// </summary>
    public sealed class InitializeApplication : MonoBehaviour
    {
        #region Private Fields

        [Header("Game Values")]
        [SerializeField]
        private RefreshRate targetFramerate;

        [Header("Loading")]
        [SerializeField]
        private string menuSceneName;

        [Header("Logging")]
        [SerializeField]
        private GameObject backTraceClient;

        #endregion

        #region Unity Callbacks

        // Start is called before the first frame update
        private void Start()
        {
            SetTargetFramerate();
            LoadMainMenu();
#if !UNITY_EDITOR
            CreateBackTraceClient();
#endif

#if DEVELOPMENT_BUILD
            Debug.Log(Application.targetFrameRate);
#endif
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Sets the application's target framerate
        /// </summary>
        private void SetTargetFramerate()
        {
            Application.targetFrameRate = (int)targetFramerate;
        }

        /// <summary>
        ///     Loads the game's main menu
        /// </summary>
        private void LoadMainMenu()
        {
            SceneManager.LoadSceneAsync(menuSceneName);
        }

        /// <summary>
        ///     Creates a backtrace client for error-tracking
        /// </summary>
        private void CreateBacktraceClient()
        {
            Instantiate(backTraceClient);
        }

        #endregion

        #region Private Enums

        /// <summary>
        ///     A simple refresh rate enumeration
        /// </summary>
        private enum RefreshRate
        {
            PlatformDefault = -1,
            Capped30 = 30,
            Capped60 = 60
        }

        #endregion
    }
}