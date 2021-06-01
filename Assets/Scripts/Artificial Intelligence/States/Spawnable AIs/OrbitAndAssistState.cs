using ManyTools.UnityExtended;
using SketchFleets.AI;
using UnityEngine;

namespace SketchFleets
{
    /// <summary>
    /// An AI state that orbits around the player and fires when he does so
    /// </summary>
    public class OrbitAndAssistState : BaseOrbitState
    {
        #region State Implementation

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            base.StateUpdate();

            if (Input.GetKey(KeyCode.Mouse0))
            {
                AI.Ship.Fire();
            }
        }

        #endregion
    }
}