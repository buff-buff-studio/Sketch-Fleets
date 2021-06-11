using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that contains the attributes relating to an item
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.itemAttributesOrder, fileName = CreateMenus.itemAttributesFileName, 
        menuName = CreateMenus.itemAttributesMenuName)]
    public class Item : ShopObject
    {
        #region Private Fields
        [Header("Item Attributes")]
        [SerializeField]
        private int id;
        [SerializeField]
        private ItemEffect effect;
        [SerializeField]
        private bool isConsumable;
        
        #endregion

        #region Properties

        public ItemEffect Effect
        {
            get => effect;
            set => effect = value;
        }

        public bool IsConsumable
        {
            get => isConsumable;
            set => isConsumable = value;
        }
        #endregion
    }
}