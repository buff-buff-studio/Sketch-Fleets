using System.Collections.Generic;
using System.Linq;
using ManyTools.UnityExtended;
using UnityEngine.SceneManagement;

namespace SketchFleets.Systems.Tutorial
{
    /// <summary>
    /// A class that manages the tutorial
    /// </summary>
    public sealed class TutorialManager : Singleton<TutorialManager>
    {
        #region MyRegion

        private Dictionary<Tutorial, bool> _tutorialElements = new Dictionary<Tutorial, bool>();
        private Tutorial[] _activeTutorialElements;

        #endregion

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();
            
            if (HaveTutorialsBeenCompleted())
            {
                Destroy(gameObject);
            }

            SceneManager.sceneLoaded += GetActiveTutorials;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            SceneManager.sceneLoaded -= GetActiveTutorials;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks whether all tutorial elements have been completed
        /// </summary>
        /// <returns>Whether all registered tutorial elements have been completed</returns>
        private bool HaveTutorialsBeenCompleted()
        {
            return _tutorialElements.Values.All(element => element);
        }

        /// <summary>
        /// Completes all tutorial elements
        /// </summary>
        /// <param name="element">The element to complete</param>
        private void CompleteTutorial(Tutorial element)
        {
            _tutorialElements[element] = true;
        }

        /// <summary>
        /// Gets all active (placed) tutorial elements
        /// </summary>
        private void GetActiveTutorials(Scene scene, LoadSceneMode mode)
        {
            _activeTutorialElements = FindObjectsOfType<Tutorial>(true)
                .Where(element => _tutorialElements[element] == false).ToArray();
        }

        #endregion
    }
}