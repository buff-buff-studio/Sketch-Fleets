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
        private FloatReference bounceCooldown = new FloatReference(4f);
        [SerializeField]
        private FloatReference rotationSpeedModifier = new FloatReference(4f);

        private Transform cachedTransform;
        private Vector3 moveDirection;
        private float bounceTimer;

        #endregion

        #region Properties

        protected bool CanBounce => bounceTimer <= 0f;

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs when the state is entered
        /// </summary>
        public override void Enter()
        {
            base.Enter();

            cachedTransform = transform;
            moveDirection = cachedTransform.up;
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            if (!shipRenderer.isVisible) return;

            float temporalSpeed = AI.Ship.Attributes.Speed * Time.deltaTime;

            cachedTransform.Translate(moveDirection * temporalSpeed, Space.World);
            Spin(temporalSpeed);

            AI.Ship.Fire();

            bounceTimer -= Time.deltaTime * Time.timeScale;
        }

        #endregion

        #region Unity Callbacks

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log(other.gameObject.name);
            Bounce(!other.gameObject.CompareTag("bullet"));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Spins the ship
        /// </summary>
        private void Spin(float speed)
        {
            speed *= rotationSpeedModifier;
            cachedTransform.Rotate(new Vector3(0f, 0f, speed));
        }
        
        /// <summary>
        /// Bounces the ship
        /// </summary>
        /// <param name="ignoreCooldown">Whether to ignore the bounce cooldown</param>
        private void Bounce(bool ignoreCooldown = true)
        {
            if (!ignoreCooldown)
            {
                if (!CanBounce) return;
            }

            float random = Random.Range(0.1f, 0.3f);
            Vector3 randomDirection = new Vector3(random, random, 0f);
            moveDirection *= -1f;
            moveDirection += randomDirection;

            bounceTimer = bounceCooldown;
        }

        #endregion
    }
}