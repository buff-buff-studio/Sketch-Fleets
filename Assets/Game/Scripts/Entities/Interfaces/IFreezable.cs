namespace SketchFleets
{
    /// <summary>
    /// An interface that describes a freezable object
    /// </summary>
    public interface IFreezable
    {
        /// <summary>
        /// Freezes the object for the specified duration
        /// </summary>
        /// <param name="duration">The duration of the frost</param>
        public void Freeze(float duration);
    }
}