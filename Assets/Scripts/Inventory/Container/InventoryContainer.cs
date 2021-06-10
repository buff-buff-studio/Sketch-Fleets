using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SketchFleets.ProfileSystem;

namespace SketchFleets.Inventory
{
    public class InventoryContainer : Container
    {
        List<ItemStack> items = new List<ItemStack>();
        public void OnEnable()
        {
            base.Start();
            
            items.Clear();
            foreach (ItemStack stack in Profile.GetData().inventoryItems)
                items.Add(stack);

            Render();
        }

        public override void Render()
        {
            int firstslot = 0;
            int j = 0;
            for(int i = firstslot; i < firstslot + 6; i ++)
            {
                ItemStack stack = (i < items.Count) ? items[i] : null;

                Sprite sprite = null;
                if (stack != null)
                    sprite = register.items[stack.Id].Icon;

                GameObject obj = slots[j].GetChild(0).gameObject;
                obj.GetComponent<Image>().sprite = sprite;
                obj.SetActive(sprite != null);
                slots[j].GetChild(1).GetComponent<TMPro.TMP_Text>().text = sprite == null ? "" : (stack.Amount == 1 ? "" : stack.Amount.ToString());

                j ++;
            }
        }

        public override ItemStack GetItemInSlot(int slot)
        {
            int firstslot = 0;
            return firstslot + slot < items.Count ? items[firstslot + slot] : null;
        }
    }
}
