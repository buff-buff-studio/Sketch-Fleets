using TMPro;
using SketchFleets.ProfileSystem;
using UnityEngine;
using UnityEngine.UI;
using SketchFleets.Data;


namespace SketchFleets.Inventory
{
    /// <summary>
    /// Store container class
    /// </summary>
    public class StoreContainer : Container
    {
        #region Public Fields
        public GameObject itemInformationPanel;
        public TMP_Text itemInformationText;
        public TMP_Text playerInventoryText;
        public TMP_Text coinCounter;
        public bool useTotalCoins = false;
        public Image currencyIcon;
        public Sprite[] currencyIconSprites;
        #endregion

        private static int selectItemIndex = -1;

        #region Unity Callbacks
        public override void Start()
        {
            base.Start();

            //Coin Icon
            if(currencyIcon != null)
                currencyIcon.sprite = useTotalCoins ? currencyIconSprites[1] : currencyIconSprites[0];

            //Load
            inventory = new StoreInventory();
            for (int i = 0; i < slots.Length; i++)
            {
                inventory.AddItem(new ItemStack(register.PickRandom()));
            }

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

            //Render items
            Render();

            //Handlers
            OnClickSlot = OnClickSlotMethod;

            //Update label
            AddCoins(0);
        }
        #endregion   

        #region Container
        public void OnClickSlotMethod(int slot)
        {
            //Show select item information
            selectItemIndex = slot;
            OpenItemInformation();

            /*
            Profile.GetData().inventoryItems.AddItem(new ItemStack(inventory.GetItem(slot).Id,1));
            Profile.Using(this);
            Profile.SaveProfile((data) => {});
            */
        }
        #endregion

        #region Screen/UI
        public override void RenderSlot(int index)
        {
            ItemStack stack = inventory.GetItem(index);

            string name = "";
            if (stack != null)
                name = register.items[stack.Id].UnlocalizedName;

            #region Temporary
            slots[index].GetChild(0).GetComponent<TMP_Text>().text = name;
            #endregion
        }

        public void OpenItemInformation()
        {
            itemInformationPanel.SetActive(true);
            ItemStack stack = inventory.GetItem(selectItemIndex);

            Item item = register.items[stack.Id];

            int count = Profile.GetData().inventoryItems.SearchItem(stack);

            itemInformationText.text = "Do you really want to buy '" + item.UnlocalizedName + "' for $" + item.ItemCost + " ? (You have " + count + " " + item.UnlocalizedName + ")\n\nPrice: " + item.ItemCost;
        }

        public void BuyItem()
        {
            ItemStack stack = inventory.GetItem(selectItemIndex);

            Item item = register.items[stack.Id];

            if (GetCoins() < item.ItemCost)
            {
                Debug.Log("Not enough money!");
                return;
            }

            AddCoins(-item.ItemCost);

            itemInformationPanel.SetActive(false);

            //Add item to inventory
            Profile.GetData().inventoryItems.AddItem(new ItemStack(inventory.GetItem(selectItemIndex).Id, 1));
            Profile.SaveProfile((data) => { });
        }

        public void CancelBuy()
        {
            itemInformationPanel.SetActive(false);
        }

        public void UpdatePlayerInventory()
        {
            //Update player items
            string s = "Player Items:\n";

            foreach (ItemStack stack in Profile.GetData().inventoryItems)
            {
                s += register.items[stack.Id].UnlocalizedName + ": " + stack.Amount + "\n";
            }

            playerInventoryText.text = s;
        }

        public void CloseToMap()
        {
            MapLevelInteraction.ReturnToMapOpeningStar();
        }

        /// <summary>
        /// Handle integration while adding coins
        /// </summary>
        /// <param name="count"></param>
        public void AddCoins(int count)
        {
            if (useTotalCoins)
            {
                Profile.Data.TotalCoins += count;
                if (coinCounter != null)
                    coinCounter.text = "$" + Profile.Data.TotalCoins;
                return;
            }

            Profile.Data.Coins += count;
            if (coinCounter != null)
                coinCounter.text = "$" + Profile.Data.Coins;
        }

        /// <summary>
        /// Handle integration while retriving coints count
        /// </summary>
        /// <returns></returns>
        public int GetCoins()
        {
            if (useTotalCoins)
                return Profile.Data.TotalCoins;

            return Profile.Data.Coins;
        }
        #endregion
    }
}