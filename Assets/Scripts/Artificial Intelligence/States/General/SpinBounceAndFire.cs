using System;
using ManyTools.UnityExtended;
using ManyTools.Variables;
using SketchFleets.AI;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// An AI state that aims and fires
    /// </summary>
    public class SpinBounceAndFire : State
    {
        #region Private Fields

        [SerializeField]
        private FloatReference rotationSpeedModifier = new FloatReference(4f);

        private SpawnableShipAI AI;
        private Transform cachedTransform;
        private Vector3 moveDirection;

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs when the state is entered
        /// </summary>
        public override void Enter()
        {
            AI = StateMachine as SpawnableShipAI;
            cachedTransform = transform;
            moveDirection = cachedTransform.up;

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
            float temporalSpeed = AI.Ship.Attributes.Speed * Time.deltaTime;

            cachedTransform.Translate(
                moveDirection * temporalSpeed, Space.World);

            temporalSpeed *= rotationSpeedModifier;
            
            cachedTransform.Rotate(
                new Vector3(0f, 0f, temporalSpeed));

            AI.Ship.Fire();
        }

        /// <summary>
        /// Runs when the state exits
        /// </summary>
        public override void Exit()
        {

        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            moveDirection *= -1f;
        }

        #endregion
    }
}