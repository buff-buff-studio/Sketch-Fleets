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
        private FloatReference itemCostIncreasePerLevel = new FloatReference(100);

        [SerializeField]
        private FloatReference upgradeLifeIncrease = new FloatReference(0);

        [SerializeField]
        private FloatReference upgradeDamageIncrease = new FloatReference(0);

        [SerializeField]
        private FloatReference upgradeShieldIncrease = new FloatReference(0);

        [SerializeField]
        private FloatReference upgradeSpeedIncrease = new FloatReference(0);
        #endregion

        #region Properties
        public FloatReference ItemCostIncreasePerLevel
        {
            get => itemCostIncreasePerLevel;
            set => itemCostIncreasePerLevel = value;
        }

        public FloatReference UpgradeLifeIncrease
        {
            get => upgradeLifeIncrease;
            set => upgradeLifeIncrease = value;
        }

        public FloatReference UpgradeDamageIncrease
        {
            get => upgradeDamageIncrease;
            set => upgradeDamageIncrease = value;
        }

        public FloatReference UpgradeShieldIncrease
        {
            get => upgradeShieldIncrease;
            set => upgradeShieldIncrease = value;
        }

        public FloatReference UpgradeSpeedIncrease
        {
            get => upgradeSpeedIncrease;
            set => upgradeSpeedIncrease = value;
        }

        #endregion
    }
}