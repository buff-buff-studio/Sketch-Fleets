namespace SketchFleets.SaveSystem
{
    /*
        Byte Save System

        Limitations:
            - Key are limited to 256 bytes (saves 3 bytes per key)
            - Max object size is 4194304KB / 4096 MB / 4 GB
        
        Features:
            - Supports
                Vector2
                Vector3
                Quaternion
                Color
                int
                float
                double
                bool
                string
                byte[]
                Serializable Objects

        Fixed Edit Mode:
            - Default edit mode
            - All object is loaded from start
            - The bytes will only be generated at the end
            - More Stable

        Dynamic Edit Mode:
            - Only needed parts of object is loaded
            - The bytes automatically changes when a value change
            - Less Stable (Need testing but seems 100% stable)
    */

    /// <summary>
    /// Core save class. Holds a seralizable save object
    /// </summary>
    public class Save : SaveObject, ISave, IEditModeProvider
    {
        #region Private Fields
        //Main byte holder
        private IByteBuffer buffer = new DefaultByteBuffer();
        //Data mode
        private EditMode mode;
        #endregion

        #region Constructors and Initiliazers
        /// <summary>
        /// Creates a new save object in Fixed edit mode
        /// </summary>
        public Save()
        {
            mode = EditMode.Fixed;
            Init(buffer,this);
        }

        /// <summary>
        /// Creates a new save object in Dynamic edit mode
        /// </summary>
        /// <returns></returns>
        public static Save NewDynamic()
        {
            return Save.FromBytes(new Save().ToBytes(),EditMode.Dynamic);
        }
        #endregion

        #region Interface Implementations
        /// <summary>
        /// Get current edit mode
        /// </summary>
        /// <returns></returns>
        public EditMode GetMode()
        {
            return mode;
        }
        #endregion 

        #region Serialization
        /// <summary>
        /// Parse save from bytes (Default Fixed edit mode)
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static Save FromBytes(byte[] bytes,EditMode mode = EditMode.Fixed)
        {
            Save save = new Save();
            save.buffer.AddBytes(bytes);

            int index = 1;
            //Load bytes
            save.Deserialize(bytes,ref index,mode == EditMode.Fixed);

            //Change mode
            save.mode = mode;
            return save;
        }

        /// <summary>
        /// Convert save object to bytes
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            if(GetMode() == EditMode.Fixed)
            {
                //Generate bytes before
                buffer.Clear();
                Serialize(buffer);
            }

            return buffer.GetBytes();
        }
        #endregion 

        #region Utils
        /// <summary>
        /// Get save object size in bytes
        /// </summary>
        /// <returns></returns>
        public int GetSizeInBytes()
        {
            if(GetMode() == EditMode.Dynamic)
                return buffer.GetBytesCount();
            throw new UnsupportedOperationException("GetByteSize is only applied for DYNAMIC saves");
        } 

        /// <summary>
        /// Convert current save to a hex string
        /// </summary>
        /// <returns></returns>
        public string ToHexString()
        {
            return System.BitConverter.ToString(ToBytes());
        }
        #endregion
    }  
}
