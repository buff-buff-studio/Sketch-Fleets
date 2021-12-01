using ManyTools.UnityExtended.Poolable;
using UnityEngine;

namespace SketchFleets.General
{
    /// <summary>
    /// A class that kills GameObjects which touch it
    /// </summary>
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public sealed class DeathZone : MonoBehaviour
    {
        #region Unity Callbacks

        private void OnCollisionEnter2D(Collision2D other)
        {
            // Attempts to get a poolable out of colliding object
            if (other.gameObject.TryGetComponent(out PoolMember poolable))
            {
                Debug.Log(poolable);
                poolable.Submerge();
            }
            else
            {
                Debug.Log(other);
                Destroy(other.gameObject);
            }
        }

        #endregion
    }
}
