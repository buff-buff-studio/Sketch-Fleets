using System;
using System.Collections.Generic;
using UnityEngine;

namespace ManyTools.UnityExtended
{
    /// <summary>
    ///     A class that mimics async behavior without actually using async
    /// </summary>
    public sealed class DelayProvider : Singleton<DelayProvider>
    {
        #region Private Fields

        private readonly List<DelayedAction> delayedActions = new List<DelayedAction>();
        private readonly HashSet<int> cancelRequests = new HashSet<int>();

        #endregion

        #region Unity Callbacks

        private void Update()
        {
            PruneCanceledActions();
            InvokeTimedActions();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Does an action with a delay
        /// </summary>
        /// <param name="action">The action to do</param>
        /// <param name="delayInSeconds">The delay of the action</param>
        /// <param name="instanceID">The ID of the object that owns the action</param>
        public void DoDelayed(Action action, float delayInSeconds, int instanceID)
        {
            delayedActions.Add(new DelayedAction(action, delayInSeconds, instanceID));
        }

        /// <summary>
        /// Cancels a delayed action
        /// </summary>
        /// <param name="owner">The ID of the object that owns the action</param>
        public void CancelDoDelayed(int owner)
        {
            cancelRequests.Add(owner);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Removes all cancelled actions from the list
        /// </summary>
        private void PruneCanceledActions()
        {
            if (cancelRequests.Count == 0) return;
            delayedActions.RemoveAll(delayedAction => cancelRequests.Contains(delayedAction.OwnerID));
            cancelRequests.Clear();
        }
        
        /// <summary>
        ///     Invokes current timed actions
        /// </summary>
        private void InvokeTimedActions()
        {
            int actionsCount = delayedActions.Count;
            if (actionsCount <= 0) return;
            
            for (int index = actionsCount - 1; index >= 0; index--)
            {
                if (!(delayedActions[index].ExecutionTime <= Time.time)) continue;
                delayedActions[index].Action?.Invoke();

                RemoveAction(index);
            }
        }

        /// <summary>
        ///     Removes an action from the list
        /// </summary>
        /// <param name="index">The index of the action to remove</param>
        private void RemoveAction(int index)
        {
            if (index >= delayedActions.Count || index < 0) return;
            delayedActions.RemoveAt(index);
        }

        #endregion

        #region Nested Structs

        /// <summary>
        /// A struct that contains information about a delayed action
        /// </summary>
        private readonly struct DelayedAction
        {
            public readonly Action Action;
            public readonly float ExecutionTime;
            public readonly int OwnerID;

            public DelayedAction(Action action, float delay, int ownerID)
            {
                Action = action;
                ExecutionTime = Time.time + delay;
                OwnerID = ownerID;
            }
        }

        #endregion
    }
}