using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that contains the attributes relating to an item
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.upgradeAttributesOrder, fileName = CreateMenus.upgradeAttributesFileName, 
        menuName = CreateMenus.upgradeAttributesMenuName)]
    public class Upgrade : ShopObject
    {
        #region Private Fields
        [SerializeField]
        private IntReference itemCostIncreasePerLevel = new IntReference(100);
        #endregion

        public IntReference ItemCostIncreasePerLevel
        {
            get => itemCostIncreasePerLevel;
            set => itemCostIncreasePerLevel = value;
        }
    }
}