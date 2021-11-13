using ManyTools.UnityExtended;
using UnityEngine;

namespace SketchFleets.AI
{
    /// <summary>
    /// A base state that handles all AI for  spawnable ships
    /// </summary>
    public class BaseSpawnableAIState : State
    {
        #region Private Fields

        protected SpawnableShipAI AI; 

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs when the state is entered
        /// </summary>
        public override void Enter()
        {
            base.Enter();
            CacheComponents();
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

        #region Private Methods

        /// <summary>
        /// Caches the AI components
        /// </summary>
        private void CacheComponents()
        {
            AI = StateMachine as SpawnableShipAI;

            if (AI == null)
            {
                Debug.LogError($"{GetType().Name} expects a {typeof(SpawnableShipAI)} State Machine!");
            }
        }

        #endregion
    }
}
