using System;
using ManyTools.UnityExtended.Editor;
using SketchFleets.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SketchFleets.Systems
{
    /// <summary>
    /// A class that contains information about a given entry
    /// </summary>
    [Serializable]
    public class ShopEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Private Fields

        [Header("Entry Parameters")]
        [SerializeField, RequiredField()]
        private Image icon;
        [SerializeField, RequiredField()]
        private TMP_Text title;
        [SerializeField, RequiredField()]
        private TMP_Text price;
        
        private ShopManager manager;
        private ItemAttributes item;

        #endregion

        #region Properties

        public Sprite Icon
        {
            get => icon.sprite;
            set => icon.sprite = value;
        }

        public string Title
        {
            get => title.text;
            set => title.text = value;
        }

        public string Price
        {
            get => price.text;
            set => price.text = value;
        }

        public ItemAttributes Item
        {
            get => item;
            set => item = value;
        }

        public ShopManager Manager
        {
            get => manager;
            set => manager = value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the entry's item
        /// </summary>
        /// <param name="entryItem">The item to set this entry to</param>
        public void SetEntryItem(ItemAttributes entryItem)
        {
            // Updates internal item
            Item = entryItem;
            
            // Updates necessary fields
            icon.sprite = Item.Icon;
            title.text = Item.Name;
            price.text = Item.Description;
        }

        #endregion
        
        #region Private Methods


        #endregion

        #region IPointer Implementations

        public void OnPointerEnter(PointerEventData eventData)
        {
            manager.SetActiveEntry(this);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            manager.SetActiveEntry(null);
        }

        #endregion
    }
}