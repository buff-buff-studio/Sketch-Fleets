using ManyTools.Variables;

namespace SketchFleets
{
    /// <summary>
    /// Defines an object whose health can be verified
    /// </summary>
    public interface IHealthVerifiable
    {
        /// <summary>
        /// The object's max health
        /// </summary>
        public FloatReference MaxHealth { get; }
        
        /// <summary>
        /// The object's current health
        /// </summary>
        public FloatReference CurrentHealth { get; }
    }
}