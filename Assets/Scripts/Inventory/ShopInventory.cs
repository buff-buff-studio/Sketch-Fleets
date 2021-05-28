using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets.Inventory
{   
    /// <summary>
    /// Stores all player upgrades
    /// </summary>
    public class ShopInventory : IInventory<ItemStack>, IEnumerable
    {
        List<ItemStack> items = new List<ItemStack>();

        public bool CanAddItem(ItemStack stack)
        {
            return true;
        }

        public int AddItem(ItemStack stack)
        {
            items.Add(stack.CopyOf(1));

            return stack.Amount;
        }

        public int GetSlotCount()
        {
            return int.MaxValue;
        }

        public int SearchItem(ItemStack stack)
        {
            return items.Contains(stack) ? 1 : 0;
        }

        public bool UseItem(ItemStack stack)
        {
            return false;
        }

        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public ItemStack GetItem(int slot)
        {
            return items[slot];
        }
    }
}