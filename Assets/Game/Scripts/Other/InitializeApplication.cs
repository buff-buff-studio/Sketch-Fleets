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
            LoadMainMenu();
#if !UNITY_EDITOR
            Instantiate(backTraceClient);
#endif
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Loads the game's main menu
        /// </summary>
        private void LoadMainMenu()
        {
            SceneManager.LoadSceneAsync(menuSceneName);
        }

        #endregion
    }
}