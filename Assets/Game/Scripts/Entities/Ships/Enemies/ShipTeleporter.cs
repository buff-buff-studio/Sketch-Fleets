using ManyTools.UnityExtended.Poolable;
using SketchFleets.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that handles ship teleportation
    /// </summary>
    [RequireComponent(typeof(Ship<ShipAttributes>))]
    public sealed class ShipTeleporter : MonoBehaviour
    {
        #region Private Fields

        [Header("Teleport Parameters")]
        [SerializeField]
        [Range(0f, 100f)]
        private float teleportChance;

        [SerializeField]
        private float teleportCooldown;

        [Header("Visual Effects")]
        [SerializeField]
        private GameObject teleportEffectPrefab;

        private Ship<ShipAttributes> _ship;
        private float _currentTeleportCooldown;

        #endregion

        #region Properties

        public Bounds TeleportBounds { get; set; }

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            TryGetComponent(out _ship);
            _ship.TookDamage += Teleport;
        }

        private void Update()
        {
            _currentTeleportCooldown -= Time.deltaTime;
        }

        private void OnDisable()
        {
            TryGetComponent(out _ship);
            _currentTeleportCooldown = 0f;
            _ship.TookDamage -= Teleport;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Teleports the ship to somewhere else on the screen
        /// </summary>
        private void Teleport()
        {
            if (!ShouldTeleport()) return;

            InstantiateTeleportEffect();
            transform.position = GetRandomPositionWithin(TeleportBounds);
            InstantiateTeleportEffect();

            _currentTeleportCooldown = teleportCooldown;
        }

        /// <summary>
        /// Instantiates the teleport effect prefab
        /// </summary>
        private void InstantiateTeleportEffect()
        {
            if (teleportEffectPrefab == null) return;
            PoolManager.Instance.Request(teleportEffectPrefab).Emerge(transform.position, Quaternion.identity);
        }

        /// <summary>
        /// Gets a random position within the bounds of the teleport zone
        /// </summary>
        /// <param name="bounds">The bounds of the teleport zone</param>
        /// <returns>A random position within the bounds of the teleport zone</returns>
        private Vector2 GetRandomPositionWithin(Bounds bounds)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns true if the ship should teleport
        /// </summary>
        /// <returns>True if the ship should teleport</returns>
        private bool ShouldTeleport()
        {
            return Random.Range(0f, 100f) <= teleportChance && _currentTeleportCooldown <= 0;
        }

        #endregion
    }
}