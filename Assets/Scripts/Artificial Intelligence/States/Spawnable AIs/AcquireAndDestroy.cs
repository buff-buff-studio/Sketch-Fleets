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

        [SerializeField]
        [Tooltip("The maximum range at which the ship will attack")]
        private float attackRange = 50f;
        
        private GameObject target;
        private Vector3 targetPosition;
        private bool inCombat = false;

        private IEnumerator updateTargetRoutine;

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

            updateTargetRoutine = UpdateTarget();
            StartCoroutine(updateTargetRoutine);

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
            
            if (!inCombat) return;
            
            AI.Ship.Look(targetPosition);
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

        #region Private Methods

        /// <summary>
        /// Gets the nearest enemy
        /// </summary>
        /// <returns>The nearest enemy</returns>
        private GameObject GetTarget()
        {
            // Caches the current position
            Vector3 currentPosition = transform.position;

            // Gets nearby enemies
            Collider2D[] existingEntities = new Collider2D[10];
            int bitWiseLayerMask = LayerMask.GetMask("Enemy");
            Physics2D.OverlapCircleNonAlloc(currentPosition, attackRange, existingEntities, bitWiseLayerMask);

            // If no objects were caught, end things here
            if (existingEntities[0] == null) return null;

            // Defaults the closest object and distance to closest
            GameObject closest = null;
            Vector3 distanceToClosest = default;

            // Gets the closest of all active entities
            for (int index = 1, upper = existingEntities.Length; index < upper; index++)
            {
                // If the entry is empty of disabled (submerged in pool), skip
                if (existingEntities[index] == null) continue;
                if (existingEntities[index].gameObject.activeSelf == false) continue;

                // If values are default/null, define them
                if (closest == null || distanceToClosest == default)
                {
                    closest = existingEntities[index].gameObject;
                    distanceToClosest = currentPosition - closest.transform.position;
                }

                // Calculates the distance between the current position and entity position
                Vector3 distance = currentPosition - existingEntities[index].transform.position;

                // If the distance is smaller than the currently smallest known distance
                if (!(distance.sqrMagnitude < distanceToClosest.sqrMagnitude)) continue;

                // Update the closest distance and closest GameObject
                distanceToClosest = distance;
                closest = existingEntities[index].gameObject;
            }

            return closest;
        }

        /// <summary>
        /// Constantly updates the target and its position, as well as the AI's combat state
        /// </summary>
        private IEnumerator UpdateTarget()
        {
            // NOTE: This Coroutine may be a very bad idea. It calls GetTarget, which allocates, but
            // the IEnumerator won't release its garbage to the collector until it ends, which it never
            // does. Check this with Mol or someone who understands what goes on over here.
            
            // NOTE: Upon further inspection, I'm almost fully certain this should be a job instead of a
            // Coroutine.
            
            // TODO: Convert this coroutine to a Job

            // Loops eternally
            while (true)
            {
                // If the target isn't null
                if (IsTargetActive(target))
                {
                    // Update the combat state and position
                    inCombat = true;
                    targetPosition = target.transform.position;
                }
                else
                {
                    // Attempt to get new target, change combat state in response
                    target = GetTarget();
                    inCombat = IsTargetActive(target);
                }

                yield return null;
            }
        }

        /// <summary>
        /// Checks if a target exists, then check if its active
        /// </summary>
        /// <param name="targetToCheck">The target to check</param>
        /// <returns>Whether the target exists and is active</returns>
        private bool IsTargetActive(GameObject targetToCheck)
        {
            if (targetToCheck == null) return false;
            return targetToCheck.gameObject.activeSelf;
        }

        #endregion
    }
}