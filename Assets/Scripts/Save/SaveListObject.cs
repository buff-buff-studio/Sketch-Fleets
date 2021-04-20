using System;
using System.Collections;
using System.Collections.Generic;

namespace SketchFleets.SaveSystem
{
    public class SaveListObject : ISaveObject, IEnumerable<object>
    {   
        #region Private Fields
        //Pointers
        private List<Pointer> pointers = new List<Pointer>();
        
        //Bytes (Only in Dynamic mode)
        private int bytePos = 0;
        private int byteCount = 0;

        //Providers
        private IEditModeProvider mode;
        private IByteBuffer bytes;
        private ISaveObject parent;
        #endregion

        #region [] Operator
        public object this[int key]
        {
            get => Get(key);
            set => Set(key, value);
        }
        #endregion

        #region Properties
        //List size
        public int Count
        {
            get {
                return pointers.Count;
            }

            set {
                
            }
        }
        #endregion

        #region Init
        public void Init(IByteBuffer provider,IEditModeProvider mode)
        {
            this.bytes = provider;
            this.mode = mode;
        }
        #endregion

        #region List Actions
        public object Get(int index)
        {
            if(index < 0 || index >= pointers.Count)
                throw new IndexOutOfRangeException("The index " + index + " is out of range!");

            return pointers[index].Value;
        }

        public T Get<T>(int index)
        {
            if(index < 0 || index >= pointers.Count)
                throw new IndexOutOfRangeException("The index " + index + " is out of range!");

            return (T) pointers[index].Value;
        }

        public void Set(int index,object value)
        {
            if(value != null)
                if(value is SaveListObject)
                {
                    SetChildListDynamic(index,(SaveListObject) value);
                    return;
                }
                else if(value is SaveObject)
                {
                    SetChildObjectDynamic(index,(SaveObject) value);
                    return;
                }

            Remove(index);
            Insert(index,value);
        }

        #region Set Providers
        /// <summary>
        /// Used to set an SaveObject as child in a Dynamic context
        /// </summary>
        /// <param name="index"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Pointer SetChildObjectDynamic(int index,SaveObject obj)
        {
            if(mode.GetMode() != EditMode.Dynamic)
                throw new UnsupportedOperationException("Operation SetChildObjectDynamic can only be done in a Dynamic object");

            SaveObject temp = new SaveObject();
     
            if(index < pointers.Count)
                Remove(index);
            
            if(index < 0 || index > pointers.Count)
                throw new IndexOutOfRangeException("The index " + index + " is out of range!");
            
            int position = index == pointers.Count ? bytePos + byteCount + 4 : (index == 0 ? bytePos + 4 : pointers[index - 1].GetEnd());

            //Add to the end of object
            Pointer pt = new Pointer(bytes,mode,this);

            foreach(PairPointer p in obj)
                temp.Set(p.Key,p.Value);
            obj.Clear();

            //Change value
            pt.Value = obj;

            pt.AddBytes(position);
            obj.SetByteInfo(position + 1,0); 

            pointers.Insert(index,pt);

            foreach(PairPointer p in temp)
                obj.Set(p.Key,p.Value);

            return pt;
        }

        /// <summary>
        /// Used to set an SaveObject as child in a Dynamic context
        /// </summary>
        /// <param name="index"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Pointer SetChildListDynamic(int index,SaveListObject obj)
        {
            if(mode.GetMode() != EditMode.Dynamic)
                throw new UnsupportedOperationException("Operation SetChildListDynamic can only be done in a Dynamic object");

            SaveListObject temp = new SaveListObject();
     
            if(index < pointers.Count)
                Remove(index);
            
            if(index < 0 || index > pointers.Count)
                throw new IndexOutOfRangeException("The index " + index + " is out of range!");
            
            int position = index == pointers.Count ? bytePos + byteCount + 4 : (index == 0 ? bytePos + 4 : pointers[index - 1].GetEnd());

            //Add to the end of object
            Pointer pt = new Pointer(bytes,mode,this);

            foreach(Pointer p in obj.GetPointers())
                temp.Add(p.Value);

            obj.Clear();

            //Change value
            pt.Value = obj;

            pt.AddBytes(position);
            obj.SetByteInfo(position + 1,0);    

            pointers.Insert(index,pt);

            foreach(Pointer p in temp.GetPointers())
                obj.Add(p.Value);

            return pt;
        }
        #endregion

        public Pointer Add(object value)
        {       
            if(value != null)
                if(value is SaveListObject)
                    return SetChildListDynamic(Count,(SaveListObject) value);
                else if(value is SaveObject)
                    return SetChildObjectDynamic(Count,(SaveObject) value);

            return Insert(pointers.Count,value);
        }

        public Pointer Insert(int index,object value)
        {       
            if(index < 0 || index > pointers.Count)
                throw new IndexOutOfRangeException("The index " + index + " is out of range!");
            
            int position = index == pointers.Count ? bytePos + byteCount + 4 : (index == 0 ? bytePos + 4 : pointers[index - 1].GetEnd());

            //Add to the end of object
            Pointer pt = new Pointer(bytes,mode,this);

            //Change value
            pt.Value = value;

            if(mode != null)
                if(mode.GetMode() == EditMode.Dynamic)
                {
                    pt.AddBytes(position);
                }

            pointers.Insert(index,pt);
            return pt;
        }

        public void Remove(int index)
        {
            if(index < 0 || index >= pointers.Count)
                throw new IndexOutOfRangeException("The index " + index + " is out of range!");

            Pointer p = pointers[index];
            
            pointers.RemoveAt(index);

            if(mode.GetMode() == EditMode.Dynamic)
            {
                p.RemoveBytes();
            }
        }

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
        public IEnumerator<object> GetEnumerator()
        {
            foreach(Pointer p in pointers)
            {
                yield return p.Value;
            }
        }

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
            buffer.AddBytes(254);         

            int pre = buffer.GetBytesCount();

            //Add objects
            foreach(Pointer p in pointers)
                p.Serialize(buffer);

            int end = buffer.GetBytesCount();
            buffer.InsertBytes(pre,BitConverter.GetBytes(end - pre));
        }

        /// <summary>
        /// Converts byte to pointers
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="loadChilds"></param>
        public void Deserialize(byte[] bytes,ref int index,bool loadChilds)
        {
            bytePos = index;
            byteCount = BitConverter.ToInt32(bytes,index);
            index += 4;
            
            int end = index + byteCount;

            int i = 0;
            while(index < end)
            {     
                byte type = bytes[index];
                int start = index;
                index ++;

                //Get value
                int size = index;
                object value = Util.Deserialize(this,this.bytes,this.mode,type,bytes,ref index,loadChilds,false);
                size = index - size;

                //Get value holder
                Pointer p = new Pointer(this.bytes,mode,this);
                pointers.Add(p);

                //Change value
                p.Value = value;

                p.SetByteInfo(start,size);

                //Mark as unloaded object
                if(value == null && !loadChilds && type != 220)
                    p.MarkUnloaded();

                i ++;
            } 
        }
        #endregion
   
        #region Util
        /// <summary>
        /// Get a pointer at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Pointer GetPointer(int index)
        {
            if(index < 0 || index >= pointers.Count)
                throw new IndexOutOfRangeException("The index " + index + " is out of range!");

            return pointers[index];
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

            foreach(Pointer p in pointers)
            {
                s += "," + p.ToString();    
            }

            return "[" + (s.Length == 0 ? "" : s.Substring(1)) + "]";
        }
        #endregion
    }    
}