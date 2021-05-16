using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManyTools.UnityExtended.Poolable
{
    public class PoolManager : Singleton<PoolManager>
    {
        #region Private Fields

        [Header("Pooling Parameters")]
        [SerializeField]
        [Tooltip("The settings for when a new pool is created implicitly")]
        private PoolData defaultPoolSettings;

        [Header("Automatic Pool Culling")]
        [SerializeField]
        [Tooltip("How frequently should pools cull excess members")]
        [Range(1, 120)]
        private int cullInterval = 40;

        [Header("Adaptive Pool Limiting")]
        [SerializeField]
        [Tooltip("How frequently pools should adapt their limit")]
        [Range(1, 120)]
        private int adaptInterval = 40;

        [Header("Default Pools")]
        [SerializeField]
        [Tooltip("What pools should be preemptively created")]
        private UnityDictionary<GameObject, PoolData> defaultPools =
            new UnityDictionary<GameObject, PoolData>();

        private Dictionary<GameObject, Pool> pools = new Dictionary<GameObject, Pool>();
        private IEnumerator cullRoutine;
        private IEnumerator adaptRoutine;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            CreateDefaultPools();

            cullRoutine = CullExcessPoolables();
            StartCoroutine(cullRoutine);

            adaptRoutine = AdaptPoolLimits();
            StartCoroutine(adaptRoutine);
        }

        #endregion

        #region Private Methods

        private IEnumerator AdaptPoolLimits()
        {
            // Caches interval
            WaitForSecondsRealtime intervalCache = new WaitForSecondsRealtime(adaptInterval);
            yield return intervalCache;

            // Loops forever
            while (true)
            {
                // Iterates through every pool
                foreach (var poolByType in pools)
                {
                    // If the pool has no cull interval peak memory array yet, initialize one
                    if (poolByType.Value.CullIntervalPeaks == null)
                    {
                        poolByType.Value.CullIntervalPeaks = new int[poolByType.Value.Data.IntervalMemory];

                        for (int index = 0, upper = poolByType.Value.CullIntervalPeaks.Length;
                            index < upper;
                            index++)
                        {
                            poolByType.Value.CullIntervalPeaks[index] = poolByType.Value.Data.PoolLimit;
                        }
                    }

                    // Updates pool member usage
                    poolByType.Value.UpdateIntervalPeaks();

                    // Sets its limit to its average usage
                    poolByType.Value.Data.PoolLimit = Mathf.Max(poolByType.Value.AverageUsage,
                        poolByType.Value.Data.MinimumLimit);
                }

                yield return intervalCache;
            }
        }

        /// <summary>
        /// Removes submerged poolables in all pools over their limit
        /// </summary>
        private IEnumerator CullExcessPoolables()
        {
            // Caches interval
            WaitForSecondsRealtime intervalCache = new WaitForSecondsRealtime(cullInterval);
            yield return intervalCache;

            // Loops forever
            while (true)
            {
                // Iterates through every pool
                foreach (var poolByType in pools)
                {
                    // If the pool has no limit or isn't over its limit, skip the iteration
                    if (poolByType.Value.Poolables.Count <= poolByType.Value.Data.PoolLimit ||
                        poolByType.Value.Data.PoolLimit == 0) continue;

                    poolByType.Value.CullExcessPoolables();
                }

                yield return intervalCache;
            }
        }

        /// <summary>
        /// Creates all default pools
        /// </summary>
        private void CreateDefaultPools()
        {
            foreach (var defaultPool in defaultPools)
            {
                CreatePool(defaultPool.Key, defaultPool.Value);
            }

            defaultPools = null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates and adds a new pool to the manager
        /// </summary>
        /// <param name="poolType">The GameObject type of the pool</param>
        /// <param name="poolData">The settings of the pool</param>
        public Pool CreatePool(GameObject poolType, PoolData poolData)
        {
            Pool newPool = new Pool(poolType, poolData);
            pools.Add(poolType, newPool);

            return newPool;
        }

        /// <summary>
        /// Destroys an existing pool
        /// </summary>
        /// <param name="poolType">The pool's key</param>
        public void DestroyPool(GameObject poolType)
        {
            // If the pool doesn't exist, do nothing
            if (!pools.ContainsKey(poolType)) return;

            pools[poolType].Wipe(true);

            // Detaches the pool from the manager
            pools.Remove(poolType);
        }

        /// <summary>
        /// Requests a poolable object from a pool
        /// </summary>
        public PoolMember Request(GameObject memberType)
        {
            return pools.ContainsKey(memberType) ? pools[memberType].Take() : 
                CreatePool(memberType, defaultPoolSettings).Take();
        }

        #endregion
    }
}