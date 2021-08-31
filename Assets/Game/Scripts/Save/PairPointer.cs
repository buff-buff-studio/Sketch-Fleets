using System.Text;

namespace SketchFleets.SaveSystem
{
    /// <summary>
    /// Pair pointer class. Holds an object key and value
    /// </summary>
    public class PairPointer : Pointer
    {
        #region Private Fields
        private string key;
        #endregion

        #region Properties
        /// <summary>
        /// Pointer key (Constant)
        /// </summary>
        /// <value>object</value>
        public string Key
        {
            get
            {
                return key;
            }

            set
            {

            }
        }
        #endregion
        
        #region Constructor
        /// <summary>
        /// Creates a new PairPointer (Internal Only)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bytes"></param>
        /// <param name="mode"></param>
        /// <param name="saveObject"></param>
        /// <returns></returns>
        public PairPointer(string key,IByteBuffer bytes,IEditModeProvider mode,ISaveObject saveObject) : base(bytes,mode,saveObject)
        {
            this.key = key;
        }
        #endregion

        #region Serializable
        /// <summary>
        /// Serialize current key and value to a byte buffer
        /// </summary>
        /// <param name="buffer"></param>
        public new void Serialize(IByteBuffer buffer)
        {
            //Serialize key
            byte[] bt = Encoding.ASCII.GetBytes(Key);
            int length = bt.Length;
            //Add key bytes
            buffer.AddBytes((byte) length);
            buffer.AddBytes(bt);

            //Serialize and add value
            base.Serialize(buffer);
        }
        
        /// <summary>
        /// Add key and value bytes to buffer
        /// </summary>
        /// <param name="index"></param>
        public new void AddBytes(int index)
        {
            //Update position
            bytePosition = index;

            //Create temporary holder
            DefaultByteBuffer buffer = new DefaultByteBuffer();

            //Serialize key
            byte[] bt = Encoding.ASCII.GetBytes(Key);
            int length = bt.Length;

            //Add key
            buffer.AddBytes((byte) length);
            buffer.AddBytes(bt);

            //Update position
            bytePosition += Util.KeyLength + length;

            DefaultByteBuffer def = new DefaultByteBuffer();
            base.Serialize(def);
            
            //Serialize value
            base.Serialize(buffer);

            //Add
            byteBuffer.InsertBytes(index,buffer.bytes);

            //Update size
            byteLength = buffer.bytes.Count;

            //Difference
            int dif = byteLength; 

            //Remove keys and type
            byteLength -= (Util.KeyLength + length) + 1;

            //Update size
            saveObject.ChangeByteSizeHeader(dif);

            //Get root
            ISaveObject parent = (ISaveObject) modeProvider;

            //Update positions from root
            foreach(Pointer p in parent.GetPointers())
            {
                p.UpdatePosition(index,dif);
            }
        }

        /// <summary>
        /// Remove key and value bytes from buffer
        /// </summary>
        public new void RemoveBytes()
        {
            //Calculate position and length to remove
            int pos = bytePosition - Util.KeyLength  - Key.Length;
            int len = byteLength + 1 + Util.KeyLength  + Key.Length;

            //Remove all bytes
            byteBuffer.RemoveBytes(pos,len);

            //Difference
            int dif = -(len); 

            //Update size
            saveObject.ChangeByteSizeHeader(dif);

            //Get root
            ISaveObject parent = (ISaveObject) modeProvider;

            //Update positions from root
            foreach(Pointer p in parent.GetPointers())
            {
                p.UpdatePosition(bytePosition,dif);
            }
        }
        #endregion

        #region Utils
        public override string ToString()
        {
            return Key + ":" + ValueToString();
        }
        #endregion
    }
}