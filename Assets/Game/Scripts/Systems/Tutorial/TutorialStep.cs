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

        public bool IsNew = true;
        public bool IsComplete;

        #endregion

        #region Public Methods

        public void Begin()
        {
            IsNew = false;
            Object.Instantiate(Popup);
        }

        #endregion
    }
}