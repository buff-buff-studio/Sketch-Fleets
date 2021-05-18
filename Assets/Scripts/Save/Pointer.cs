namespace SketchFleets.SaveSystem
{
    /// <summary>
    /// Default pointer class. Holds an object value
    /// </summary>
    public class Pointer
    {
        #region Private Fields
        //Components
        private bool loaded = false;
        private object value;
        #endregion

        #region Protected Fields
        //Bytes (Only in Dynamic mode)
        protected int bytePosition = 0;
        protected int byteLength = 0;
        //Providers
        protected IByteBuffer byteBuffer;
        protected IEditModeProvider modeProvider;
        //Save object root
        protected ISaveObject saveObject;
        #endregion
        
        #region Properties
        
        /// <summary>
        /// Pointer value
        /// </summary>
        /// <value>object</value>
        public object Value
        {
            get
            {
                if(!loaded)
                {
                    loaded = true;   
                    //Try to load
                    byte[] bt = byteBuffer.GetBytes();
                    int index = bytePosition + 1;
                    this.value = Util.Deserialize(saveObject,byteBuffer,modeProvider,bt[bytePosition],bt,ref index,false,true);
                }

                return this.value;
            }

            set
            {
                if(!Util.IsObjectSupported(value))
                    throw new UnsupportedObjectException("The object " + value.ToString() + " is not supported by save system");
                
                this.value = value;

                if(this.value is ISaveObject)
                {
                    //if(((ISaveObject) this.value).GetParent() == null && modeProvider != null && modeProvider.GetMode() == EditMode.Dynamic)
                    //    throw new UnsupportedObjectException("You can't use new SaveObject or new SaveListObject in Dynamic saves. Use CreateChild or CreateChildList instead");
                    
                    ((ISaveObject) this.value).Init(byteBuffer,modeProvider);
                    ((ISaveObject) this.value).SetParent(saveObject);
                }

                if(modeProvider != null && modeProvider.GetMode() == EditMode.Dynamic && loaded)
                {   
                    DefaultByteBuffer holder = new DefaultByteBuffer();
                    Serialize(holder);

                    int dif = byteLength;

                    //Replace
                    byteBuffer.RemoveBytes(bytePosition,byteLength + 1);
                    byteBuffer.InsertBytes(bytePosition,holder.bytes);
                    byteLength = holder.bytes.Count - 1;

                    //Difference
                    dif = byteLength - dif; 

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


                loaded = true;        
            }
        }
        #endregion

        #region Constructors and Initializers
        /// <summary>
        /// Creates a new pointer (Internal Only)
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="mode"></param>
        /// <param name="saveObject"></param>
        public Pointer(IByteBuffer bytes,IEditModeProvider mode,ISaveObject saveObject)
        {
            this.byteBuffer = bytes;
            this.modeProvider = mode;
            this.saveObject = saveObject;
        }
        #endregion
    
        #region Serialization
        /// <summary>
        /// Serializes value to a byte buffer
        /// </summary>
        /// <param name="provider"></param>
        public void Serialize(IByteBuffer buffer)
        {
            Util.Serialize(buffer,Value);
        }

        /// <summary>
        /// Add current value bytes to buffer
        /// </summary>
        /// <param name="index"></param>
        public void AddBytes(int index)
        {
            bytePosition = index;
            
            DefaultByteBuffer holder = new DefaultByteBuffer();
            Serialize(holder);

            //Replace
            byteBuffer.InsertBytes(bytePosition,holder.bytes);
            byteLength = holder.bytes.Count;

            //Difference
            int dif = byteLength; 

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

        /// <summary>
        /// Remove current value bytes from buffer
        /// </summary>
        public void RemoveBytes()
        {
            byteBuffer.RemoveBytes(bytePosition,byteLength);

            //Difference
            int dif = -(byteLength); 

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

        #region Byte Info
        /// <summary>
        /// Update position of sibblings and parents
        /// </summary>
        /// <param name="current"></param>
        /// <param name="change"></param>
        public void UpdatePosition(int current,int change)
        {
            if(bytePosition >= current)
                bytePosition += change;  

            if(this.value != null)
                if(this.value is ISaveObject)
                {
                    foreach(Pointer p in ((ISaveObject) this.value).GetPointers())
                        p.UpdatePosition(current,change);
                }      
        }
        
        /// <summary>
        /// Change current Dynamic byte information (Dynamic Only)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="length"></param>
        public void SetByteInfo(int position,int length)
        {
            bytePosition = position;
            byteLength = length;
        }

        /// <summary>
        /// Get length of value in bytes
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            return byteLength;
        }
        #endregion

        #region Utils
        /// <summary>
        /// Check if pointer value is loaded
        /// </summary>
        /// <returns></returns>
        public bool IsLoaded()
        {
            return loaded;
        }

        /// <summary>
        /// Mark pointer value as unloaded
        /// </summary>
        public void MarkUnloaded()
        {
            loaded = false;
        }

        /// <summary>
        /// Get end position in byte array (Dynamic Only)
        /// </summary>
        /// <returns></returns>    
        public int GetEnd()
        {
            return bytePosition + byteLength;
        }

        /// <summary>
        /// Only value to string
        /// </summary>
        /// <returns></returns>
        public string ValueToString()
        {
            if(!IsLoaded())
                return "<...>";
            else if(Value == null)
                return "<null>";
            else
                return Value.ToString();
        }

        /// <summary>
        /// Override to string implementation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ValueToString();
        } 
        #endregion
    }
}