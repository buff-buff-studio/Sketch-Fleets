﻿using UnityEngine;

namespace ManyTools.UnityExtended.Poolable
{
    public class PoolMember : MonoBehaviour
    {
        #region Private Fields

        private bool isSubmerged = true;

        #endregion

        #region Properties

        public bool IsSubmerged => isSubmerged;
        public Pool MotherPool { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Submerges the Poolable object into the pool.
        /// </summary>
        public virtual void Submerge()
        {
            StopAllCoroutines();
            DelayProvider.Instance.CancelDoDelayed(GetInstanceID());
            gameObject.SetActive(false);

            if (isSubmerged) return;
            isSubmerged = true;
            MotherPool.CurrentEmerged--;
        }
        
        /// <summary>
        /// Submerges the Poolable object into the pool with a delay.
        /// </summary>
        /// <param name="delay">The delay in seconds to wait before submerging the object</param>
        public void SubmergeDelayed(float delay)
        {
            DelayProvider.Instance.DoDelayed(Submerge, delay, GetInstanceID());
        }

        /// <summary>
        /// Emerges the Poolable object from the pool
        /// </summary>
        /// <param name="position">The position at which to emerge the object</param>
        /// <param name="rotation">The rotation to emerge the object with</param>
        public virtual void Emerge(Vector3 position, Quaternion rotation)
        {
            Transform objectTransform = transform;

            objectTransform.position = position;
            objectTransform.rotation = rotation;

            gameObject.SetActive(true);
            isSubmerged = false;
            MotherPool.CurrentEmerged++;
        }

        #endregion
    }
}