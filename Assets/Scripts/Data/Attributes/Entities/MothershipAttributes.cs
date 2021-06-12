using System.Collections.Generic;
using ManyTools.UnityExtended;
using SketchFleets.Inventory;
using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that holds attributes relative to the mothership
    /// </summary>
    [CreateAssetMenu(fileName = CreateMenus.motherShipAttributesFileName, 
        menuName = CreateMenus.motherShipAttributesMenuName, order = CreateMenus.motherShipAttributesOrder)]
    public sealed class MothershipAttributes : ShipAttributes
    {
        #region Private Fields

        [Header("Mothership Attributes")]
        [SerializeField]
        [Tooltip("How long is the regenerate ability cooldown")]
        private FloatReference regenerateCooldown = new FloatReference(30f);
        [SerializeField]
        [Tooltip("The interval between each self-destruct of ally ships when using the regenerate ability")]
        private FloatReference regenerateKillInterval = new FloatReference(0.15f);

        [SerializeField]
        protected ShopObjectRegister itemRegister;
        [SerializeField]
        protected ShopObjectRegister upgradeRegister;
        #endregion

        #region Properties

        public FloatReference RegenerateCooldown => regenerateCooldown;

        public FloatReference RegenerateKillInterval => regenerateKillInterval;

        public ShopObjectRegister ItemRegister => itemRegister;

        public ShopObjectRegister UpgradeRegister => upgradeRegister;

        #endregion

    }
}