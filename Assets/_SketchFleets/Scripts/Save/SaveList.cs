namespace SketchFleets.SaveSystem
{
    /// <summary>
    /// List save class. Holds a seralizable save object list
    /// </summary>
    public class SaveList : SaveListObject, ISave, IEditModeProvider
    {
        #region Private Fields
        //Main byte holder
        private IByteBuffer buffer = new DefaultByteBuffer();
        //Data mode
        private EditMode mode;
        #endregion

        #region Constructors and Initiliazers
        /// <summary>
        /// Creates a new save list in Fixed edit mode
        /// </summary>
        public SaveList()
        {
            mode = EditMode.Fixed;
            Init(buffer,this);
        }

        /// <summary>
        /// Creates a new save list in Dynamic edit mode
        /// </summary>
        /// <returns></returns>
        public static SaveList NewDynamic()
        {
            return SaveList.FromBytes(new SaveList().ToBytes(),EditMode.Dynamic);
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
        /// Parse save list from bytes (Default Fixed edit mode)
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static SaveList FromBytes(byte[] bytes,EditMode mode = EditMode.Fixed)
        {
            SaveList save = new SaveList();
            save.buffer.AddBytes(bytes);

            int index = 1;
            //Load bytes
            save.Deserialize(bytes,ref index,mode == EditMode.Fixed);

            //Change mode
            save.mode = mode;
            return save;
        }

        /// <summary>
        /// Convert save list object to bytes
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
        /// Get save list object size in bytes
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
