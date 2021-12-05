namespace SketchFleets
{
    /// <summary>
    /// A class for objects that should not be looped
    /// </summary>
    public interface INonLoopable
    {
        /// <summary>
        /// Prevents the object from being looped
        /// </summary>
        public void PreventLoop();
    }
}