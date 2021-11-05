namespace SketchFleets.AI
{
    /// <summary>
    /// An AI state that aims and fires
    /// </summary>
    public sealed class AimAndFireState : BaseEnemyAIState
    {
        #region State Implementation

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            if (!ShouldBeActive()) return;
            
            AI.Ship.Look(AI.Target.transform.position);
            AI.Ship.Fire();
        }

        #endregion
    }
}