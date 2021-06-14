using SketchFleets.Data;
using SketchFleets.Inventory;
using SketchFleets.LanguageSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SketchFleets.UI
{
    /// <summary>
    /// A class that controls and populates a codex card
    /// </summary>
    public class CodexCard : MonoBehaviour
    {
        #region Private Fields

        [Header("Text")]
        [SerializeField, Tooltip("The name of the card")]
        private TMP_Text cardName;
        [SerializeField, Tooltip("The description of the card")]
        private TMP_Text cardDescription;
        [SerializeField, Tooltip("The health of the card")]
        private TMP_Text cardHealth;
        [SerializeField, Tooltip("The description of the card")]
        private TMP_Text cardDamage;

        [Header("Images")]
        [SerializeField, Tooltip("The image component for when the card is for a ship")]
        private Image shipCardImage;
        [SerializeField, Tooltip("The card's own image component")]
        private Image cardImage;

        [Header("Cards")]
        [SerializeField, Tooltip("The image for the bronze card")]
        private Sprite bronzeCard;
        [SerializeField, Tooltip("The image for the silver card")]
        private Sprite silverCard;
        [SerializeField, Tooltip("The image for the gold card")]
        private Sprite goldCard;

        [SerializeField, Tooltip("The image for the item bronze card")]
        private Sprite itemBronzeCard;
        [SerializeField, Tooltip("The image for the item silver card")]
        private Sprite itemSilverCard;
        [SerializeField, Tooltip("The image for the item gold card")]
        private Sprite itemGoldCard;
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Fills the card with a given ship attribute
        /// </summary>
        /// <param name="attributes"></param>
        public void FillCardWithShip(ShipAttributes attributes)
        {
            WriteToCardShip(attributes);
            GetImageShip(attributes);
            SetCardRarity(attributes.CodexRarity);
        }

        public void FillCardWithUpgrade(Upgrade upgrade)
        {
            WriteToCardUpgrade(upgrade);
            GetImageUpgrade(upgrade);
            SetCardRarityItem(upgrade.CodexEntryRarity);
        }

        public void FillCardWithItem(Item item)
        {
            WriteToCardItem(item);
            GetImageItem(item);
            SetCardRarityItem(item.CodexEntryRarity);
        }
        
        public void FillCardOnlyWithRarity(CodexEntryRarity rarity)
        {
            SetCardRarityItem(rarity);
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Writes appropriate text to the card
        /// </summary>
        private void WriteToCardShip(ShipAttributes ship)
        {
            Debug.Log(ship.UnlocalizedName);
            cardName.text = LanguageManager.Localize(ship.UnlocalizedName);
            cardDescription.text = LanguageManager.Localize(ship.UnlocalizedDescription);
            cardHealth.text = ship.MaxHealth.Value.ToString();
            cardDamage.text = GetShipDamage(ship).ToString();
        }

        private void WriteToCardUpgrade(Upgrade upgrade)
        {
            cardName.text = LanguageManager.Localize(upgrade.UnlocalizedName);
            cardDescription.text = LanguageManager.Localize("desc_" + upgrade.UnlocalizedName);
            cardHealth.text = "";
            cardDamage.text = "";
        }
        
        private void WriteToCardItem(Item item)
        {
            cardName.text = LanguageManager.Localize(item.UnlocalizedName);
            cardDescription.text = LanguageManager.Localize("desc_" + item.UnlocalizedName);
            cardHealth.text = "";
            cardDamage.text = "";
        }

        /// <summary>
        /// Gets the image for the card out of a ship attribute
        /// </summary>
        private void GetImageShip(ShipAttributes ship)
        {
            ship.Prefab.TryGetComponent(out SpriteRenderer sRenderer);
            shipCardImage.sprite = sRenderer.sprite;
            shipCardImage.material = sRenderer.sharedMaterial;
        }

        private void GetImageUpgrade(Upgrade upgrade)
        {     
            shipCardImage.sprite = upgrade.Icon;
            shipCardImage.material = null;
            shipCardImage.preserveAspect = true;
        }

        private void GetImageItem(Item item)
        {     
            shipCardImage.sprite = item.Icon;
            shipCardImage.material = null;
            shipCardImage.preserveAspect = true;
        }
        

        /// <summary>
        /// Gets the damage of a ship
        /// </summary>
        /// <param name="ship">The ship to get the damage of</param>
        /// <returns>The damage of the given ship</returns>
        private float GetShipDamage(ShipAttributes ship)
        {
            return ship.Fire != null ? ship.Fire.DirectDamage.Value : ship.CollisionDamage.Value;
        }

        /// <summary>
        /// Sets the card's rarity
        /// </summary>
        /// <param name="rarity">The rarity of the card</param>
        private void SetCardRarity(CodexEntryRarity rarity)
        {
            cardImage.sprite = rarity switch
            {
                CodexEntryRarity.Bronze => bronzeCard,
                CodexEntryRarity.Silver => silverCard,
                CodexEntryRarity.Gold => goldCard,
            };
        }

        /// <summary>
        /// Sets the card's rarity
        /// </summary>
        /// <param name="rarity">The rarity of the card</param>
        private void SetCardRarityItem(CodexEntryRarity rarity)
        {
            cardImage.sprite = rarity switch
            {
                CodexEntryRarity.Bronze => itemBronzeCard,
                CodexEntryRarity.Silver => itemSilverCard,
                CodexEntryRarity.Gold => itemGoldCard,
            };
        }
        
        #endregion
    }
}
