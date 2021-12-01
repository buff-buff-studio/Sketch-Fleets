using UnityEngine;

namespace SketchFleets.Systems.Tutorial
{
    /// <summary>
    /// A simple class that handles popups
    /// </summary>
    public sealed class Popup : MonoBehaviour
    {
        #region Public Methods

        /// <summary>
        /// Destroys the object containing the component
        /// </summary>
        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}