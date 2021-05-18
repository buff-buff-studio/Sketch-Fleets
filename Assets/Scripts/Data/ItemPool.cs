using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that contains data about a pool of items
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.itemPoolOrder, fileName = CreateMenus.itemPoolFileName, 
        menuName = CreateMenus.itemPoolMenuName)]
    public sealed class ItemPool : ScriptableObject
    {
        #region Private Fields

        [SerializeField] 
        private List<ItemAttributes> items;

        #endregion

        #region Properties

        public List<ItemAttributes> Items
        {
            get => items;
            set => items = value;
        }

        #endregion

        #region Public Methods

        public ItemAttributes PickRandom()
        {
            int totalWeight = 0;
            
            for (int index = 0, upper = items.Count; index < upper; index++)
            {
                totalWeight += items[index].Rarity;
            }
            
            int randomPick = Random.Range(0, totalWeight);

            for (int index = 0, upper = items.Count; index < upper; index++)
            {
                if (randomPick <= items[index].Rarity)
                {
                    return items[index];
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