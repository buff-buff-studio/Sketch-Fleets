using System;
using UnityEngine;

namespace ManyTools.UnityExtended.Poolable
{
    /// <summary>
    /// A struct that holds data for display in the editor about default pools
    /// </summary>
    [Serializable]
    public class PoolData
    {
        [Header("Pool Settings")]
        [Tooltip("The ideal limit of members in the pool.")]
        public int PoolLimit;
        [Tooltip("How many objects to preemptively add to the pool.")]
        public int PreFillAmount;
        [Tooltip("Whether the pool should periodically cull members over the pool's limit")]
        public bool CullExcessMembers;
        [Tooltip("Whether the pool's limit should change periodically to reflect its usage")]
        public bool AdaptivePoolLimits;
        [Tooltip("How many intervals will be recorded to calculate the adaptive limit")]
        public int IntervalMemory;
        [Tooltip("The minimum pool limit. The adaptive pool limit will never go below this point")]
        public int MinimumLimit;
            
        public PoolData(int poolLimit = 0, int preFillAmount = 0, bool cullExcessMembers = false, bool 
        adaptivePoolLimits = false, int minimumLimit = 0, int intervalMemory = 0)
        {
            PoolLimit = poolLimit;
            PreFillAmount = preFillAmount;
            CullExcessMembers = cullExcessMembers;
            AdaptivePoolLimits = adaptivePoolLimits;
            MinimumLimit = minimumLimit;
            IntervalMemory = intervalMemory;
        }
    }
}