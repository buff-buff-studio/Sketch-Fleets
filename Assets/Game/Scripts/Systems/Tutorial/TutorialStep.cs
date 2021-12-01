using System;
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

        [HideInInspector]
        public bool IsNew = true;

        #endregion

        #region Properties

        public Canvas Canvas { get; set; }

        #endregion

        #region Public Methods

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
        private void InstantiatePopUp()
        {
            GameObject popup = Object.Instantiate(Popup, Canvas.transform);
            popup.transform.SetAsLastSibling();
        }

        #endregion
    }
}