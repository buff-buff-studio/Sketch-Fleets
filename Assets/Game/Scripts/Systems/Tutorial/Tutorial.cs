using System.Collections.Generic;
using ManyTools.UnityExtended.Editor;
using SketchFleets.ProfileSystem;
using UnityEngine;

namespace SketchFleets.Systems.Tutorial
{
    /// <summary>
    /// A class that manages a single tutorial
    /// </summary>
    public sealed class Tutorial : MonoBehaviour
    {
        #region Private Fields

        [SerializeField]
        private List<TutorialStep> steps = new List<TutorialStep>();

        [SerializeField]
        private bool autoBeginFirstStep = false;

        private int currentStepIndex = 0;

        #endregion

        #region Properties
        
        private TutorialStep CurrentStep => steps[currentStepIndex];

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            if (Profile.Data.Tutorials.Completed.Contains(name))
            {
                return;
            }
            
            if (autoBeginFirstStep)
            {
                BeginStep();
            }
            else
            {
                SubscribeCurrentStepToTrigger();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Subscribes a step to a trigger
        /// </summary>
        private void SubscribeCurrentStepToTrigger()
        {
            CurrentStep.Trigger.AddListener(BeginStep);
        }

        /// <summary>
        /// Begins a step by showing its popup
        /// </summary>
        private void BeginStep()
        {
            if (IsStepOld(CurrentStep)) return;
            CurrentStep.Begin();

            ProgressSteps();
        }

        /// <summary>
        /// Moves to the next step or ends the tutorial
        /// </summary>
        private void ProgressSteps()
        {
            UnsubscribeCurrentStepFromTrigger();

            if (currentStepIndex + 1 >= steps.Count)
            {
                CompleteTutorial();
                return;
            }

            currentStepIndex++;
            SubscribeCurrentStepToTrigger();
        }

        /// <summary>
        /// Unsubscribes the current step from its trigger
        /// </summary>
        private void UnsubscribeCurrentStepFromTrigger()
        {
            if (CurrentStep.Trigger == null) return;
            CurrentStep.Trigger.RemoveListener(BeginStep);
        }

        /// <summary>
        /// Marks the tutorial as complete
        /// </summary>
        private void CompleteTutorial()
        {
            Profile.Data.Tutorials.Completed.Add(name);
            Profile.SaveProfile((data) => { });
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

        #endregion
    }
}