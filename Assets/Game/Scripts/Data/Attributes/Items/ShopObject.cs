using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that contains the attributes relating to an item
    /// </summary>
    public class ShopObject : ScriptableObject
    {
        #region Private Fields
        [SerializeField]
        private string unlocalized_name;
        [SerializeField, Tooltip("Higher values mean the item is more common, lower values mean the item" +
                                 "is rare. 0 makes so that the item never spawns.")]
        private IntReference rarity = new IntReference(10);
        [SerializeField]
        private IntReference itemCost = new IntReference(100);

        [SerializeField]
        private IntReference itemCostIncreasePerUnit = new IntReference(0);

        [SerializeField]
        private IntReference itemAmountLimit = new IntReference(0);

        [SerializeField]
        private Sprite icon;
        #endregion

        #region Properties
        public IntReference ItemCost
        {
            get => itemCost;
            set => itemCost = value;
        }

        public IntReference ItemCostIncreasePerUnit
        {
            get => itemCostIncreasePerUnit;
            set => itemCostIncreasePerUnit = value;
        }

        public IntReference ItemAmountLimit
        {
            get => itemAmountLimit;
            set => itemAmountLimit = value;
        }
        
        public string UnlocalizedName
        {
            get => unlocalized_name;
            set => unlocalized_name = value;
        }

        public IntReference Rarity
        {
            get => rarity;
            set => rarity = value;
        }

        public Sprite Icon
        {
            get => icon;
            set => icon = value;
        }
        #endregion
    }
}