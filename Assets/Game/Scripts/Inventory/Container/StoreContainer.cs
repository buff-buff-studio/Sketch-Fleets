using TMPro;
using SketchFleets.ProfileSystem;
using UnityEngine;
using UnityEngine.UI;
using SketchFleets.Data;
using System.Collections.Generic;
using ManyTools.Events;
using SketchFleets.Interaction;

namespace SketchFleets.Inventory
{
    public class FallingItem
    {
        public GameObject Object;
        public float Time = 0;

        public FallingItem(GameObject gameObject,float time)
        {
            this.Object = gameObject;
            this.Time = time;
        }
    }
    /// <summary>
    /// Store container class
    /// </summary>
    public sealed class StoreContainer : Container
    {
        #region Private Fields

        [Header("Events")]
        [SerializeField]
        private GameEvent onItemBought;

        #endregion
        
        #region Public Fields
        
        [Header("Shop Parameters")]
        public GameObject itemInformationPanel;

        public List<FallingItem> fallingItems = new List<FallingItem>();

        public TMP_Text itemBuyConfirmation;
        public TMP_Text itemBuyPrice;
        public TMP_Text itemAmount;

        public TMP_Text coinCounter;
        public bool isUpgradeShop = false;
        public Image currencyIcon;
        public Sprite[] currencyIconSprites;
        public RectTransform coinCountBackground;

        public AudioSource buySound;
        public AudioSource noMoneySound;

        public GameObject cardPrefab;

        public MapLevelInteraction interaction;
        #endregion

        private static int selectItemIndex = -1;

