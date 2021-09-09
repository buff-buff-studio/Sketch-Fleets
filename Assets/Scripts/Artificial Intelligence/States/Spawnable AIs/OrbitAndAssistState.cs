using System.Collections;
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
        #region State Implementation

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            base.StateUpdate();
        }

        public IEnumerator MobileFire()
        {
            while (true)
            {
                AI.Ship.Fire();
                yield return null;
            }
        }

        #endregion
    }
}