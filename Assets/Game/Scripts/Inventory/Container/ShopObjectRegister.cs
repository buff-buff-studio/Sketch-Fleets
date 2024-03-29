using System;
using UnityEngine;
using SketchFleets.Data;

namespace SketchFleets.Inventory
{
    /// <summary>
    /// Class used to register items
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.itemRegisterOrder, fileName = CreateMenus.itemRegisterFileName,
        menuName = CreateMenus.itemRegisterMenuName)]
    public class ShopObjectRegister : Register<ShopObject>
    {
        #region Public Fields
        public bool isUpgrade = false;
        #endregion

        #region Public Methods        
        public override int PickRandom(int slot)
        {
            if(isUpgrade)
            {
                return slot;
            }

            int totalWeight = 0;

            for (int index = 0, upper = items.Length; index < upper; index++)
            {
                totalWeight += items[index].Rarity;
            }

            int randomPick = UnityEngine.Random.Range(0, totalWeight);

            for (int index = 0, upper = items.Length; index < upper; index++)
            {
                if (randomPick <= items[index].Rarity)
                {
                    return index;
                }
                else
                {
                    randomPick -= items[index].Rarity;
                }
            }

            throw new IndexOutOfRangeException("Drew outside the bounds of the item pool");
        }
        #endregion
    }
}
