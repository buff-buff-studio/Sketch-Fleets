using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that contains the attributes relating to an item
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.itemAttributesOrder, fileName = CreateMenus.itemAttributesFileName, 
        menuName = CreateMenus.itemAttributesMenuName)]
    public class ItemAttributes : Attributes
    {
        #region Private Fields

        [Header("Item Attributes")]
        [SerializeField]
        private Sprite icon;
        [SerializeField]
        private IntReference itemCost = new IntReference(100);
        [SerializeField, Tooltip("Higher values mean the item is more common, lower values mean the item" +
                                 "is rare. 0 makes so that the item never spawns.")]
        private IntReference rarity = new IntReference(10);
        [SerializeField]
        private ItemEffect effect;
        
        #endregion

        #region Properties

        public IntReference ItemCost
        {
            get => itemCost;
            set => itemCost = value;
        }

        public Sprite Icon
        {
            get => icon;
            set => icon = value;
        }

        public IntReference Rarity
        {
            get => rarity;
            set => rarity = value;
        }

        public ItemEffect Effect
        {
            get => effect;
            set => effect = value;
        }

        #endregion
    }
}