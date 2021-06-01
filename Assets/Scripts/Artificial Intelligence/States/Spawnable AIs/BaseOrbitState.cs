using ManyTools.UnityExtended;
using UnityEngine;

namespace SketchFleets.AI
{
    /// <summary>
    /// A base class that handles orbiting behavior
    /// </summary>
    public class BaseOrbitState : BaseSpawnableAIState
    {
        #region Private Fields

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
            
            orbitAngle = (360f / (float)AI.Ship.Attributes.MaximumShips) * AI.Ship.SpawnNumber;

            base.Enter();
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            ParametricOrbit();
            AI.Ship.Look(AI.MainCamera.ScreenToWorldPoint(Input.mousePosition));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Orbits around a point using parametric circle equation
        /// </summary>
        private void ParametricOrbit()
        {
            // The current angle around the circumference
            orbitAngle += AI.Ship.Attributes.Speed * Time.deltaTime;

            // The X and Y positions of the GameObject in the radius
            float orbitX = Mathf.Cos(orbitAngle) * AI.Ship.Attributes.OrbitRadius;
            float orbitY = Mathf.Sin(orbitAngle) * AI.Ship.Attributes.OrbitRadius;

            // Updates the transform position
            transform.position = AI.Player.transform.position + new Vector3(orbitX, orbitY);
        }

        #endregion
    }
}
