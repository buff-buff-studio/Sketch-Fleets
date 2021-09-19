using ManyTools.UnityExtended;
using ManyTools.UnityExtended.Poolable;
using SketchFleets.Data;
using SketchFleets.General;
using UnityEngine;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that controls a spawned ship
    /// </summary>
    public sealed class SpawnedShip : Ship<SpawnableShipAttributes>
    {
        #region Properties

        public int SpawnNumber { get; set; }

        #endregion

        #region PoolMember Overrides

        /// <summary>
        /// Emerges the Poolable object from the pool
        /// </summary>
        /// <param name="position">The position at which to emerge the object</param>
        /// <param name="rotation">The rotation to emerge the object with</param>
        public override void Emerge(Vector3 position, Quaternion rotation)
        {
            base.Emerge(position, rotation);
            // The invoke here is beyond horrible in terms of performance, but I'd rather not spend
            // more time in this script, the deadline is looming
            DelayProvider.Instance.DoDelayed(EmergeSpawnEffect, 0.1f, GetInstanceID());
        }

        #endregion
        
        #region Ship Overrides

        /// <summary>
        /// Makes the ship die
        /// </summary>
        public override void Die()
        {
            LevelManager.Instance.Player.RemoveActiveSummon(this);

            base.Die();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Emerges a spawn effect at the ship's position
        /// </summary>
        private void EmergeSpawnEffect()
        {
            Transform cachedTransform = transform;
            PoolManager.Instance.Request(Attributes.SpawnEffect).
                Emerge(cachedTransform.position, cachedTransform.rotation);
        }

        #endregion
    }
}