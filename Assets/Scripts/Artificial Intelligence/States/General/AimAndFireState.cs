using ManyTools.UnityExtended;
using SketchFleets.AI;
using UnityEngine;

namespace SketchFleets
{
    public class AimAndFireState : State
    {
        #region Private Fields

        private EnemyShipAI AI;
        
        #endregion

        #region State Implementation

        /// <summary>
        /// Runs when the state is entered
        /// </summary>
        public override void Enter()
        {
            AI = StateMachine as EnemyShipAI;
            Debug.Log(StateMachine);

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
            AI.Ship.Fire();
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