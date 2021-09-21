using ManyTools.UnityExtended.Editor;
using SketchFleets.Entities;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    ///     A class that holds data about a specific formation
    /// </summary>
    [CreateAssetMenu(fileName = CreateMenus.shipFormationFileName, menuName = CreateMenus.shipFormationMenuName,
        order = CreateMenus.shipFormationOrder)]
    public sealed class ShipFormation : ScriptableObject
    {
        #region Private Fields

        [Header("Ship")]
        [SerializeField]
        [RequiredField]
        private ShipAttributes[] ships = new ShipAttributes[1];

        [Header("Placement")]
        [SerializeField]
        [RequiredField]
        private GameObject formationObject;

        private Formation formation;

        #endregion

        #region Properties

        public ShipAttributes[] Ships => ships;
        public Formation Formation => formation;

        #endregion

        #region Unity Callbacks

        private void OnValidate()
        {
            ValidateFormationObject();
            GenerateShipAttributeArray();
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Generates a ship attribute array matching the given formation
        /// </summary>
        private void GenerateShipAttributeArray()
        {
            if (formation == null) return;

            ShipAttributes[] attributeCache = ships;
            ships = new ShipAttributes[formation.FormationPoints.Length];

            for (int index = 0, max = Mathf.Min(attributeCache.Length, ships.Length); index < max; index++)
            {
                ships[index] = attributeCache[index];
            }
        }
        
        /// <summary>
        ///     Validates the input formation object
        /// </summary>
        private void ValidateFormationObject()
        {
            if (formationObject.TryGetComponent(out formation)) return;
            formationObject = null;
            Debug.LogError("The formation object must have a Formation component at the top level");
        }

        #endregion
    }
}