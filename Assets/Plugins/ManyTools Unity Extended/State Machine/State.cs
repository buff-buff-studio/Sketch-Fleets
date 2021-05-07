using ManyTools.UnityExtended.Editor;
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

        [SerializeField]
        [RequiredField()]
        private string stateID;
        private IStateMachine stateMachine;

        private int stateHash = default;

        #endregion

        #region Properties

        public string StateID => stateID;

        public int StateHash => GetStateIDHash();

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
                if (string.IsNullOrEmpty(stateID))
                {
                    Debug.LogError($"A state's state ID was empty! All IDs must be non-empty and unique");
                }
                
                // TODO: Evaluate if there aren't faster hash functions that could be used here instead.
                stateHash = stateID.GetHashCode();
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
            stateMachine.CurrentState = this;
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