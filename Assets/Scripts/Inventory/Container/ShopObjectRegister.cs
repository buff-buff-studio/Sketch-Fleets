using System;
using UnityEngine;
using SketchFleets.Data;

namespace SketchFleets.Inventory
{
    [CreateAssetMenu(order = CreateMenus.itemRegisterOrder, fileName = CreateMenus.itemRegisterFileName,
        menuName = CreateMenus.itemRegisterMenuName)]
    /// <summary>
    /// Class used to register items
    /// </summary>
    public class ShopObjectRegister : Register<ShopObject>
    {
        #region Public Methods
        
        public override int PickRandom()
        {
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
