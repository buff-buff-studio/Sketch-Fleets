
namespace SketchFleets.SaveSystem
{
    /// <summary>
    /// Get save edit mode
    /// </summary>
    public interface IEditModeProvider
    {
        /// <summary>
        /// Get edit mode from object
        /// </summary>
        /// <returns></returns>
        EditMode GetMode();
    }
}