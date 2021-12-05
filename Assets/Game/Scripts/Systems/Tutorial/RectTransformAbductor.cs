using System;
using UnityEngine;

namespace SketchFleets.Systems.Tutorial
{
    /// <summary>
    /// A class that abducts a Rect Transform so that it can be shown elsewhere
    /// </summary>
    public sealed class RectTransformAbductor : MonoBehaviour
    {
        #region Private Fields

        [Header("Target")]
        [SerializeField]
        private string targetName;
        
        [Header("Destination")]
        [SerializeField]
        private Transform destination;
        
        private GameObject _target;
        private int targetSiblingIndexCache;
        private Vector3 targetPositionCache;
        private Transform targetParentCache;

        #endregion

        #region Unity Callbacks

        private void OnDestroy()
        {
            Return();
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Abducts the target and moves it to the new position
        /// </summary>
        public void Abduct()
        {
            FindTarget();
            if (_target == null) return;
            CacheTargetInfo();
            
            _target.transform.SetParent(destination);
            _target.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Returns the target to its original position
        /// </summary>
        public void Return()
        {
            _target.transform.SetParent(targetParentCache);
            _target.transform.SetSiblingIndex(targetSiblingIndexCache);
            _target.transform.position = targetPositionCache;
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// Finds the target in the scene
        /// </summary>
        private void FindTarget()
        {
            _target = GameObject.Find(targetName);

            if (_target == null)
            {
                Debug.Log("Target not found!");
            }
        }
        
        /// <summary>
        /// Caches information about the target so that it can be returned to its original position
        /// </summary>
        private void CacheTargetInfo()
        {
            targetSiblingIndexCache = _target.transform.GetSiblingIndex();
            targetPositionCache = _target.transform.position;
            targetParentCache = _target.transform.parent;
        }

        #endregion
    }
}