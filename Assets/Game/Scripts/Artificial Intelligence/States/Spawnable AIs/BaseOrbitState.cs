using SketchFleets.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SketchFleets.AI
{
    /// <summary>
    /// A base class that handles orbiting behavior
    /// </summary>
    public class BaseOrbitState : BaseSpawnableAIState
    {
        #region Private Fields

        private float startingOrbitAngle;
        private float orbitAngle;

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs when the state is entered
        /// </summary>
        public override void Enter()
        {
            AI = StateMachine as SpawnableShipAI;

            if (AI == null)
            {
                Debug.LogError($"{GetType().Name} expects a {typeof(SpawnableShipAI)} State Machine!");
            }
            
            startingOrbitAngle = 360f / (float)AI.Ship.Attributes.MaximumShips * AI.Ship.SpawnNumber;

            base.Enter();
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            ParametricOrbit(Time.time * AI.Ship.Attributes.Speed);
            AI.Ship.Look(AI.Player.GetComponent<Mothership>()._ShootingTarget.targetPoint.position);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Orbits around a point using parametric circle equation
        /// </summary>
        private void ParametricOrbit(float orbitCompletion)
        {
            // 
            orbitCompletion *= 360f;
            
            // The X and Y positions of the GameObject in the radius
            float orbitX = math.cos(orbitCompletion + startingOrbitAngle) * AI.Ship.Attributes.OrbitRadius;
            float orbitY = math.sin(orbitCompletion + startingOrbitAngle) * AI.Ship.Attributes.OrbitRadius;

            // Updates the transform position
            transform.position = AI.Player.transform.position + new Vector3(orbitX, orbitY);
        }

        #endregion
    }
}
