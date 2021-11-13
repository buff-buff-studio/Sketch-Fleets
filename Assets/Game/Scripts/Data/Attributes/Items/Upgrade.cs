using ManyTools.Variables;
using UnityEngine;
using SketchFleets.Inventory;

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
        private FloatReference upgradeLifeIncrease = new FloatReference(0);

        [SerializeField]
        private FloatReference upgradeDamageIncrease = new FloatReference(0);

        [SerializeField]
        private FloatReference upgradeShieldIncrease = new FloatReference(0);

        [SerializeField]
        private FloatReference upgradeSpeedIncrease = new FloatReference(0);
        
        [SerializeField]
        private IntReference upgradeColorInventorySize = new IntReference(0);

        [SerializeField]
        private CodexEntryRarity codexEntryRarity;
        #endregion

        #region Properties
        public CodexEntryRarity CodexEntryRarity => codexEntryRarity;

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
        public IntReference UpgradeColorInventorySize
        {
            get => upgradeColorInventorySize;
            set => upgradeColorInventorySize = value;
        }

        #endregion
    }
}