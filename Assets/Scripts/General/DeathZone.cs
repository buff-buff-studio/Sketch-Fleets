using ManyTools.UnityExtended.Poolable;
using UnityEngine;

namespace SketchFleets.General
{
    /// <summary>
    /// A class that kills GameObjects which touch it
    /// </summary>
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class DeathZone : MonoBehaviour
    {
        #region Unity Callbacks

        private void OnCollisionEnter2D(Collision2D other)
        {
            // Attempts to get a poolable out of colliding object
            PoolMember poolable = other.gameObject.GetComponent<PoolMember>();

            if (poolable == null)
            {
                // If is poolable, submerge
                poolable.Submerge();
            }
            else
            {
                // If not, destroy
                Destroy(other.gameObject);
            }
        }

        #endregion
    }
}
