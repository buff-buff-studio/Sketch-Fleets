namespace SketchFleets.SaveSystem
{
    /// <summary>
    /// Used to define byte edit mode for save objects
    /// </summary>
    public enum EditMode
    {
        /// <summary>Fixed mode. Bytes only will on call ToBytes. The entire object will be loaded instantly</summary>
        Fixed,
        /// <summary>Dynamic mode. Bytes will be stored on load and will change on every object change. You can load only the data you want to read/write</summary>
        Dynamic
    }
}