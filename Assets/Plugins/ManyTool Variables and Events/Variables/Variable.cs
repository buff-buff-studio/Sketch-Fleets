using System;
using ManyTools.Events;
using UnityEngine;

namespace ManyTools.Variables
{
    /// <summary>
    /// ScriptableObject-based variable base class, that stores a value of a given type
    /// </summary>
    [Serializable]
    public abstract class Variable<T> : ScriptableObject, ISerializationCallbackReceiver
    {
        #region Field Declarations

        #pragma warning disable CS0414
        [SerializeField] [Multiline] private string _description = null;
        #pragma warning restore CS014
        [SerializeField] private T _value;
        [SerializeField] private GameEvent _onChangeEvent;

        // [SerializeField] private EventScriptable _onUpdatedEvent = null;
        private T _runtimeValue;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the runtime or editor value of the Hybrid Type depending on
        /// whether the game is running, or simply in the editor
        /// </summary>
        /// <value>The value of the Hybrid Type</value>
        public virtual T Value
        {
            get
            {
                #if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    #endif
                    return _runtimeValue;
                    #if UNITY_EDITOR
                }
                else
                {
                    return _value;
                }
                #endif
            }
            set
            {
                #if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    #endif
                    _runtimeValue = value;
                    #if UNITY_EDITOR
                }
                else
                {
                    _value = value;
                }
                #endif

                if (_onChangeEvent != null)
                {
                    _onChangeEvent.Invoke();
                }
            }
        }

        /// <summary>
        /// Gets the variable's value at the start of the runtime
        /// </summary>
        /// <value>The value at the start of the runtime</value>
        public virtual T StartingValue
        {
            get { return _value; }
        }

        #endregion

        #region ISerializationCallbackReceiver Implementation

        public void OnAfterDeserialize()
        {
            _runtimeValue = _value;
        }

        public void OnBeforeSerialize()
        {
        }

        #endregion
    }
}