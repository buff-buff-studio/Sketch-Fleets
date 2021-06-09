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

        [SerializeField]
        private IntReference upgradeLifeIncrease = new IntReference(0);

        [SerializeField]
        private IntReference upgradeDamageIncrease = new IntReference(0);

        [SerializeField]
        private IntReference upgradeShieldIncrease = new IntReference(0);

        [SerializeField]
        private IntReference upgradeSpeedIncrease = new IntReference(0);
        #endregion

        #region Properties
        public IntReference ItemCostIncreasePerLevel
        {
            get => itemCostIncreasePerLevel;
            set => itemCostIncreasePerLevel = value;
        }

        public IntReference UpgradeLifeIncrease
        {
            get => upgradeLifeIncrease;
            set => upgradeLifeIncrease = value;
        }

        public IntReference UpgradeDamageIncrease
        {
            get => upgradeDamageIncrease;
            set => upgradeDamageIncrease = value;
        }

        public IntReference UpgradeShieldIncrease
        {
            get => upgradeShieldIncrease;
            set => upgradeShieldIncrease = value;
        }

        public IntReference UpgradeSpeedIncrease
        {
            get => upgradeSpeedIncrease;
            set => upgradeSpeedIncrease = value;
        }

        #endregion
    }
}