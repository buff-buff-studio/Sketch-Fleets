using System.Collections.Generic;

namespace SketchFleets.SaveSystem
{
    /// <summary>
    /// Used to write/read bytes for serialization/deserialization process
    /// </summary>
    public interface IByteBuffer
    {
        /// <summary>
        /// Return stored byte array
        /// </summary>
        /// <returns></returns>
        byte[] GetBytes();
        
        /// <summary>
        /// Insert one or more bytes into byte buffer
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bytes"></param>
        void InsertBytes(int index,IEnumerable<byte> bytes);

        /// <summary>
        /// Remove one or more bytes from byte buffer
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        void RemoveBytes(int index,int count);

        /// <summary>
        /// Add bytes at the end of byte buffer
        /// </summary>
        /// <param name="bytes"></param>
        void AddBytes(params byte[] bytes);

        /// <summary>
        /// Get count of byte buffer bytes
        /// </summary>
        /// <returns></returns>
        int GetBytesCount();

        /// <summary>
        /// Clear byte buffer
        /// </summary>
        void Clear();
    }
}