using System.Collections.Generic;
using UnityEngine;

namespace ManyTools.UnityExtended.WeightedPool
{
    /// <summary>
    /// A pool of objects with weights
    /// </summary>
    /// <typeparam name="T">The type of the object to draw from</typeparam>
    public class WeightedPool<T> : ScriptableObject
    {
        #region Private Fields

        [Header("Pool Config")]
        [SerializeField]
        [Tooltip("The object and its weight")]
        private List<WeightedPoolEntry> pool = new List<WeightedPoolEntry>();

        #endregion

        #region Properties

        public List<WeightedPoolEntry> Pool => pool;

        #endregion

        #region Public Methods

        /// <summary>
        /// Draws an item from the weighted pool
        /// </summary>
        /// <returns>The drawn item</returns>
        public T Draw()
        {
            int pickedWeight = Random.Range(0, GetTotalWeight());

            for (int index = 0, upper = Pool.Count; index < upper; index++)
            {
                if (pickedWeight < Pool[index].weight)
                {
                    return Pool[index].item;
                }
                else
                {
                    pickedWeight -= Pool[index].weight;
                }
            }

            Debug.LogError("The weighed random algorithm failed! This should never happen." +
                           " Maybe the pool is empty?");
            return default;
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the total weight of the pool
        /// </summary>
        /// <returns>The total weight of the pool</returns>
        private int GetTotalWeight()
        {
            int totalWeight = 0;

            for (int index = 0, upper = Pool.Count; index < upper; index++)
            {
                totalWeight += Pool[index].weight;
            }

            return totalWeight;
        }

        #endregion

        #region PoolEntry Class

        /// <summary>
        /// A class that contains a specific weighed pool entry
        /// </summary>
        [System.Serializable]
        public struct WeightedPoolEntry
        {
            [SerializeField, Tooltip("The item to be drawn")]
            public T item;
            [SerializeField, Tooltip("The item's weight")]
            public int weight;
        }

        #endregion
    }
}