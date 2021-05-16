using System.Collections.Generic;
using ManyTools.UnityExtended;
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

        #endregion

        #region Properties

        public FloatReference RegenerateCooldown => regenerateCooldown;

        #endregion

    }
}