using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.AI
{
    /// <summary>
    /// An AI state that aims and fires
    /// </summary>
    public class SpinBounceAndFire : BaseEnemyAIState
    {
        #region Private Fields

        [SerializeField]
        private FloatReference rotationSpeedModifier = new FloatReference(4f);

        private Transform cachedTransform;
        private Vector3 moveDirection;

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs when the state is entered
        /// </summary>
        public override void Enter()
        {
            base.Enter();

            cachedTransform = transform;
            moveDirection = cachedTransform.right;
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            if (!shipRenderer.isVisible) return;

            float temporalSpeed = AI.Ship.Attributes.Speed * Time.deltaTime;

            cachedTransform.Translate(moveDirection * temporalSpeed, Space.World);

            temporalSpeed *= rotationSpeedModifier;
            
            cachedTransform.Rotate(
                new Vector3(0f, 0f, temporalSpeed));

            AI.Ship.Fire();
        }


        #endregion

        #region Unity Callbacks

        private void OnCollisionEnter2D(Collision2D other)
        {
            float random = Random.Range(0.1f, 0.3f);
            Vector3 randomDirection = new Vector3(random, random, 0f);
            moveDirection *= -1f;
            moveDirection += randomDirection;
        }

        #endregion
    }
}