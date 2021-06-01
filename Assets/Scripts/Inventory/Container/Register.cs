using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.Data;

namespace SketchFleets.Inventory
{
    /// <summary>
    /// Base register class
    /// </summary>
    public abstract class Register<T> : ScriptableObject
    {
        #region Public Fields
        [SerializeField]
        public T[] items;
        #endregion

        /// <summary>
        /// Pick random index from register
        /// </summary>
        /// <returns></returns>
        public abstract int PickRandom();

        /// <summary>
        /// Get id for item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual int GetIdFor(T item)
        {
            for(int i = 0; i < items.Length; i ++)
                if(items[i].Equals(item))
                    return i;
            return -1;
        }
    }
}