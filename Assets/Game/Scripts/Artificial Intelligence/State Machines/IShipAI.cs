using SketchFleets.Data;

namespace SketchFleets.AI
{
    /// <summary>
    /// An interface that provides basic functionality for AI.
    /// </summary>
    public interface IShipAI
    {
        public ShipAttributes.Faction Faction { get; }
        
        /// <summary>
        /// Sets the ship's faction
        /// </summary>
        /// <param name="faction">The faction to set the ship to</param>
        public void SetFaction(ShipAttributes.Faction faction);
        
        /// <summary>
        /// Flips the ship's faction. Neutral ships cannot be flipped.
        /// </summary>
        public void FlipFaction();
    }
}