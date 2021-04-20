using System.Collections.Generic;

namespace SketchFleets.SaveSystem
{
    /// <summary>
    /// Defaut byte holder implementation, using List<byte> to hold bytes
    /// </summary>
    public class DefaultByteBuffer : IByteBuffer
    {
        #region Public Fields
        public List<byte> bytes = new List<byte>();
        #endregion

        #region Interface Implementations
        public byte[] GetBytes()
        {
            return bytes.ToArray();
        }

        public List<byte> GetByteList()
        {
            return bytes;
        }

        public void InsertBytes(int index,IEnumerable<byte> bytes)
        {
            this.bytes.InsertRange(index,bytes);
        }

        public void RemoveBytes(int index,int count)
        {
            this.bytes.RemoveRange(index,count);
        }

        public void AddBytes(params byte[] bytes)
        {
            this.bytes.AddRange(bytes);
        }

        public int GetBytesCount()
        {
            return this.bytes.Count;
        }

        public void Clear()
        {
            this.bytes.Clear();
        }
        #endregion
    }
}