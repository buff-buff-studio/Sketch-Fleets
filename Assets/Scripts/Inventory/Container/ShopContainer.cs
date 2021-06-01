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

        #region Unity Callbacks
        public new void Start() 
        {
            base.Start();

            //Update label
            UpdateLabel();

            //Handlers
            OnClickSlot = OnClickSlotMethod;
        }
        #endregion

        #region Container
        public void OnClickSlotMethod(int slot)
        {
            //Add item to player inventory
            Profile.GetData().inventoryItems.AddItem(new ItemStack(inventory.GetItem(slot).Id,1));
            Profile.Using(this);
            Profile.SaveProfile((data) => {});

            //Update labels
            UpdateLabel();
        }
        #endregion
        
        #region Temporary
        public void UpdateLabel()
        {
            string s = "Player Inventory:\n";
            foreach(ItemStack i in Profile.GetData().inventoryItems)
            {
                s += register.items[i.Id].UnlocalizedName + " * " + i.Amount + "\n";
            }
        
            s += "\nCodex:\n";

            foreach(CodexEntry entry in Profile.GetData().codex.GetUnlockedEntries(CodexEntryType.Item))
            {
                s += entry.type + ": " + entry.id + "\n";
            }

            inventoryLabel.text = s;
        }
        #endregion
    }
}