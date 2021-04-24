using ManyTools.UnityExtended.Editor;
using SketchFleets.Data;
using TMPro;
using UnityEngine;

namespace SketchFleets.Systems
{
    /// <summary>
    /// A class that manages the trade of goods
    /// </summary>
    public class ShopManager : MonoBehaviour
    {
        #region Private Fields

        [Header("Shop Parameters")]
        [SerializeField, RequiredField()]
        private ItemPool itemPool;
        [SerializeField, RequiredField()]
        private TMP_Text currentItemDescription;

        [Header("References")]
        [SerializeField, RequiredField()]
        private ShopEntry[] shopEntries;

        private ShopEntry activeEntry;

        #endregion

        #region MonoBehaviour Implementation

        private void Start()
        {
            PopulateShopEntries();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the currently active shop entry, displaying its description
        /// </summary>
        /// <param name="entry">The entry to be set as the active entry</param>
        public void SetActiveEntry(ShopEntry entry)
        {
            currentItemDescription.text = entry == null ? string.Empty : entry.Item.Description;
        }

        /// <summary>
        /// Buys the currently active entry
        /// </summary>
        public void BuyActiveEntry()
        {
            // TODO: Implement purchase
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populates all shop entries with items
        /// </summary>
        private void PopulateShopEntries()
        {
            for (int index = 0, upper = shopEntries.Length; index < upper; index++)
            {
                shopEntries[index].Manager = this;
                shopEntries[index].SetEntryItem(itemPool.PickRandom());
            }
        }

        #endregion
    }
}