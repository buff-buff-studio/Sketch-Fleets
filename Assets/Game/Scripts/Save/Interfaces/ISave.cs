namespace SketchFleets.SaveSystem
{
    /// <summary>
    /// Main save objects base
    /// </summary>
    public interface ISave
    {
        /// <summary>
        /// Converts current object to byte (or just retrieve bytes from a <c>Dynamic</c> object) 
        /// </summary>
        /// <returns></returns>
        byte[] ToBytes();

        /// <summary>
        /// Return the size of a <c>Dynamic</c> object in bytes (Dynamic Only)
        /// </summary>
        /// <returns>Byte Size</returns>
        int GetSizeInBytes();
        
        /// <summary>
        /// Converts current byte value to hexadecimal string
        /// </summary>
        /// <returns></returns>
        string ToHexString();
    }
}