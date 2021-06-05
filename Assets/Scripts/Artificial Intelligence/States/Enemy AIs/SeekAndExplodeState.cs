using ManyTools.UnityExtended;
using SketchFleets.AI;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// An AI state that seeks the player and explodes upon colliding with him
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class SeekAndExplodeState : State
    {
        #region Private Fields

        // [SerializeField]
        // private FloatReference accelerationMultiplier = new FloatReference(10f);

        private EnemyShipAI AI;
        private Rigidbody2D rigidbody2d;
        
        #endregion

        #region State Implementation

        /// <summary>
        /// Runs when the state is entered
        /// </summary>
        public override void Enter()
        {
            AI = StateMachine as EnemyShipAI;
            rigidbody2d = GetComponent<Rigidbody2D>();

            if (AI == null)
            {
                Debug.LogError("AimAndFireState expects a EnemyShipAI State Machine!");
            }

            base.Enter();
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            AI.Ship.Look(AI.Player.transform.position);
            rigidbody2d.AddForce(transform.up * (AI.Ship.Attributes.Speed * Time.deltaTime));
            //transform.Translate(transform.up * (AI.Ship.Attributes.Speed * Time.deltaTime), Space.Self);
        }
        
        /// <summary>
        /// Runs when the state exits
        /// </summary>
        public override void Exit()
        {

        }

        #endregion
    }
}