using System.Collections.Generic;
using UnityEngine;
using SketchFleets.ProfileSystem;
using SketchFleets.Data;
using SketchFleets.UI;

namespace SketchFleets.Inventory
{
    /// <summary>
    /// Main Codex container class
    /// </summary>
    public class CodexContainer : MonoBehaviour
    {
        #region Registers

        [Header("Cards")]
        [SerializeField, Tooltip("The card template")]
        private GameObject codexCard;
        [SerializeField, Tooltip("The locked card template")]
        private GameObject lockedCodexCard;
        
        [Header("Display Container")]
        [SerializeField, Tooltip("The object that contains all cards")]
        private GameObject containerDisplay;
        
        [Header("Registers")]
        [SerializeField, Tooltip("The register for this kind of entry")]
        private ShopObjectRegister registerItems;
        [SerializeField, Tooltip("The register for this kind of entry")]
        private ShopObjectRegister registerUpgrades;
        [SerializeField, Tooltip("The register for this kind of entry")]
        private ShipRegister registerShips;

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            AddAllCards();
        }

        private void OnDisable()
        {
            ClearAllCards();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds all cards to the codex
        /// </summary>
        private void AddAllCards()
        {
            DisplayAllCards(CodexEntryType.Ship, registerShips);
            DisplayAllCards(CodexEntryType.Item, registerItems);
            DisplayAllCards(CodexEntryType.Upgrade, registerUpgrades);
        }

        /// <summary>
        /// Render all entries
        /// </summary>
        /// <param name="type"></param>
        /// <param name="register"></param>
        /// <typeparam name="T"></typeparam>
        private void DisplayAllCards<T>(CodexEntryType type, Register<T> register)
        {
            List<int> unlockedList = new List<int>();

            //Add unlocked id
            foreach (CodexEntry entry in Profile.GetData().codex.GetUnlockedEntries(type)) unlockedList.Add(entry.ID);

            //Render items
            for (int index = 0; index < register.items.Length; index++)
            {
                bool isUnlocked = unlockedList.Contains(index);
                DisplayCard(index, register.items[index], isUnlocked);
            }
        }

        /// <summary>
        /// Displays a single card
        /// </summary>
        /// <param name="index"></param>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <param name="unlocked"></param>
        /// <returns></returns>
        private bool DisplayCard(int id, object item, bool unlocked)
        {
            switch (item)
            {
                case Item codexItem:
                    CreateItemCard((Item) registerItems.items[id], unlocked);
                    break;
                
                case Upgrade upgradeItem:
                    CreateUpgradeCard((Upgrade) registerUpgrades.items[id], unlocked);
                    break;

                case ShipAttributes codexShip:
                    CreateShipCard(registerShips.items[id], unlocked);
                    break;
                default:
                    break;
            }

            return true;
        }
        
        /// <summary>
        /// Creates a codex card with a ship
        /// </summary>
        /// <param name="ship">The ship to create the card with</param>
        /// <param name="unlocked">Whether the given card has been unlocked</param>
        private void CreateShipCard(ShipAttributes ship, bool unlocked)
        {
            Debug.Log(ship.Name);
            Instantiate(unlocked ? codexCard : lockedCodexCard).TryGetComponent(out CodexCard card);
            AddToDisplay(card.gameObject);
            if (unlocked)
                card.FillCardWithShip(ship);
            else
                card.FillCardOnlyWithRarity(ship.CodexRarity);
        }

        /// <summary>
        /// Create a codex card with an item
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="unlocked"></param>
        private void CreateItemCard(Item item, bool unlocked)
        {
            Instantiate(unlocked ? codexCard : lockedCodexCard).TryGetComponent(out CodexCard card);
            AddToDisplay(card.gameObject);
            if (unlocked)
                card.FillCardWithItem(item);
            else
                card.FillCardOnlyWithRarity(item.CodexEntryRarity);
        }

        /// <summary>
        /// Create a codex card with an upgrade
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="unlocked"></param>
        private void CreateUpgradeCard(Upgrade upgrade, bool unlocked)
        {
            Instantiate(unlocked ? codexCard : lockedCodexCard).TryGetComponent(out CodexCard card);
            AddToDisplay(card.gameObject);
            if (unlocked)
                card.FillCardWithUpgrade(upgrade);
            else
                card.FillCardOnlyWithRarity(upgrade.CodexEntryRarity);
        }
        
        /// <summary>
        /// Adds a given card to the display
        /// </summary>
        /// <param name="card">The card to add</param>
        private void AddToDisplay(GameObject card)
        {
            card.transform.SetParent(containerDisplay.transform);
            card.transform.localScale = Vector3.one;
            card.transform.localEulerAngles = new Vector3(0,0,Random.Range(-3f,3f));
        }

        /// <summary>
        /// Clears all cards from codex
        /// </summary>
        private void ClearAllCards()
        {
            int index = 0;
            var children = new GameObject[containerDisplay.transform.childCount];

            foreach (Transform child in containerDisplay.transform)
            {
                children[index] = child.gameObject;
                index++;
            }

            for (int destroyIndex = 0, upper = children.Length; destroyIndex < upper; destroyIndex++)
            {
                Debug.Log(destroyIndex);
                DestroyImmediate(children[destroyIndex].gameObject);
            }
        }

        #endregion
    }
}