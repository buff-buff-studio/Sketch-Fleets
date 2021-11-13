using System;
using System.Collections.Generic;
using ManyTools.UnityExtended;
using UnityEngine;

namespace ManyTools.UnityExtended
{
    /// <summary>
    ///     A class that mimics async behavior without actually using async
    /// </summary>
    public sealed class DelayProvider : Singleton<DelayProvider>
    {
        #region Private Fields

        private readonly List<Action> actions = new List<Action>();
        private readonly List<float> times = new List<float>();
        private readonly List<int> instanceIDs = new List<int>();

        #endregion

        #region Unity Callbacks

        private void Update()
        {
            InvokeTimedActions();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Does an action with a delay
        /// </summary>
        /// <param name="action">The action to do</param>
        /// <param name="delayInSeconds">The delay of the action</param>
        /// <param name="instanceID">The instance ID linked with the action</param>
        public void DoDelayed(Action action, float delayInSeconds, int instanceID)
        {
            actions.Add(action);
            times.Add(Time.time + delayInSeconds);
            instanceIDs.Add(instanceID);
        }

        /// <summary>
        /// Cancels a delayed action
        /// </summary>
        /// <param name="instanceID">The instance ID linked with the action</param>
        public void CancelDoDelayed(int instanceID)
        {
            for (int index = instanceIDs.Count - 1; index >= 0; index--)
            {
                if (instanceIDs[index] != instanceID) continue;
                RemoveAction(index);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Invokes current timed actions
        /// </summary>
        private void InvokeTimedActions()
        {
            int actionsCount = actions.Count;
            if (actionsCount <= 0) return;
            
            for (int index = actionsCount - 1; index >= 0; index--)
            {
                if (!(times[index] <= Time.time)) continue;
                actions[index]?.Invoke();

                RemoveAction(index);
            }
        }

        /// <summary>
        ///     Removes an action from the list
        /// </summary>
        /// <param name="index">The index of the action to remove</param>
        private void RemoveAction(int index)
        {
            if (index >= instanceIDs.Count || index < 0) return;
            
            times.RemoveAt(index);
            actions.RemoveAt(index);
            instanceIDs.RemoveAt(index);
        }

        #endregion
    }
}