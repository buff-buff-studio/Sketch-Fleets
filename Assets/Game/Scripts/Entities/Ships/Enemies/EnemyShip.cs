using ManyTools.UnityExtended;
using SketchFleets.Data;
using SketchFleets.General;
using UnityEngine;

namespace SketchFleets.Enemies
{
    /// <summary>
    /// A class that controls an enemy ship
    /// </summary>
    public sealed class EnemyShip : Ship<ShipAttributes>
    {
        /// <summary>
        /// Emerges the Poolable object from the pool
        /// </summary>
        /// <param name="position">The position at which to emerge the object</param>
        /// <param name="rotation">The rotation to emerge the object with</param>
        public override void Emerge(Vector3 position, Quaternion rotation)
        {
            base.Emerge(position, rotation);
            
            // TODO: This is a band-aid to fix a bug. Make sure to remove it later
            DelayProvider.Instance.DoDelayed(Die, 60f, GetInstanceID());
        }

        public override void Die()
        {
            base.Die();

            LevelManager.Instance.CountShipDeath();
        }
    }
}