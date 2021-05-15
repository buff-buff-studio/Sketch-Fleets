namespace SketchFleets
{
    /// <summary>
    /// A class that controls a collectible entity
    /// </summary>
    public interface ICollectible
    {
        /// <summary>
        /// Applies all necessary effects upon collection
        /// </summary>
        public void Collect();
    }
}