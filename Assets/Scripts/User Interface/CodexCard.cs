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
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Fills the card with a given ship attribute
        /// </summary>
        /// <param name="attributes"></param>
        public void FillCardWithShip(ShipAttributes attributes)
        {
            WriteToCard(attributes);
            GetShipImage(attributes);
            SetCardRarity(attributes.CodexRarity);
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Writes appropriate text to the card
        /// </summary>
        private void WriteToCard(ShipAttributes ship)
        {
            cardName.text = LanguageManager.Localize(ship.UnlocalizedName);
            cardDescription.text = LanguageManager.Localize(ship.UnlocalizedDescription);
            cardHealth.text = ship.MaxHealth.Value.ToString();
            cardDamage.text = GetShipDamage(ship).ToString();
        }

        /// <summary>
        /// Gets the image for the card out of a ship attribute
        /// </summary>
        private void GetShipImage(ShipAttributes ship)
        {
            ship.Prefab.TryGetComponent(out SpriteRenderer sRenderer);
            shipCardImage.sprite = sRenderer.sprite;
            shipCardImage.material = sRenderer.sharedMaterial;
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
        
        #endregion
    }
}
