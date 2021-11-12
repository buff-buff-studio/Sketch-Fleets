using System.Collections.Generic;
using System.Linq;
using ManyTools.UnityExtended;
using SketchFleets.Data;
using SketchFleets.Entities;
using UnityEngine;

namespace SketchFleets.AI
{
    /// <summary>
    /// A state machine that handles the AI of spawned ships.
    /// </summary>
    [RequireComponent(typeof(SpawnedShip))]
    public sealed class SpawnableShipAI : StateMachine, IShipAI
    {
        #region Private Fields

        private GameObject _target;

        #endregion

        #region Properties

        public GameObject Player { get; private set; }
        public SpawnedShip Ship { get; private set; }
        public Camera MainCamera { get; private set; }

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

        private bool HasCachedTarget => _target != null && _target.activeSelf;


        #endregion
        
        #region Unity Callbacks

        private void Awake()
        {
            Ship = GetComponent<SpawnedShip>();
            Faction = Ship.Attributes.ShipFaction;
            MainCamera = Camera.main;
            
            // TODO: Replace with a better way to get the player.
            Player = GameObject.FindWithTag("Player");
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

        private Transform[] GetAllShips()
        {
            // TODO: Change this to simply fetching a list from the EnemySpawner once the formations branch gets merged

            if (Ship.Attributes.ShipFaction == ShipAttributes.Faction.Friendly)
            {
                return GameObject.FindGameObjectsWithTag("Enemy")
                    .Select(ship => ship.transform)
                    .ToArray();
            }
            else
            {
                return GameObject.FindGameObjectsWithTag("PlayerSpawn")
                    .Select(ship => ship.transform)
                    .Append(FindObjectOfType<Mothership>().transform).ToArray();
            }
        }

        #endregion
    }
}