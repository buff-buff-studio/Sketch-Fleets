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

        private ShipAttributes entry;
        
        #endregion

        #region Properties

        public ShipAttributes Entry
        {
            get => entry;
            set => entry = value;
        }

        #endregion

        #region ICollectible Implementation

        /// <summary>
        /// Applies all necessary effects upon collection
        /// </summary>
        public void Collect()
        { 
            CodexListener.Instance.CollectEntry(Entry);
            Submerge();
        }

        #endregion

        #region Unity Callbacks

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Collect();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Defines the collectible's entry
        /// </summary>
        /// <param name="ship"></param>
        private void DefineEntry(ShipAttributes ship)
        {
            Entry = ship;
        }
        
        #endregion
    }
}