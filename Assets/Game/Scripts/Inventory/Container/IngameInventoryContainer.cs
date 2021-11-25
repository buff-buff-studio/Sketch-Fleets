using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SketchFleets.Data;

namespace SketchFleets.Inventory
{
    public class IngameInventoryContainer : InventoryContainer
    {
        private void Awake() 
        {
            OnClickSlot = OnClickSlotInv;

            for (int i = 0; i < slots.Length; i++)
            {
                Button btn = slots[i].GetComponent<Button>();
                int ji = i;
                if (btn != null)
                    btn.onClick.AddListener(() =>
                    {
                        OnClickSlotInternal(ji);
                    });
            }
        }

        public override bool FilterItem(ItemStack stack)
        {
            return ((Item) register.items[stack.Id]).IsConsumable;
        }

        public void OnClickSlotInv(int slot)
        {
            if(slot + page * 4 >= items.Count)
                return;

            ItemStack it = items[slot + page * 4];
            if(it != null)
            {  
                ItemInventory inventory = ProfileSystem.Profile.Data.inventoryItems;
                for(int i = 0; i < inventory.GetSlotCount(); i ++)
                {
                    ItemStack stack = inventory.GetItem(i);
                    if(stack != null)
                    if(stack.Id == it.Id)
                    {
                        //Use item
                        OnUseItem((Item) register.items[it.Id]);
                        inventory.RemoveItem(i,1);
                        ProfileSystem.Profile.SaveProfile((data) => { });
                        
                        items.Clear();
                        foreach (ItemStack stackb in ProfileSystem.Profile.GetData().inventoryItems)
                            if(FilterItem(stackb))
                            items.Add(stackb);

                        Render();
                        break;
                    }
                }
            }
        }

        public void OnUseItem(Item item)
        {
            //On item used
            ItemEffect effect = item.Effect;

            IngameEffectApplier.ApplyItem(item);
        }
    }
}
