namespace SketchFleets.AI
{
    /// <summary>
    /// An AI state that aims and fires
    /// </summary>
    public class AimAndFireState : BaseEnemyAIState
    {
        #region State Implementation

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            if (!shipRenderer.isVisible) return;
            
            AI.Ship.Look(AI.Player.transform.position);
            AI.Ship.Fire();
        }

        #endregion
    }
}