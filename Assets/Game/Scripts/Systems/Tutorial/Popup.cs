using System;
using UnityEngine;

namespace SketchFleets.Systems.Tutorial
{
    /// <summary>
    /// A simple class that handles popups
    /// </summary>
    public sealed class Popup : MonoBehaviour
    {
        #region Private Fields

        [Header("Popup Options")]
        [SerializeField]
        private bool PauseOnAppear = true;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            if (PauseOnAppear)
            {
                PauseGame();
            }
        }

        private void OnDestroy()
        {
            ResumeGame();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Destroys the object containing the component
        /// </summary>
        public void Close()
        {
            Destroy(gameObject);
            ResumeGame();
        }

        /// <summary>
        /// Pauses the game
        /// </summary>
        public static void PauseGame()
        {
            Time.timeScale = 0;
        }

        /// <summary>
        /// Resumes the game
        /// </summary>
        public static void ResumeGame()
        {
            Time.timeScale = 1;
        }

        #endregion
    }
}