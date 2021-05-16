using ManyTools.UnityExtended;
using ManyTools.Variables;
using SketchFleets.AI;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// An AI state that orbits around the player and fires when he does so
    /// </summary>
    public class OrbitAndExplodeState : State
    {
        #region Private Fields

        [SerializeField]
        private FloatReference launchSpeedMultiplier = new FloatReference(5f);
        
        private SpawnableShipAI AI;
        private float orbitAngle;
        private bool explode = false;

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs when the state is entered
        /// </summary>
        public override void Enter()
        {
            AI = StateMachine as SpawnableShipAI;

            if (Mathf.Approximately(orbitAngle, 0f))
            {
                orbitAngle = Random.Range(1f, 360f);
            }

            if (AI == null)
            {
                Debug.LogError($"{GetType().Name} expects a {typeof(SpawnableShipAI)} State Machine!");
            }

            base.Enter();
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            if (!explode)
            {
                ParametricOrbit();
                AI.Ship.Look(AI.MainCamera.ScreenToWorldPoint(Input.mousePosition));

                if (Input.GetKey(KeyCode.Space))
                {
                    explode = true;
                }
            }
            else
            {
                Vector3 movement = Vector3.up * 
                                   (AI.Ship.Attributes.Speed * Time.deltaTime * launchSpeedMultiplier);
                transform.Translate(movement);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!explode) return;
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerSpawn") ||
                other.gameObject.CompareTag("bullet")) return;
            
            AI.Ship.Die();
        }

        /// <summary>
        /// Runs when the state exits
        /// </summary>
        public override void Exit()
        {
            
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