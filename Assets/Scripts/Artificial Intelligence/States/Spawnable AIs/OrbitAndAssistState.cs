using ManyTools.UnityExtended;
using SketchFleets.AI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SketchFleets
{
    /// <summary>
    /// An AI state that orbits around the player and fires when he does so
    /// </summary>
    public class OrbitAndAssistState : BaseOrbitState
    {
        #region Import Control

        private PlayerControl playerControl;

        private void Awake()
        {
            playerControl = new PlayerControl();
            playerControl.Enable();
        }

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            base.StateUpdate();

            playerControl.Player.Shoot.performed += FireCall;
        }

        void FireCall(InputAction.CallbackContext context)
        {
            AI.Ship.Fire();
        }

        #endregion
    }
}