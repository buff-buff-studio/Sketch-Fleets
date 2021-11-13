using ManyTools.UnityExtended.Poolable;
using SketchFleets.Data;
using SketchFleets.Systems.Codex;
using UnityEngine;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that controls a drop containing a codex entry
    /// </summary>
    public class CodexEntryDrop : PoolMember, ICollectible
    {
        #region Private Fields

        [Header("Parameters")]
        [SerializeField, Tooltip("The effect spawned upon collection")]
        private GameObject deathEffect;
        
        private ShipAttributes entry;
        private bool collected;

        #endregion

        #region Properties

        public ShipAttributes Entry
        {
            get => entry;
            set => entry = value;
        }

        #endregion

        #region PoolMember Overrides

        /// <summary>
        /// Emerges the Poolable object from the pool
        /// </summary>
        /// <param name="position">The position at which to emerge the object</param>
        /// <param name="rotation">The rotation to emerge the object with</param>
        public override void Emerge(Vector3 position, Quaternion rotation)
        {
            collected = false;
            base.Emerge(position, rotation);
        }

        /// <summary>
        /// Submerges the Poolable object into the pool.
        /// </summary>
        public override void Submerge()
        {
            Transform cachedTransform = transform;
            PoolManager.Instance.Request(deathEffect).Emerge(cachedTransform.position, cachedTransform.rotation);
            base.Submerge();
        }

        #endregion
        
        #region ICollectible Implementation

        /// <summary>
        /// Applies all necessary effects upon collection
        /// </summary>
        public void Collect()
        {
            CodexListener.Instance.CollectEntry(Entry);
            collected = true;
            Submerge();
        }

        #endregion

        #region Unity Callbacks

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || collected) return;
            Collect();
        }

        #endregion
    }
}