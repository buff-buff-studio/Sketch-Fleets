using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets.Inventory
{
    public class ItemStack
    {
        #region Private Fields
        private int _amount;
        private int _id;
        #endregion

        #region Properties
        public int Amount { get { return _amount; } set { _amount = value; } }
        public int Id { get { return _id; } set { _id = value; } }
        #endregion

        #region Constructors
        public ItemStack(int id,int amount)
        {
            this._id = id;
            this._amount = amount;
        }

        public ItemStack(int id)
        {
            this._id = id;
            this._amount = 1;
        }
        #endregion

        #region Utils
        public ItemStack CopyOf(int amount)
        {
            return new ItemStack(_id,amount);
        }
        #endregion

        #region Object Utils
        /// <summary>
        /// Compare two objects
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(obj is ItemStack)
                return ((ItemStack) obj)._id == _id;

            return false;
        }

        /// <summary>
        /// Get itemstack hash code based on id
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
        #endregion
    }
}
