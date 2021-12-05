using System.Collections.Generic;
using System.Linq;
using ManyTools.Variables;
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

        [SerializeField]
        private BoolReference nextStepIsBlocked;

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

            InjectTutorialReferences();
            
            if (autoBeginFirstStep)
            {
                BeginStep();
            }
            else
            {
                SubscribeCurrentStepToTrigger();
            }
        }

        private void OnDestroy()
        {
            foreach (TutorialStep step in steps.Where(step => step != null && step.Trigger != null))
            {
                step.Trigger.RemoveListener(BeginStep);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Ends the current step and begins the next one
        /// </summary>
        public void StepForward()
        {
            UnsubscribeCurrentStepFromTrigger();

            if (currentStepIndex + 1 >= steps.Count)
            {
                CompleteTutorial();
                return;
            }

            CurrentStep.Begin();
            currentStepIndex++;
            SubscribeCurrentStepToTrigger();
        }

        /// <summary>
        /// Marks the tutorial as complete
        /// </summary>
        public void CompleteTutorial()
        {
            Profile.Data.Tutorials.Completed.Add(name);
            Profile.SaveProfile((data) => { });
        }
        
        /// <summary>
        /// Unblocks the next step
        /// </summary>
        public void UnblockNextStep()
        {
            Debug.Log("Unblocking next step");
            nextStepIsBlocked.Value = false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Injects references to the tutorial system into all steps
        /// </summary>
        private void InjectTutorialReferences()
        {
            foreach (TutorialStep step in steps)
            {
                step.Tutorial = this;
            }
        }
        
        /// <summary>
        /// Subscribes a step to a trigger
        /// </summary>
        private void SubscribeCurrentStepToTrigger()
        {
            if (CurrentStep.Trigger == null) return;
            CurrentStep.Trigger.AddListener(BeginStep);
        }

        /// <summary>
        /// Begins a step by showing its popup
        /// </summary>
        private void BeginStep()
        {
            if (IsStepOld(CurrentStep) || nextStepIsBlocked.Value) return;
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