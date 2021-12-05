using System;
using System.Threading.Tasks;
using ManyTools.Events;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SketchFleets.Systems.Tutorial
{
    /// <summary>
    /// A class that handles an individual tutorial step.
    /// </summary>
    [Serializable]
    public sealed class TutorialStep
    {
        #region Public Fields

        [SerializeField]
        public GameEvent Trigger;

        [SerializeField]
        public GameObject Popup;

        [SerializeField]
        public float PopupDelay = 0f;

        #endregion

        #region Private Fields

        private bool isNew = true;
        private Canvas canvasCache;

        #endregion

        #region Properties

        public Tutorial Tutorial { get; set; }

        public Canvas Canvas
        {
            get
            {
                canvasCache ??= GameObject.FindGameObjectWithTag("TutorialReceiver").GetComponent<Canvas>();
                return canvasCache;
            }
        }

        public bool IsNew
        {
            get => isNew;
            set => isNew = value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Ends the whole tutorial and marks it as complete
        /// </summary>
        public void EndTutorial() => Tutorial.CompleteTutorial();

        /// <summary>
        /// Shows the tutorial's next step
        /// </summary>
        public void StepForward() => Tutorial.StepForward();

        /// <summary>
        /// Shows the popup and begins the step
        /// </summary>
        public void Begin()
        {
            IsNew = false;
            InstantiatePopUp();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Instantiates the popup in the canvas
        /// </summary>
        private async void InstantiatePopUp()
        {
            if (PopupDelay > 0)
            {
                await Task.Delay((int)(PopupDelay * 1000));
            }

            Popup popup = Object.Instantiate(Popup, Canvas.transform).GetComponent<Popup>();
            popup.Step = this;
            popup.transform.SetAsLastSibling();
        }

        #endregion
    }
}