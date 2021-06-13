using SketchFleets.Inventory;
using UnityEngine;

namespace SketchFleets.Data.Registers
{
    /// <summary>
    /// A register for ship attributes
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.shipRegisterOrder, fileName = CreateMenus.shipRegisterFileName,
        menuName = CreateMenus.shipRegisterMenuName)]
    public class ShipAttributesRegister : Register<ShipAttributes>
    {
        /// <summary>
        /// Pick random index from register
        /// </summary>
        /// <returns></returns>
        public override int PickRandom(int slot)
        {
            return default;
        }
    }
}