using System;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets.Systems.Tutorial
{
    /// <summary>
    /// A simple class that stores completed turorials
    /// </summary>
    [Serializable]
    public sealed class TutorialData
    {
        #region Public Fields

        [SerializeField]
        public List<string> Completed = new List<string>();

        #endregion
    }
}