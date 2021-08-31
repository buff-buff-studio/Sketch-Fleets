using UnityEngine;

namespace SketchFleets.UI
{
    /// <summary>
    /// Handles the logic of closing the application
    /// </summary>
    public class ExitButton : MonoBehaviour
    {
        // NOTE: The existence of this class is due to not using the GameManager class I made.
        // It should have been used.
        #region Public Methods

        /// <summary>
        /// Exits the application in editor or in runtime
        /// </summary>
        public void ExitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        #endregion
    }
}