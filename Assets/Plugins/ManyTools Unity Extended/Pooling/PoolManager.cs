using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManyTools.UnityExtended.Poolable
{
    public class PoolManager : Singleton<PoolManager>
    {
        #region Private Fields

        [Header("Automatic Pool Culling")]
        [SerializeField]
        [Tooltip("Whether pools should destroy members excess members when above their limit")]
        private bool automaticCulling = true;
        [SerializeField]
        [Tooltip("How frequently should pools cull excess members")]
        [Range(1, 120)]
        private int cullInterval = 40;
        [SerializeField]
        [Tooltip("When a new pool is created automatically, what should be its limit? Set 0 for no limit.")]
        private int defaultPoolLimit = 0;


        [Header("Adaptive Pool Limits")]
        [SerializeField]
        [Tooltip("Whether pool limits should automatically be updated to reflect recent usage")]
        private bool adaptivePoolLimits = true;
        [SerializeField]
        [Tooltip("How frequently pools should adapt their limit")]
        [Range(1, 120)]
        private int adaptInterval = 40;
        [SerializeField]
        [Tooltip("How many cull intervals pools should remember to define their adaptive limit")]
        private int intervalMemory = 3;

        private Dictionary<GameObject, Pool> pools = new Dictionary<GameObject, Pool>();
        private IEnumerator cullRoutine;
        private IEnumerator adaptRoutine;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            if (automaticCulling)
            {
                cullRoutine = CullExcessPoolables();
                StartCoroutine(cullRoutine);
            }

            if (adaptivePoolLimits && automaticCulling)
            {
                adaptRoutine = AdaptPoolLimits();
                StartCoroutine(adaptRoutine);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator AdaptPoolLimits()
        {
            // Caches interval
            WaitForSecondsRealtime intervalCache = new WaitForSecondsRealtime(adaptInterval);

            // Loops forever
            while (true)
            {
                // Iterates through every pool
                foreach (var poolByType in pools)
                {
                    // If the pool has no cull interval peak memory array yet, initialize one
                    if (poolByType.Value.CullIntervalPeaks == null)
                    {
                        poolByType.Value.CullIntervalPeaks = new int[intervalMemory];

                        for (int index = 0, upper = poolByType.Value.CullIntervalPeaks.Length;
                            index < upper;
                            index++)
                        {
                            poolByType.Value.CullIntervalPeaks[index] = poolByType.Value.PoolLimit;
                        }
                    }

                    // Updates pool member usage
                    poolByType.Value.UpdateIntervalPeaks();

                    // Sets its limit to its average usage
                    poolByType.Value.PoolLimit = poolByType.Value.AverageUsage;
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

            // Loops forever
            while (true)
            {
                // Iterates through every pool
                foreach (var poolByType in pools)
                {
                    // If the pool has no limit or isn't over its limit, skip the iteration
                    if (poolByType.Value.Poolables.Count <= poolByType.Value.PoolLimit ||
                        poolByType.Value.PoolLimit == 0) continue;

                    poolByType.Value.CullExcessPoolables();
                }

                yield return intervalCache;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates and adds a new pool to the manager
        /// </summary>
        /// <param name="poolType">The GameObject type of the pool</param>
        /// <param name="limit">The ideal limit of poolables in the pool</param>
        public Pool CreatePool(GameObject poolType, int limit)
        {
            Pool newPool = new Pool(poolType, limit);
            pools.Add(poolType, newPool);

            return newPool;
        }

        /// <summary>
        /// Creates and adds a new pool to the manager
        /// </summary>
        /// <param name="poolType">The GameObject type of the pool</param>
        /// <param name="limit">The ideal limit of poolables in the pool</param>
        /// <param name="preFillAmount">The amount of objects to preemptively add to the pool</param>
        public Pool CreatePool(GameObject poolType, int limit, int preFillAmount)
        {
            Pool newPool = new Pool(poolType, limit, preFillAmount);
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
            return pools.ContainsKey(memberType) ? 
                pools[memberType].Take() : CreatePool(memberType, defaultPoolLimit).Take();
        }

        #endregion
    }
}