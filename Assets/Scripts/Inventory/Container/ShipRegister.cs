using System;
using UnityEngine;
using SketchFleets.Data;

namespace SketchFleets.Inventory
{
    [CreateAssetMenu(order = CreateMenus.shipRegisterOrder, fileName = CreateMenus.shipRegisterFileName,
        menuName = CreateMenus.shipRegisterMenuName)]
    public class ShipRegister : Register<ShipAttributes>
    {
        #region Public Methods
        public override int PickRandom()
        {
            return UnityEngine.Random.Range(0,items.Length);
        }
        #endregion
    }
}
