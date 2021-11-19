using System.Collections.Generic;
using System.Linq;
using ManyTools.UnityExtended;
using SketchFleets.Data;
using SketchFleets.Enemies;
using SketchFleets.Entities;
using UnityEngine;

namespace SketchFleets.AI
{
    /// <summary>
    /// A state machine that controls the AI of a ship
    /// </summary>
    [RequireComponent(typeof(EnemyShip))]
    public sealed class EnemyShipAI : StateMachine, IShipAI
    {
        #region Private Fields

        private GameObject _target;

        #endregion

        #region Properties

        public GameObject Target
        {
            get
            {
                if (!HasCachedTarget)
                {
                    TryGetTarget(out _target);
                }

                return _target;
            }
        }

        public EnemyShip Ship { get; private set; }
        private bool HasCachedTarget => _target != null && _target.activeSelf;

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            Ship = GetComponent<EnemyShip>();
            Faction = Ship.Attributes.ShipFaction;
        }

        #endregion

        #region IShipAI Implementation

        public ShipAttributes.Faction Faction { get; private set; }

        /// <summary>
        /// Sets the ship's faction
        /// </summary>
        /// <param name="faction">The faction to set the ship to</param>
        public void SetFaction(ShipAttributes.Faction faction)
        {
            Faction = faction;
            TryGetTarget(out _target);
        }

        /// <summary>
        /// Flips the ship's faction. Neutral ships cannot be flipped.
        /// </summary>
        public void FlipFaction()
        {
            if (Faction == ShipAttributes.Faction.Neutral) return;

            Faction = Faction == ShipAttributes.Faction.Friendly
                ? ShipAttributes.Faction.Hostile
                : ShipAttributes.Faction.Friendly;
            
            TryGetTarget(out _target);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Attempts to get a target for the ship
        /// </summary>
        /// <param name="target">The found target</param>
        /// <returns>Whether it could acquire a target for the ship</returns>
        private bool TryGetTarget(out GameObject target)
        {
            target = GetClosestTransform(GetAllShips())?.gameObject;
            return target != null;
        }

        /// <summary>
        /// Gets the closest out of a list of transforms
        /// </summary>
        /// <param name="transforms">The list of transforms to get the closest of</param>
        /// <returns>The closest out of the given transforms</returns>
        private Transform GetClosestTransform(IReadOnlyList<Transform> transforms)
        {
            Transform closestTransform = null;

            float smallestDistance = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            for (int index = 0, max = transforms.Count; index < max; index++)
            {
                Vector3 directionToTarget = transforms[index].position - currentPosition;
                float distance = directionToTarget.sqrMagnitude;

                if (!(distance < smallestDistance)) continue;

                smallestDistance = distance;
                closestTransform = transforms[index];
            }

            return closestTransform;
        }

        private List<Transform> GetAllShips()
        {
            // TODO: Change this to simply fetching a list from the EnemySpawner once the formations branch gets merged

            if (Faction == ShipAttributes.Faction.Friendly)
            {
                return Ship.Spawner.ActiveEnemyShips;
            }
            else
            {
                return GameObject.FindGameObjectsWithTag("PlayerSpawn")
                    .Select(ship => ship.transform)
                    .Append(FindObjectOfType<Mothership>().transform).ToList();
            }
        }

        #endregion
    }
}