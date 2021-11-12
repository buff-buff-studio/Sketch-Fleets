using SketchFleets.AI;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// An AI state that acquires targets and fires at them
    /// </summary>
    public sealed class AcquireAndDestroy : BaseSpawnableAIState
    {
        #region Private Fields

        private Transform CachedTransform { get; set; }

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

            base.Enter();
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            base.StateUpdate();
            transform.Translate(AI.Ship.Attributes.Speed * Time.deltaTime * Time.timeScale,
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

        #region Unity Callbacks

        private void OnEnable()
        {
            if (CachedTransform == null)
            {
                CachedTransform = transform;
            }

            CachedTransform.up = CachedTransform.right;
        }

        #endregion
    }
}