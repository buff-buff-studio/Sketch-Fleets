using ManyTools.Variables;
using UnityEngine;
using UnityEngine.Events;

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
        private float timeScaleOnAppear = 0f;

        [SerializeField]
        private bool blockOtherPopups = false;

        [SerializeField]
        private BoolReference nextStepIsBlocked;

        [SerializeField]
        private bool unblockOnDestroy = false;

        [Header("Popup Events")]
        [SerializeField]
        private UnityEvent onOpen;

        [SerializeField]
        private UnityEvent onClose;

        private bool resume = true;

        #endregion

        #region Properties

        public TutorialStep Step { get; set; }

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            onOpen.Invoke();

            if (nextStepIsBlocked != null)
            {
                nextStepIsBlocked.Value = blockOtherPopups;
            }

            EditTimeScale();
        }

        private void OnDestroy()
        {
            if (unblockOnDestroy && nextStepIsBlocked != null)
            {
                nextStepIsBlocked.Value = false;
            }

            if (resume)
            {
                ResumeGame();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Steps forward in the parent tutorial
        /// </summary>
        public void StepForwardInTutorial() => Step.StepForward();

        /// <summary>
        /// Ends the tutorial of the parent step
        /// </summary>
        public void EndParentTutorial() => Step.EndTutorial();

        /// <summary>
        /// Destroys the object containing the component
        /// </summary>
        public void Close()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Destroys the object containing the component
        /// </summary>
        public void CloseWithoutResuming()
        {
            resume = false;
            Destroy(gameObject);
        }

        /// <summary>
        /// Pauses the game
        /// </summary>
        public void EditTimeScale()
        {
            Time.timeScale = timeScaleOnAppear;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        /// <summary>
        /// Resumes the game
        /// </summary>
        public static void ResumeGame()
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        #endregion
    }
}