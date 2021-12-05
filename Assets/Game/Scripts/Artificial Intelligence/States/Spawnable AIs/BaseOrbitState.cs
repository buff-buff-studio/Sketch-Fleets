using SketchFleets.Data;
using SketchFleets.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SketchFleets.AI
{
    /// <summary>
    /// A base class that handles orbiting behavior
    /// </summary>
    public class BaseOrbitState : BaseSpawnableAIState
    {
        #region Private Fields

        private Transform _crosshair;
        private float _startingOrbitAngle;

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs when the state is entered
        /// </summary>
        public override void Enter()
        {
            base.Enter();

            _crosshair = AI.Player.GetComponent<Mothership>().ShootingTarget.targetPoint;
            _startingOrbitAngle = 360f / (float)AI.Ship.Attributes.MaximumShips * AI.Ship.SpawnNumber;
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            if (AI.Faction == ShipAttributes.Faction.Friendly)
            {
                ParametricOrbit(Time.time * AI.Ship.Attributes.Speed);
            }

            AI.Ship.Look(AI.Faction == ShipAttributes.Faction.Friendly
                ? _crosshair.position
                : AI.Target.transform.position);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Orbits around a point using parametric circle equation
        /// </summary>
        private void ParametricOrbit(float orbitCompletion)
        {
            // Calculate the orbit angle
            orbitCompletion *= 360f;

            // The X and Y positions of the GameObject in the radius
            float orbitX = math.cos(orbitCompletion + _startingOrbitAngle) * AI.Ship.Attributes.OrbitRadius;
            float orbitY = math.sin(orbitCompletion + _startingOrbitAngle) * AI.Ship.Attributes.OrbitRadius;

            // Updates the transform position
            transform.position = AI.Player.transform.position + new Vector3(orbitX, orbitY);
        }

        #endregion
    }
}