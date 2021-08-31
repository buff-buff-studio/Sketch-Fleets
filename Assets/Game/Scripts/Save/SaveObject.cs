using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace SketchFleets.SaveSystem
{
    /// <summary>
    /// Holds a list of pair pointers (Similiar to a dictionary)
    /// </summary>
    public class SaveObject : ISaveObject, IEnumerable<PairPointer>
    {
        #region Private Fields
        //Pointers
        private List<PairPointer> pointers = new List<PairPointer>();

        //Bytes (Only in Dynamic mode)
        private int bytePos = 0;
        private int byteCount = 0;

        //Providers
        private IEditModeProvider mode;
        private IByteBuffer bytes;
        private ISaveObject parent;
        #endregion

        #region Properties
        /// <summary>
        /// Get current pairs Count
        /// </summary>
        /// <value>int</value>
        public int Count
        {
            get {
                return pointers.Count;
            }

            set {
                
            }
        }
        #endregion

        #region [] Operator
        /// <summary>
        /// Get a value
        /// </summary>
        /// <value>object</value>
        public object this[string key]
        {
            get => Get(key);
            set => Set(key, value);
        }
        #endregion

        #region Init
        public void Init(IByteBuffer provider,IEditModeProvider mode)
        {
            this.bytes = provider;
            this.mode = mode;
        }
        #endregion

        #region Object Actions
        /// <summary>
        /// Check if has a pair with a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasKey(string key)
        {
            foreach(PairPointer p in pointers)
            {
                if(p.Key == key)
                    return true;
            }

            return false;
        }
        
        /// <summary>
        /// Get value from key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            PairPointer pointer = GetPointer(key);

            if(key == null)
                return null;
            if(pointer == null)
                return null;

            return pointer.Value;
        }

        /// <summary>
        /// Get parsed value from key
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            PairPointer pointer = GetPointer(key);

            if(key == null)
                return default(T);
            if(pointer == null)
                return default(T);

            return (T) pointer.Value;
        }

        #region Set Providers
        /// <summary>
        /// Used to set an SaveObject as child in a Dynamic context
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private PairPointer SetChildObjectDynamic(string key,SaveObject value)
        {
            if(mode != null)
                if(mode.GetMode() != EditMode.Dynamic)
                    throw new UnsupportedOperationException("Operation SetChildObjectDynamic can only be done in a Dynamic object");

            SaveObject temp = new SaveObject();

            //Get value holder
            PairPointer pt = GetPointer(key);
            if(pt != null)
            {
                //Remove current and create new pointer
                Remove(key);
            }

            //Add to the end of object
            pt = new PairPointer(key,bytes,mode,this);

            foreach(PairPointer p in value)
                temp.Set(p.Key,p.Value);
            value.Clear();

            //Change value
            pt.Value = value;
    
            pt.AddBytes(bytePos + byteCount + 4);
            value.SetByteInfo(bytePos + byteCount,0);   
            

            //Add pointer to list
            pointers.Add(pt);

            foreach(PairPointer p in temp)
                value.Set(p.Key,p.Value);

            return pt;
        }
        
        /// <summary>
        /// Used to set an SaveListObject as child in a Dynamic context
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private PairPointer SetChildListDynamic(string key,SaveListObject value)
        {
            if(mode.GetMode() != EditMode.Dynamic)
                throw new UnsupportedOperationException("Operation SetChildListDynamic can only be done in a Dynamic object");

            SaveListObject temp = new SaveListObject();

            //Get value holder
            PairPointer pt = GetPointer(key);
            if(pt != null)
            {
                //Remove current and create new pointer
                Remove(key);
            }

            //Add to the end of object
            pt = new PairPointer(key,bytes,mode,this);

            foreach(Pointer p in value.GetPointers())
                temp.Add(p.Value);
            value.Clear();

            //Change value
            pt.Value = value;
    
            pt.AddBytes(bytePos + byteCount + 4);
            value.SetByteInfo(bytePos + byteCount,0);    

            //Add pointer to list
            pointers.Add(pt);

            foreach(Pointer p in temp.GetPointers())
                value.Add(p.Value);

            return pt;
        }
        #endregion

        /// <summary>
        /// Set a pair with key and value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public PairPointer Set(string key,object value)
        {   
            if(mode.GetMode() == EditMode.Dynamic)
                if(value != null)
                    if(value is SaveListObject)
                    {
                        return SetChildListDynamic(key,(SaveListObject) value);
                        
                    }
                    else if(value is SaveObject)
                    {
                        return SetChildObjectDynamic(key,(SaveObject) value);
                    }

            //Get value holder
            PairPointer pt = GetPointer(key);
            if(pt == null)
            {
                //Add to the end of object
                pt = new PairPointer(key,bytes,mode,this);

                //Change value
                pt.Value = value;
        
                if(mode != null && mode.GetMode() == EditMode.Dynamic)
                {
                    pt.AddBytes(bytePos + byteCount + 4);
                }

                //Add pointer to list
                pointers.Add(pt);
                return pt;
            } 
            
            //Change value
            pt.Value = value;

            //Get pointer
            return pt;
        }

        /// <summary>
        /// Remove a pair with a key
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            int i = 0;
            foreach(PairPointer p in pointers)
            {
                if(p.Key == key)
                {
                    pointers.RemoveAt(i);

                    if(mode.GetMode() == EditMode.Dynamic)
                    {
                        p.RemoveBytes();
                    }
                    return;
                }
                i ++;
            }
        }

        /// <summary>
        /// Clear all pairs
        /// </summary>
        public void Clear()
        {   
            pointers.Clear();

            if(mode != null)
                if(mode.GetMode() == EditMode.Dynamic)
                {
                    bytes.RemoveBytes(bytePos,byteCount + 4);
                    bytes.InsertBytes(bytePos,new byte[]{0,0,0,0});
                }

            if(parent != null)
                parent.ChangeByteSizeHeader(-byteCount);

            byteCount = 0;
        }
        #endregion
        
        #region Enumerator
        /// <summary>
        /// Get pairs enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<PairPointer> GetEnumerator()
        {
            foreach(PairPointer p in pointers)
            {
                yield return p;
            }
        }

        /// <summary>
        /// Get pairs enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Save Object
        public IEnumerable<Pointer> GetPointers()
        {
            return pointers;
        }

        public ISaveObject GetParent()
        {
            return parent;
        }

        public void SetParent(ISaveObject parent)
        {
            this.parent = parent;
        }
        #endregion

        #region Child Instantiation
        /// <summary>
        /// Creates a child object for an object (Use in Dynamic mode to prevent errors)
        /// </summary>
        /// <returns></returns>
        public SaveObject CreateChild()
        {
            if(mode.GetMode() == EditMode.Dynamic)
                throw new UnsupportedOperationException("Operation CreateChild can only be done in a Fixed object");


            SaveObject obj = new SaveObject();
            obj.Init(this.bytes,mode);
            obj.SetParent(this);
            return obj;
        }

        /// <summary>
        /// Creates a child list for an object (Use in Dynamic mode to prevent errors)
        /// </summary>
        /// <returns></returns>
        public SaveListObject CreateChildList()
        {
            if(mode.GetMode() == EditMode.Dynamic)
                throw new UnsupportedOperationException("Operation CreateChildList can only be done in a Fixed object");

            SaveListObject obj = new SaveListObject();
            obj.Init(this.bytes,mode);
            obj.SetParent(this);
            return obj;
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Serialize pointers to byte buffer
        /// </summary>
        /// <param name="provider"></param>
        public void Serialize(IByteBuffer buffer)
        {
            //Add object type marker
            buffer.AddBytes(255);         

            //Get bytes count
            int pre = buffer.GetBytesCount();

            //Add objects
            foreach(PairPointer p in pointers.ToArray())
                if(p != null)
                    p.Serialize(buffer);

            //Get end count
            int end = buffer.GetBytesCount();

            //Add object header length
            buffer.InsertBytes(pre,BitConverter.GetBytes(end - pre));
        }

        /// <summary>
        /// Converts byte to pairs
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="loadChilds"></param>
        public void Deserialize(byte[] bytes,ref int index,bool loadChilds)
        {
            //Get byte position and count
            bytePos = index;
            byteCount = BitConverter.ToInt32(bytes,index);
            index += 4;
            
            int end = index + byteCount;
            while(index < end)
            {     
                //Get key length
                int keyLength = bytes[index];
                index ++;

                //Get key
                string key = Encoding.ASCII.GetString(bytes,index,keyLength);
                index += keyLength;

                //Get type
                byte type = bytes[index];
                int start = index;
                index ++;

                //Get value
                int size = index;
                object value = Util.Deserialize(this,this.bytes,this.mode,type,bytes,ref index,loadChilds,false);
                size = index - size;

                //Get value holder
                PairPointer p = GetPointer(key);
                if(p == null)
                {
                    //Add to the end of object
                    p = new PairPointer(key,this.bytes,mode,this);

                    pointers.Add(p);
                } 

                //Change value
                p.Value = value;

                //Set position and length
                p.SetByteInfo(start,size);

                //Mark as unloaded object
                if(value == null && !loadChilds && type != 220)
                    p.MarkUnloaded();
            } 
        }
        #endregion

        #region Util
        /// <summary>
        /// Get pointer from pair key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public PairPointer GetPointer(string key)
        {
            foreach(PairPointer p in pointers)
            {
                if(p.Key == key)
                    return p;
            }

            return null;
        }

        public void ChangeByteSizeHeader(int change)
        {
            bytes.RemoveBytes(bytePos,4);
            byteCount += change;
            bytes.InsertBytes(bytePos,BitConverter.GetBytes(byteCount));
            
            if(GetParent() != null)
            {
                GetParent().ChangeByteSizeHeader(change);
            }
        }

        public void SetByteInfo(int position,int length)
        {
            bytePos = position;
            byteCount = length;
        }

        public override string ToString()
        {
            string s = "";

            foreach(PairPointer p in pointers)
            {
                s += ",\"" + p.Key + "\": " + p.ValueToString();    
            }

            return "{" + (s.Length == 0 ? "" : s.Substring(1)) + "}";
        }
        #endregion
    }    
}