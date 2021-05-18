using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManyTools.Tests
{
    /// <summary>
    /// A class that contains a single bool, for testing
    /// </summary>
    public class TestValueContainer : MonoBehaviour
    {
        #region Public Fields

        public bool raised;
        public int value;

        #endregion

        #region Properties

        public bool Raised
        {
            get => raised;
            set => raised = value;
        }

        public int Value
        {
            get => value;
            set => this.value = value;
        }

        #endregion
    }
}
