using System;
using System.Collections;
using System.Linq;
using SketchFleets.AI;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// An AI state that seeks an outlet and connects to it
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class SeekAndConnectState : BaseEnemyAIState
    {
        #region Private Fields

        private OutletState outlet;
        private Rigidbody2D rigidbody2d;
        
        private Coroutine findOutletRoutine;

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            outlet = null;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (outlet == null) return;
            if (other.gameObject != outlet.gameObject) return;
            AI.Ship.Die();
            outlet.Plug();
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

            if (AI == null)
            {
                Debug.LogError("SeekAndConnectState expects a EnemyShipAI State Machine!");
            }

            base.Enter();

            findOutletRoutine = StartCoroutine(FindOutlet());
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            if (!ShouldBeActive()) return;

            if (!IsThereAvailableOutlet())
            {
                AI.Ship.Look(AI.Target.transform.position);
                AI.Ship.Fire();
            }
            else
            {
                AI.Ship.Look(outlet.transform.position);
                rigidbody2d.AddForce(transform.up * (AI.Ship.Attributes.Speed * Time.deltaTime));
            }
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
        /// Checks whether there is an outlet to connect to
        /// </summary>
        /// <returns>Whether there is an outlet to connect to</returns>
        private bool IsThereAvailableOutlet()
        {
            return outlet != null && outlet.IsConnected == false && outlet.gameObject.activeSelf;
        }

        /// <summary>
        /// Finds an outlet to connect to
        /// </summary>
        private IEnumerator FindOutlet()
        {
            WaitForSeconds wait = new WaitForSeconds(3f);

            while (true)
            {
                if (IsThereAvailableOutlet())
                {
                    yield return wait;
                    continue;
                }

                // Memory leak risk here
                outlet = FindObjectsOfType<OutletState>(false).FirstOrDefault(foundOutlet => foundOutlet.IsConnected == false);
                
                yield return wait;
            }
        }

        #endregion
    }
}