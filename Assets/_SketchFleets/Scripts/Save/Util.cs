using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SketchFleets.SaveSystem
{
    /// <summary>
    /// Main save system util class
    /// </summary>
    public class Util
    {   
        #region Settings
        public const int KeyLength = 1;
        #endregion

        #region Util Methods
        /// <summary>
        /// Check if object is supported by save system
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsObjectSupported(object value)
        {
            if(value == null)
                return true;

            if(value is SaveObject || value is SaveListObject)
                return true;

            if(value is int || value is float || value is double || value is string || value is bool)
                return true;

            if(value is Vector2 || value is Vector3 || value is Color || value is Quaternion)
                return true;

            if(((System.Object) value).GetType().IsSerializable)
                return true;

            if(value is byte[])
                return true;

            return false;
        }

        /// <summary>
        /// Just skip an object (Used in Dynamic loadding mode)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bytes"></param>
        public static void Skip(ref int index,byte[] bytes)
        {
            index += 4 + GetByteLength(bytes,index);
        }

        /// <summary>
        /// Get byte length from header
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int GetByteLength(byte[] bytes,int index)
        {
            return BitConverter.ToInt32(bytes,index);;   
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Serialize object into byte buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        public static void Serialize(IByteBuffer buffer,object value)
        {
            //Null
            if(value == null)
            {
                buffer.AddBytes(220);
                return;
            }

            #region Internal
            if(value is SaveObject)
            {
                ((SaveObject) value).Serialize(buffer);
                return;
            }
            if(value is SaveListObject)
            {
                ((SaveListObject) value).Serialize(buffer);
                return;
            }
            #endregion

            #region Primitives
            if(value is int)
            {
                //Add int type
                buffer.AddBytes(1);
                buffer.AddBytes(BitConverter.GetBytes((int) value));
                return;
            }
            if(value is string)
            {
                //Add string type
                buffer.AddBytes(2);
                //Add bytes length
                byte[] bt = Encoding.ASCII.GetBytes((string) value);
                int length = bt.Length;
                buffer.AddBytes(BitConverter.GetBytes(length));
                buffer.AddBytes(bt);
                return;
            }
            if(value is float)
            {
                //Add float type
                buffer.AddBytes(3);
                buffer.AddBytes(BitConverter.GetBytes((float) value));
                return;
            }
            if(value is double)
            {
                //Add double type
                buffer.AddBytes(4);
                buffer.AddBytes(BitConverter.GetBytes((double) value));
                return;
            }
            if(value is bool)
            {
                //Add double type
                buffer.AddBytes(((bool)value) ? (byte)5 : (byte)6);
                return;
            }
            #endregion

            #region Unity Objects
            if(value is Color)
            {
                Color o = (Color) value;
                //Add double type
                buffer.AddBytes(100);
                buffer.AddBytes(BitConverter.GetBytes(o.r));
                buffer.AddBytes(BitConverter.GetBytes(o.g));
                buffer.AddBytes(BitConverter.GetBytes(o.b));
                buffer.AddBytes(BitConverter.GetBytes(o.a));
                return;
            }

            if(value is Vector2)
            {
                Vector2 o = (Vector2) value;
                //Add double type
                buffer.AddBytes(101);
                buffer.AddBytes(BitConverter.GetBytes(o.x));
                buffer.AddBytes(BitConverter.GetBytes(o.y));
                return;
            }
            if(value is Vector3)
            {
                Vector3 o = (Vector3) value;
                //Add double type
                buffer.AddBytes(102);
                buffer.AddBytes(BitConverter.GetBytes(o.x));
                buffer.AddBytes(BitConverter.GetBytes(o.y));
                buffer.AddBytes(BitConverter.GetBytes(o.z));
                return;
            }
            if(value is Quaternion)
            {
                Quaternion o = (Quaternion) value;
                //Add double type
                buffer.AddBytes(103);
                buffer.AddBytes(BitConverter.GetBytes(o.x));
                buffer.AddBytes(BitConverter.GetBytes(o.y));
                buffer.AddBytes(BitConverter.GetBytes(o.z));
                buffer.AddBytes(BitConverter.GetBytes(o.w));
                return;
            }
            #endregion

            //Serializable
            if(((System.Object) value).GetType().IsSerializable)
            {
                buffer.AddBytes(200);
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, value);
                    byte[] bt = ms.ToArray();
                    int length = bt.Length;
                    buffer.AddBytes(BitConverter.GetBytes(length));
                    buffer.AddBytes(bt);
                    return;
                }
            }

            //Byte Array
            if(value is string)
            {
                //Add string type
                buffer.AddBytes(201);
                //Add bytes length
                byte[] bt = (byte[]) value;
                int length = bt.Length;
                buffer.AddBytes(BitConverter.GetBytes(length));
                buffer.AddBytes(bt);
                return;
            }
        }

        /// <summary>
        /// Convert bytes to object
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="bytebuffer"></param>
        /// <param name="modeProvider"></param>
        /// <param name="type"></param>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="loadChilds"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        public static object Deserialize(ISaveObject parent,IByteBuffer bytebuffer,IEditModeProvider modeProvider,int type,byte[] bytes,ref int index,bool loadChilds,bool isFirst)
        {
            #region Internal
            //SaveObject
            if(type == 255)
            { 
                if(loadChilds || isFirst) 
                {
                    //Load
                    //Create child
                    SaveObject obj = new SaveObject();
                    obj.Init(bytebuffer,modeProvider);  
                    obj.SetParent(parent);
                    obj.Deserialize(bytes,ref index,loadChilds);
                    return obj;
                }
                else
                {
                    //Just skip
                    Skip(ref index,bytes);
                    return null;
                }       
            }

            //SaveObjectList
            if(type == 254)
            { 
                if(loadChilds || isFirst) 
                {
                    //Load
                    //Create child
                    SaveListObject obj = new SaveListObject();
                    obj.Init(bytebuffer,modeProvider);  
                    obj.SetParent(parent);
                    obj.Deserialize(bytes,ref index,loadChilds);
                    return obj;
                }
                else
                {
                    //Just skip
                    Skip(ref index,bytes);
                    return null;
                }       
            }
            #endregion
            
            #region Primitives
            //int
            if(type == 1)
            {
                object o = BitConverter.ToInt32(bytes,index);
                index += 4;
                return o;
            }

            //string
            if(type == 2)
            {
                int size = BitConverter.ToInt32(bytes,index);
                index += 4;

                string o = Encoding.ASCII.GetString(bytes,index,size);
                index += size;
                return o;
            }

            //float
            if(type == 3)
            {
                object o = BitConverter.ToSingle(bytes,index);
                index += 4;
                return o;
            }

            //double
            if(type == 4)
            {
                object o = BitConverter.ToDouble(bytes,index);
                index += 8;
                return o;
            }

            //bool
            if(type == 5)
                return true;
            if(type == 6)
                return false;
            #endregion

            #region Unity Objects
            //Color
            if(type == 100)
            {
                object o = new Color(BitConverter.ToSingle(bytes,index),
                    BitConverter.ToSingle(bytes,index + 4),
                    BitConverter.ToSingle(bytes,index + 8),
                    BitConverter.ToSingle(bytes,index + 12));
                index += 16;
                return o;
            }

            //Vector2
            if(type == 101)
            {
                object o = new Vector2(BitConverter.ToSingle(bytes,index),
                    BitConverter.ToSingle(bytes,index + 4));
                index += 8;
                return o;
            }

            //Vector3
            if(type == 102)
            {
                object o = new Vector3(BitConverter.ToSingle(bytes,index),
                    BitConverter.ToSingle(bytes,index + 4),
                    BitConverter.ToSingle(bytes,index + 8));
                index += 12;
                return o;
            }

            //Quaternion
            if(type == 103)
            {
                object o = new Quaternion(BitConverter.ToSingle(bytes,index),
                    BitConverter.ToSingle(bytes,index + 4),
                    BitConverter.ToSingle(bytes,index + 8),
                    BitConverter.ToSingle(bytes,index + 12));
                index += 16;
                return o;
            }
            #endregion


            //null
            if(type == 220)
                return null;

            //Serializable
            if(type == 200)
            {
                if(loadChilds || isFirst) 
                {
                    int size = BitConverter.ToInt32(bytes,index);
                    index += 4;


                    MemoryStream memStream = new MemoryStream();
                    BinaryFormatter binForm = new BinaryFormatter();
                    memStream.Write(bytes, index, size);
                    memStream.Seek(0, SeekOrigin.Begin);
                    object obj = (object) binForm.Deserialize(memStream);

                    index += size;

                    return obj;
                }

                //Just skip
                Skip(ref index,bytes);
                return null;
                
            }

            if(type == 201)
            {
                if(loadChilds || isFirst)
                {
                    int size = BitConverter.ToInt32(bytes,index);
                    index += 4;
                    byte[] bt = new byte[size];
                    Array.Copy(bytes,index,bt,0,size);
                    index += size;
                    return null;
                }
            }

            return null;
        }
        #endregion
    }
}