using SketchFleets.AI;

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
            AI.Ship.Fire();
        }

        #endregion
    }
}