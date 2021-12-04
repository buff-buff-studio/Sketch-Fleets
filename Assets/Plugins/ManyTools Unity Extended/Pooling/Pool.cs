using System.Collections.Generic;
using UnityEngine;

namespace ManyTools.UnityExtended.Poolable
{
    public class Pool
    {
        #region Private Fields

        private GameObject template;
        private PoolData poolData;
        private List<PoolMember> poolables = new List<PoolMember>();
        
        // Adaptive limiting
        private int[] cullIntervalPeaks;
        private int currentEmerged;
        private int peakEmerged;

        #endregion

        #region Properties

        public List<PoolMember> Poolables => poolables;

        public int[] CullIntervalPeaks
        {
            get => cullIntervalPeaks;
            set => cullIntervalPeaks = value;
        }

        public int AverageUsage
        {
            get
            {
                if (cullIntervalPeaks == null) return poolables.Count;
                if (cullIntervalPeaks.Length == 0) return poolables.Count;

                int intervalTotal = 0;
                int upper = cullIntervalPeaks.Length;
                
                for (int index = 0; index < upper; index++)
                {
                    intervalTotal += cullIntervalPeaks[index];
                }

                // ReSharper disable once IntDivisionByZero
                return intervalTotal / upper;
            }
        }

        public int CurrentEmerged
        {
            get => currentEmerged;
            set
            {
                if (currentEmerged > peakEmerged)
                {
                    peakEmerged = currentEmerged;
                }
                
                currentEmerged = value;
            } 
        }

        public PoolData Data
        {
            get => poolData;
            set => poolData = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new pool with no limit
        /// </summary>
        /// <param name="template">The template GameObject of the pool</param>
        /// <param name="poolData">Data about the pool's setup</param>
        public Pool(GameObject template, PoolData poolData)
        {
            this.template = template;
            this.poolData = poolData;
            Add(poolData.PreFillAmount);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Removes an object from the pool
        /// </summary>
        /// <param name="indexInPool">The index of the object in the pool</param>
        /// <param name="destroy">Whether the object should be destroyed after removal</param>
        public void DePool(int indexInPool, bool destroy = true)
        {
            if (destroy)
            {
                Object.Destroy(poolables[indexInPool].gameObject);
            }

            poolables.RemoveAt(indexInPool);
        }

        /// <summary>
        /// Removes submerged poolables until the pool is at or under its limit
        /// </summary>
        public void CullExcessPoolables()
        {
            // De-Pool members until the pool is at or under its limit
            for (int index = Poolables.Count - 1; index >= 0; index--)
            {
                // Remove the poolable only if he is submerged
                if (Poolables[index].IsSubmerged)
                {
                    DePool(index);
                }

                // Check if the pool is at or under its limit. If so, end the loop here
                if (Poolables.Count <= poolData.PoolLimit)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Removes all objects from the pool
        /// </summary>
        /// <param name="destroy">Whether to destroy pool objects before removal</param>
        public void Wipe(bool destroy)
        {
            for (int index = Poolables.Count - 1; index >= 0; index--)
            {
                DePool(index, destroy);
            }
        }

        /// <summary>
        /// Mass adds objects to the pool
        /// </summary>
        /// <param name="amount">The amount of objects to add</param>
        public void Add(int amount)
        {
            for (int index = 0; index < amount; index++)
            {
                // Instantiates the object
                GameObject poolable = GameObject.Instantiate(template, Vector3.zero,
                    Quaternion.identity);

                // Gets its IPoolable component and immediately submerges it
                PoolMember poolMember = poolable.GetComponent<PoolMember>();
                poolMember.MotherPool = this;
                poolMember.gameObject.SetActive(false);
                
                // Adds it to the list
                poolables.Add(poolMember);
            }
        }

        /// <summary>
        /// Takes a submerged poolable from the pool
        /// </summary>
        /// <returns>The submerged poolable</returns>
        public PoolMember Take()
        {
            // Tries getting the earliest submerged poolable
            for (int index = 0, upper = Poolables.Count; index < upper; index++)
            {
                if (poolables[index].IsSubmerged)
                {
                    return poolables[index];
                }
            }

            // If none was available, create a new one and return it
            Add(1);
            return poolables[poolables.Count - 1];
        }

        /// <summary>
        /// Samples, updates and culls the interval peak array
        /// </summary>
        public void UpdateIntervalPeaks()
        {
            // If adaptive limiting features are turned off, skip
            if (cullIntervalPeaks == null) return;
            if (cullIntervalPeaks.Length == 0) return;
            
            // Remove oldest peak and add newest peak
            for (int index = 0, upper = cullIntervalPeaks.Length; index < upper; index++)
            {
                if (index + 1 < upper)
                {
                    cullIntervalPeaks[index] = cullIntervalPeaks[index + 1];
                }
                else
                {
                    cullIntervalPeaks[index] = peakEmerged;
                }
            }

            // Reset peak emerged
            peakEmerged = currentEmerged;
        }

        #endregion
    }
}