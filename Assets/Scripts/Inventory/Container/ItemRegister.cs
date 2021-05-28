using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.Data;

namespace SketchFleets.Inventory
{
    [CreateAssetMenu(order = CreateMenus.itemRegisterOrder, fileName = CreateMenus.itemRegisterFileName,
        menuName = CreateMenus.itemRegisterMenuName)]
    /// <summary>
    /// Class used to register items
    /// </summary>
    public class ItemRegister : ScriptableObject
    {
        #region Public Fields
        [SerializeField]
        public Item[] items;
        #endregion

        #region Public Methods
        /// <summary>
        /// Pick random item from register
        /// </summary>
        /// <returns></returns>
        public int PickRandom()
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
