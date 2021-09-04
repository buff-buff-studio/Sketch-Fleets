using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets.Inventory
{   
    /// <summary>
    /// Stores all player upgrades
    /// </summary>
    public class UpgradeInventory : IInventory<ItemStack>, IEnumerable
    {
        List<ItemStack> upgrades = new List<ItemStack>();

        public bool CanAddItem(ItemStack stack)
        {
            return SearchItem(stack) == 0;
        }

        public int AddItem(ItemStack stack)
        {
            if(CanAddItem(stack))
            {
                upgrades.Add(stack.CopyOf(1));
                return stack.Amount - 1;
            }

            return stack.Amount;
        }

        public int GetSlotCount()
        {
            return int.MaxValue;
        }

        public int SearchItem(ItemStack stack)
        {
            return upgrades.Contains(stack) ? 1 : 0;
        }

        public bool UseItem(ItemStack stack)
        {
            return SearchItem(stack) > 0;
        }

        public IEnumerator GetEnumerator()
        {
            return upgrades.GetEnumerator();
        }

        public ItemStack GetItem(int slot)
        {
            return upgrades[slot];
        }
    }
}