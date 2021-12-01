using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets.Systems.Tutorial
{
    /// <summary>
    /// A class that manages a single tutorial
    /// </summary>
    public sealed class Tutorial : MonoBehaviour
    {
        [SerializeField]
        private List<TutorialStep> steps = new List<TutorialStep>();

        #region Unity Callbacks

        

        #endregion

        #region Private Methods

        private void OnEnable()
        {
        }


        /// <summary>
        /// Begins a step by showing its popup
        /// </summary>
        /// <param name="step">The step to begin</param>
        private void BeginStep(TutorialStep step)
        {
            if (!HasPreviousStepBeenCompleted(step) || IsStepOld(step)) return;
            step.Begin();
        }

        /// <summary>
        /// Checks whether a given step is old (Has already begun or ended)
        /// </summary>
        /// <param name="step">The step to check</param>
        /// <returns>Whether a given step is old (Has already begun or ended)</returns>
        private static bool IsStepOld(TutorialStep step)
        {
            return !step.IsNew;
        }
        
        /// <summary>
        /// Checks whether the previous step has been completed
        /// </summary>
        /// <param name="step">The step to check</param>
        /// <returns>Whether the previous step has been completed</returns>
        private bool HasPreviousStepBeenCompleted(TutorialStep step)
        {
            int stepIndex = steps.IndexOf(step);
            return stepIndex == 0 || steps[stepIndex - 1].IsComplete;
        }

        #endregion
    }
}