using System.Collections;
using ManyTools.UnityExtended;
using SketchFleets.AI;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// An AI state that acquires targets and fires at them
    /// </summary>
    public class AcquireAndDestroy : BaseSpawnableAIState
    {
        #region Private Fields
        
        Transform cachedTransform;

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
                Debug.LogError("AimAndFireState expects a EnemyShipAI State Machine!");
            }

            cachedTransform = transform;
            base.Enter();
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            base.StateUpdate();
            cachedTransform.up = cachedTransform.right;
            cachedTransform.Translate(AI.Ship.Attributes.Speed * Time.deltaTime * Time.timeScale,
                0f, 0f, Space.World);

            AI.Ship.Fire();
        }

        /// <summary>
        /// Runs when the state exits
        /// </summary>
        public override void Exit()
        {
            StopAllCoroutines();
        }

        #endregion
    }
}