using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SketchFleets.ProfileSystem;

namespace SketchFleets.Inventory
{
    public class InventoryContainer : Container
    {
        #region Public Fields
        public TMPro.TMP_Text pageHeader;
        public Button prevPageButton;
        public Button nextPageButton;
        public ShopObjectRegister upgradeRegister;
        public int slotSize = 6;

        public TMPro.TMP_Text coinCounter;
        public RectTransform coinCountBackground;
        public Image currencyIcon;
        public Sprite[] currencyIconSprites;
        public bool isUpgradeInventory = false;
        #endregion

        #region Protected Fields
        protected int page = 0;
        protected List<ItemStack> items = new List<ItemStack>();
        #endregion

        #region Unity Callbacks
        public void OnEnable()
        {
            base.Start();

            //Coin Icon
            if (currencyIcon != null)
                currencyIcon.sprite = isUpgradeInventory ? currencyIconSprites[1] : currencyIconSprites[0];

            if (coinCounter != null)
                if (isUpgradeInventory)
                    coinCounter.text = Profile.Data.TotalCoins.ToString();
                else
                    coinCounter.text = Profile.Data.Coins.ToString();

            items.Clear();
            foreach (ItemStack stack in Profile.GetData().inventoryItems)
                if (FilterItem(stack))
                    items.Add(stack);

            Render();
        }

        public virtual bool FilterItem(ItemStack stack)
        {
            return true;
        }
        #endregion

        #region Buttons
        public void NextPage()
        {
            page++;
            Render();
        }

        public void PreviousPage()
        {
            page--;
            Render();
        }
        #endregion

        #region Main Renderer
        public override void Render()
        {
            int firstslot = page * slotSize;
            int j = 0;
            for (int i = firstslot; i < firstslot + slotSize; i++)
            {

                ItemStack stack = (i < items.Count) ? items[i] : null;
                Sprite sprite = null;
                if (stack != null && stack.Amount > 0)
                    sprite = register.items[stack.Id].Icon;

                GameObject obj = slots[j].GetChild(0).gameObject;
                obj.GetComponent<Image>().sprite = sprite;
                obj.SetActive(sprite != null);
                slots[j].GetChild(1).GetComponent<TMPro.TMP_Text>().text = sprite == null ? "" : (stack.Amount == 1 ? "" : stack.Amount.ToString());

                j++;
            }

            if (slots.Length > slotSize)
                for (int i = slotSize; i < slotSize + upgradeRegister.items.Length; i++)
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

            int pageCount = (int)Mathf.Ceil(items.Count / (slotSize * 1f));
            if (pageCount < 1)
                pageCount = 1;

            if (prevPageButton != null)
                prevPageButton.interactable = page > 0;
            if (nextPageButton != null)
                nextPageButton.interactable = page < pageCount - 1;
            if (pageHeader != null)
                pageHeader.text = (page + 1) + "/" + pageCount;

            for (int i = 0; i < slots.Length; i++)
            {

                Button btn = slots[i].GetComponent<Button>();
                int ji = i;
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() =>
                    {
                        OnClickSlotInternal(ji);
                    });
                }
            }
        }
        #endregion

        #region Utils
        public override ItemStack GetItemInSlot(int slot)
        {
            if (slot >= slotSize)
            {
                //Upgrades
                return Profile.Data.inventoryUpgrades.SearchItem(new ItemStack(slot - slotSize, 1)) > 0 ? new ItemStack(slot - slotSize, Profile.Data.inventoryUpgrades.SearchItem(new ItemStack(slot - slotSize))) : null;
            }

            int firstslot = page * slotSize;
            return firstslot + slot < items.Count ? items[firstslot + slot] : null;
        }

        public override ShopObjectRegister GetRegisterForSlot(int slot)
        {
            if (slot >= slotSize)
                return upgradeRegister;

            return register;
        }

        protected override void Update()
        {
            base.Update();

            if (coinCountBackground != null)
                coinCountBackground.sizeDelta = new Vector2(coinCounter.GetRenderedValues(true).x + 200, 130);
        }
        #endregion
    }
}
