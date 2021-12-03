using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// A class that loops enemies around the screen
    /// </summary>
    public class EnemyLoop : MonoBehaviour
    {
        [SerializeField, Tooltip("The area to teleport the ships to")]
        private Collider2D teleportArea;
        
        #region Unity Callbacks

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Enemy")) return;
            col.gameObject.GetComponent<INonLoopable>()?.PreventLoop();
            col.transform.position = GetRandomSpawnPoint();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a random spawn point within the spawn area
        /// </summary>
        /// <returns>A random spawn point within the spawn area</returns>
        private Vector3 GetRandomSpawnPoint()
        {
            return new Vector3(teleportArea.transform.position.x, GetRandomYInSpawnArea());
        }

        /// <summary>
        /// Gets a random Y coordinate inside the spawn area
        /// </summary>
        /// <returns>A random Y coordinate in the spawn area</returns>
        private float GetRandomYInSpawnArea()
        {
            return teleportArea.transform.position.y + teleportArea.bounds.extents.y * Random.Range(-1f, 1f);
        }

        #endregion
    }
}
