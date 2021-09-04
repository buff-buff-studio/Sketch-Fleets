using ManyTools.UnityExtended;
using UnityEngine;

namespace SketchFleets.AI
{
    /// <summary>
    /// A base state that handles all AI for  spawnable ships
    /// </summary>
    public class BaseEnemyAIState : State
    {
        #region Private Fields

        protected EnemyShipAI AI;
        protected Renderer shipRenderer;

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs when the state is entered
        /// </summary>
        public override void Enter()
        {
            base.Enter();

            AI = StateMachine as EnemyShipAI;

            shipRenderer = GetComponent<Renderer>();

            if (AI == null)
            {
                Debug.LogError($"{GetType().Name} expects a {typeof(EnemyShipAI)} State Machine!");
            }
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            
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
