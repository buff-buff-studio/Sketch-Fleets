using ManyTools.UnityExtended.Editor;
using UnityEngine;

namespace ManyTools.UnityExtended
{
    /// <summary>
    /// A template state machine pattern
    /// </summary>
    [System.Serializable]
    public class StateMachine : MonoBehaviour, IStateMachine
    {
        #region Protected Fields

        [SerializeField, RequiredField()]
        protected State defaultState;
        [SerializeField]
        protected State[] states;

        #endregion

        #region Properties

        public State CurrentState { get; set; }
        public State DefaultState => defaultState;

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            TransitionToState(defaultState.StateHash);

            if (states.Length <= 0 || CurrentState == null)
            {
                Debug.LogError($"The state machine has no states or could not default to the first" +
                               $" state!");
                return;
            }
            
            CheckForHashCollisions();
        }

        // Update is called once per frame
        private void Update()
        {
            CurrentState.StateUpdate();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Transitions to a given state through its ID
        /// </summary>
        /// <param name="stateID">The state ID</param>
        public void TransitionToState(string stateID)
        {
            for (int index = 0, upper = states.Length; index < upper; index++)
            {
                if (states[index].StateID != stateID) continue;

                CurrentState.Exit();
                CurrentState = states[index];
                CurrentState.Enter();

                return;
            }
        }

        /// <summary>
        /// Transitions to a given state through its hash.
        /// </summary>
        /// <param name="stateHash">The state ID Hash</param>
        public void TransitionToState(int stateHash)
        {
            for (int index = 0, upper = states.Length; index < upper; index++)
            {
                if (states[index].StateHash != stateHash) continue;

                CurrentState.Exit();
                CurrentState = states[index];
                CurrentState.Enter();

                return;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Tests if any states have identical hashes
        /// </summary>
        private void CheckForHashCollisions()
        {
            for (int index = 0, upper = states.Length; index < upper; index++)
            {
                for (int nestedIndex = index + 1; nestedIndex < upper; nestedIndex++)
                {
                    if (states[index].StateHash == states[nestedIndex].StateHash)
                    {
                        Debug.LogError("A hash collision was detected in the State Machine! Ensure every" +
                                       " state ID is unique and that there are no duplicate states!");
                    }
                }
            }
        }

        #endregion
    }
}