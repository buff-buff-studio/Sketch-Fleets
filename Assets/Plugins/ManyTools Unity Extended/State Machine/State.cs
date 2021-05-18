using UnityEngine;

namespace ManyTools.UnityExtended
{
    /// <summary>
    /// A class that controls a state of a state machine
    /// </summary>
    [System.Serializable]
    public abstract class State : MonoBehaviour, IState
    {
        #region Private Fields
        
        private int stateHash = default;

        #endregion

        #region Protected Fields

        protected IStateMachine stateMachine;

        #endregion

        #region Properties

        public string StateID => GetType().Name;

        public int StateHash => GetStateIDHash();

        public IStateMachine StateMachine
        {
            get => stateMachine;
            set => stateMachine = value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the integer hash of a state's ID
        /// </summary>
        /// <returns>The hash of the state's ID</returns>
        private int GetStateIDHash()
        {
            if (stateHash != default)
            {
                return stateHash;
            }
            else
            {
                if (string.IsNullOrEmpty(StateID))
                {
                    Debug.LogError($"A state's state ID was empty! All IDs must be non-empty and unique");
                }
                
                // TODO: Evaluate if there aren't faster hash functions that could be used here instead.
                stateHash = StateID.GetHashCode();
                return stateHash;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs when the state is entered
        /// </summary>
        public virtual void Enter()
        {
            StateMachine.CurrentState = this;
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public abstract void StateUpdate();

        /// <summary>
        /// Runs when the state exits
        /// </summary>
        public abstract void Exit();

        #endregion
    }
}