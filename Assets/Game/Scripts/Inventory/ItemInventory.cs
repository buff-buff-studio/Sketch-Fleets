using System.Collections;
using UnityEngine;

namespace SketchFleets.Inventory
{
    /// <summary>
    /// Base inventory class
    /// </summary>
    public class ItemInventory : IInventory<ItemStack>, IEnumerable
    {
        #region Protected Fields
        protected ItemStack[] items;
        #endregion

        #region Properties
        /// <summary>
        /// Index accessor
        /// </summary>
        /// <value></value>
        public ItemStack this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create new inventory
        /// </summary>
        /// <param name="slots"></param>
        public ItemInventory(int slots)
        {
            this.items = new ItemStack[slots];
        }
        #endregion

        #region Item Management
        /// <summary>
        /// Check if can add item to inventory
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        public bool CanAddItem(ItemStack stack)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                    return true;

                if (items[i].Equals(stack))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Add item returning added amount 
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        public virtual int AddItem(ItemStack stack)
        {
            if (!CanAddItem(stack))
                return stack.Amount;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = stack;
                    return 0;
                }

                if (items[i].Equals(stack))
                {
                    items[i].Amount += stack.Amount;
                    return 0;
                }
            }

            return stack.Amount;
        }

        /// <summary>
        /// Remove item from inventory
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public int RemoveItem(int slot, int amount)
        {
            if (items[slot] == null)
                return 0;

            int remove = Mathf.Min(amount, items[slot].Amount);

            if (items[slot].Amount == remove)
                items[slot] = null;
            else
                items[slot].Amount -= remove;

            return remove;
        }

        /// <summary>
        /// Remove item from inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int RemoveItem(ItemStack item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                    if (items[i].Equals(item))
                    {
                        return RemoveItem(i,item.Amount);
                    }
            }

            return 0;
        }

        /// <summary>
        /// Count stored item amount
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int SearchItem(ItemStack item)
        {
            for (int i = 0; i < items.Length; i++)
                if(items[i] != null)
                    if (items[i].Equals(item))
                        return items[i].Amount;

            return 0;
        }
        
        /// <summary>
        /// Try to use item
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        public bool UseItem(ItemStack stack)
        {
            int count = SearchItem(stack);
            if(count < stack.Amount)
                return false;
            
            RemoveItem(stack);

            return true;
        }

        public ItemStack GetItem(int slot)
        {
            return items[slot];
        }
        #endregion

        #region Utils
        /// <summary>
        /// Enumerate through stored items
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < items.Length; i++)
                if (items[i] != null)
                    if(items[i].Amount > 0)
                        yield return items[i];
        }

        /// <summary>
        /// Get inventory slot count
        /// </summary>
        /// <returns></returns>
        public int GetSlotCount()
        {
            return items.Length;
        }
        #endregion
    }
}
