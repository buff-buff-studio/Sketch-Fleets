using SketchFleets.AI;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// An AI state that seeks the player and explodes upon colliding with him
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class AccelerateAndExplodeState : BaseEnemyAIState
    {
        #region Private Fields

        private Rigidbody2D rigidbody2d;
        private Transform cachedTransform;

        #endregion

        #region Unity Callbacks

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (CollisionIsBullet(collision)) return;
            AI.Ship.Damage(AI.Ship.CurrentHealth * 2);
        }

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs when the state is entered
        /// </summary>
        public override void Enter()
        {
            AI = StateMachine as EnemyShipAI;
            rigidbody2d = GetComponent<Rigidbody2D>();
            cachedTransform = transform;

            if (AI == null)
            {
                Debug.LogError("AccelerateAndExplodeState expects a EnemyShipAI State Machine!");
            }

            base.Enter();
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            if (!ShouldBeActive()) return;

            AI.Ship.Look(AI.Target.transform.position.x > cachedTransform.position.x ? Vector2.right : Vector2.left);
            rigidbody2d.AddForce(transform.up * (AI.Ship.Attributes.Speed * Time.deltaTime));
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
        /// Checks whether a given 2D collision is with a bullet
        /// </summary>
        /// <param name="collision">The collision to check</param>
        /// <returns>Whether it is with a bullet</returns>
        private static bool CollisionIsBullet(Collision2D collision)
        {
            return collision.gameObject.CompareTag("bullet");
        }

        #endregion
    }
}