        #region Unity Callbacks
        public override void Start()
        {
            base.Start();

            //Coin Icon
            if(currencyIcon != null)
                currencyIcon.sprite = isUpgradeShop ? currencyIconSprites[1] : currencyIconSprites[0];

            //Load
            inventory = new StoreInventory();
            Random.InitState((int) (Time.time * 35678));
            for (int i = 0; i < (isUpgradeShop ? register.items.Length : slots.Length); i++)
            {
                ItemStack stack = new ItemStack(register.PickRandom(i));
                inventory.AddItem(stack);
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

        public void OnEnable()
        {
            foreach(FallingItem rb in fallingItems)
            {
                Destroy(rb.Object);
            }

            fallingItems.Clear();
        }

        public void OnDisable() 
        {
            foreach(FallingItem rb in fallingItems)
            {
                Destroy(rb.Object);
            }

            fallingItems.Clear();
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

            Sprite sprite = null;
            if (stack != null)
                sprite = register.items[stack.Id].Icon;
                
            #region Temporary
            if(stack != null)
            {
                #region Increase price per bought amount
                int count = isUpgradeShop ? Profile.GetData().inventoryUpgrades.SearchItem(stack) : Profile.GetData().inventoryItems.SearchItem(stack);
                ShopObject item = register.items[stack.Id];
                int increase = item.ItemCostIncreasePerUnit.Value * count;
                #endregion

                slots[index].GetChild(0).GetComponentInChildren<TMP_Text>().text = (item.ItemCost.Value + increase) + "$";
                slots[index].GetChild(1).GetComponent<Image>().sprite = sprite;
                slots[index].GetChild(1).GetComponent<ItemStackAnimation>().isBuyable = item.ItemAmountLimit == 0 || count < item.ItemAmountLimit;
            }
            slots[index].gameObject.SetActive(sprite != null);
            #endregion
        }

        public void OpenItemInformation()
        {   
            ItemStack stack = inventory.GetItem(selectItemIndex);
            ShopObject item = register.items[stack.Id];

            int count = isUpgradeShop ? Profile.GetData().inventoryUpgrades.SearchItem(stack) : Profile.GetData().inventoryItems.SearchItem(stack);
            if(item.ItemAmountLimit != 0 && count >= item.ItemAmountLimit)
                return;

            itemInformationPanel.SetActive(true);

            #region Increase price per bought amount
            int increase = register.items[stack.Id].ItemCostIncreasePerUnit.Value * Profile.Data.inventoryUpgrades.SearchItem(stack);
            #endregion

            itemBuyPrice.text = (item.ItemCost.Value + increase).ToString();
            string name = LanguageSystem.LanguageManager.Localize(item.UnlocalizedName);
            itemAmount.text = LanguageSystem.LanguageManager.Localize("ui_shop_amount",count.ToString(),name);
            itemBuyConfirmation.text = LanguageSystem.LanguageManager.Localize("ui_shop_buy_confirmation","1",name);
        }

        public void BuyItem()
        {
            ItemStack stack = inventory.GetItem(selectItemIndex);

            ShopObject item = register.items[stack.Id];

            #region Increase price per bought amount
            int increase = register.items[stack.Id].ItemCostIncreasePerUnit.Value * Profile.Data.inventoryUpgrades.SearchItem(stack);
            #endregion
                
            int itemCost = item.ItemCost.Value + increase;

            if (GetCoins() < itemCost)
            {
                noMoneySound.Play();
                //Money blink
                moneyAnim = 720;
                itemInformationPanel.SetActive(false);
                return;
            }

            AddCoins(-itemCost);

            itemInformationPanel.SetActive(false);

            //Add item to inventory
            if(isUpgradeShop)
            {
                Profile.GetData().inventoryUpgrades.AddItem(new ItemStack(inventory.GetItem(selectItemIndex).Id, 1));
            }
            else
            {
                Profile.GetData().inventoryItems.AddItem(new ItemStack(inventory.GetItem(selectItemIndex).Id, 1));
                onItemBought?.Invoke();
            }

            if(Random.Range(0,0.999f) <= 0.2f)
            {
                if(isUpgradeShop)
                {
                    //Add id
                    if(Profile.GetData().codex.AddItem(new CodexEntry(CodexEntryType.Upgrade, stack.Id)) == 0)
                    {
                        DropCard(selectItemIndex);
                    }
                }
                else
                {
                    //Add id
                    if(Profile.GetData().codex.AddItem(new CodexEntry(CodexEntryType.Item, stack.Id)) == 0)
                        DropCard(selectItemIndex);
                }
            }

            buySound.Play();

            //Clone item
            CloneItem(selectItemIndex);
            
            //Refresh container
            Render();

            Profile.SaveProfile((data) => { });
        }

        public void DropCard(int slot)
        {
            GameObject source = slots[slot].GetChild(1).gameObject;
            GameObject obj = GameObject.Instantiate(cardPrefab);
            obj.transform.parent = slots[slot].parent;
            obj.transform.position = source.transform.position;
            obj.transform.eulerAngles = source.transform.eulerAngles;
            obj.transform.localScale = source.transform.localScale;
            obj.SetActive(true);

            ApplyPhysics(obj,true);
        }

        public void CloneItem(int slot)
        {
            GameObject source = slots[slot].GetChild(1).gameObject;
            GameObject obj = GameObject.Instantiate(source);
            
            obj.name = "Falling Item";
            obj.transform.parent = slots[slot].parent;
            obj.transform.position = source.transform.position;
            obj.transform.eulerAngles = source.transform.eulerAngles;
            obj.transform.localScale = source.transform.localScale;

            ApplyPhysics(obj,false);
        }
        public void ApplyPhysics(GameObject obj,bool card)
        {
            Destroy(obj.GetComponent<ItemStackAnimation>());
            Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
            fallingItems.Add(new FallingItem(obj,3));          

            float degrees = rb.transform.eulerAngles.z;

            rb.AddTorque((obj.transform.eulerAngles.z < 180 ? -1 : 1) * Random.Range(200,400));

            Vector2 dir = new Vector2(-Mathf.Sin(degrees * Mathf.Deg2Rad)/2f,Mathf.Cos(degrees * Mathf.Deg2Rad));

            rb.drag = 0.5f;
            rb.gravityScale = card ? 50f : 75f;
            rb.AddForce(dir * Random.Range(400,500),ForceMode2D.Impulse);

            if(fallingItems.Count > 3)
                Destroy(fallingItems[0].Object);  
        }

        public void CancelBuy()
        {
            itemInformationPanel.SetActive(false);
        }

        public void CloseToMap()
        {
            interaction.ReturnToMapOpeningStar();
        }

        /// <summary>
        /// Handle integration while adding coins
        /// </summary>
        /// <param name="count"></param>
        public void AddCoins(int count)
        {
            if (isUpgradeShop)
            {
                Profile.Data.TotalCoins += count;
                if (coinCounter != null)
                {
                    coinCounter.text = Profile.Data.TotalCoins.ToString();
                }
                return;
            }

            Profile.Data.Coins += count;
            if (coinCounter != null)
            {
                coinCounter.text = Profile.Data.Coins.ToString();
            }
        }

        /// <summary>
        /// Handle integration while retriving coints count
        /// </summary>
        /// <returns></returns>
        public int GetCoins()
        {
            if (isUpgradeShop)
                return Profile.Data.TotalCoins;

            return Profile.Data.Coins;
        }

        private float moneyAnim = 0f;
        protected override void Update()
        {
            base.Update();

            coinCountBackground.sizeDelta = new Vector2(coinCounter.GetRenderedValues(true).x + 200,130);

            foreach(FallingItem rb in fallingItems)
            {
                rb.Time -= Time.deltaTime;
                if(rb.Time <= 0)
                {
                    Destroy(rb.Object);
                    fallingItems.Remove(rb);
                    return;
                }
            }

            moneyAnim -= Time.deltaTime * 720;
            if(moneyAnim < 0)
                moneyAnim = 0;

            coinCounter.color = Color.Lerp(Color.white,Color.red,Mathf.Sin(moneyAnim * Mathf.Deg2Rad));
        }
        #endregion
    }
}