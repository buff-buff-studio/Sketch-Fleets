using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SketchFleets.ProfileSystem;

namespace SketchFleets.Inventory
{
    public class InventoryContainer : Container
    {
        public TMPro.TMP_Text pageHeader;
        public Button prevPageButton;
        public Button nextPageButton;
        public ShopObjectRegister upgradeRegister;

        private int page = 0;

        List<ItemStack> items = new List<ItemStack>();
        public void OnEnable()
        {
            base.Start();
            
            items.Clear();
            foreach (ItemStack stack in Profile.GetData().inventoryItems)
                items.Add(stack);

            Render();
        }

        public void NextPage()
        {
            page ++;
            Render();
        }

        public void PreviousPage()
        {
            page --;
            Render();
        }

        public override void Render()
        {
            int firstslot = page * 6;
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

            if(slots.Length > 6)
                for(int i = 6; i < 10; i ++)
                {
                    ItemStack stack = GetItemInSlot(i);

                    Sprite sprite = null;
                    if (stack != null)
                        sprite = upgradeRegister.items[stack.Id].Icon;

                    GameObject obj = slots[i].GetChild(0).gameObject;
                    obj.GetComponent<Image>().sprite = sprite;
                    obj.SetActive(sprite != null);
                    slots[i].GetChild(1).GetComponent<TMPro.TMP_Text>().text = sprite == null ? "" : stack.Amount.ToString();

                }

            int pageCount = (int) Mathf.Ceil(items.Count / 6f);
            if(pageCount < 1)
                pageCount = 1;

            prevPageButton.interactable = page > 0;
            nextPageButton.interactable = page < pageCount - 1;

            pageHeader.text = (page + 1) + "/" + pageCount;
        }

        public override ItemStack GetItemInSlot(int slot)
        {
            if(slot >= 6)
            {
                //Upgrades
                return Profile.Data.inventoryUpgrades.SearchItem(new ItemStack(slot - 6,1)) > 0 ? new ItemStack(slot - 6,Profile.Data.inventoryUpgrades.SearchItem(new ItemStack(slot - 6))) : null;      
            }

            int firstslot = page * 6;
            return firstslot + slot < items.Count ? items[firstslot + slot] : null;
        }

        public override ShopObjectRegister GetRegisterForSlot(int slot)
        {
            if(slot >= 6)
                return upgradeRegister;

            return register;
        }
    }
}
