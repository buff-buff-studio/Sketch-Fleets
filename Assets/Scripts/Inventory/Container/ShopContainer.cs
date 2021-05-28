using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SketchFleets.ProfileSystem;

namespace SketchFleets.Inventory
{
    /// <summary>
    /// Shop container class
    /// </summary>
    public class ShopContainer : Container
    {
        #region Temporary
        public TMP_Text inventoryLabel;
        #endregion

        public new void Start() 
        {
            base.Start();

            //Update label
            UpdateLabel();

            //Handlers
            OnClickSlot = OnClickSlotMethod;
        }

        public void OnClickSlotMethod(int slot)
        {
            //Add item to player inventory
            Profile.GetData().inventoryItems.AddItem(new ItemStack(inventory.GetItem(slot).Id,1));
            Profile.Using(this);
            Profile.SaveProfile((data) => {});

            //Update labels
            UpdateLabel();
        }

        public void UpdateLabel()
        {
            string s = "Player Inventory:\n";
            foreach(ItemStack i in Profile.GetData().inventoryItems)
            {
                s += register.items[i.Id].UnlocalizedName + " * " + i.Amount + "\n";
            }
            inventoryLabel.text = s.Length == 0 ? "{EMPTY}" : s;
        }
    }
}