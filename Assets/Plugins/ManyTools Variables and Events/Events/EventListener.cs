using System;
using ManyTools.Events.Types;
using UnityEngine;
using UnityEngine.Events;

namespace ManyTools.Events
{
    public class EventListener : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameEvent _gameEvent;
        [SerializeField] private UnityEvent _onEventInvoked = new UnityEvent();

        #endregion

        #region Properties

        public GameEvent GameEvent
        {
            get => _gameEvent;
            set
            {
                if (_gameEvent != null)
                {
                    _gameEvent.RemoveListener(this);
                }

                _gameEvent = value;

                if (_gameEvent != null)
                {
                    _gameEvent.AddListener(this);
                }
            }
        }

        public UnityEvent UnityEvent => _onEventInvoked;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            if (GameEvent == null) return;
            GameEvent.AddListener(this);
        }

        private void OnDestroy()
        {
            if (GameEvent == null) return;
            GameEvent.RemoveListener(this);
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Invokes the unity event when the event is invoked
        /// </summary>
        public void OnEventInvoked()
        {
            _onEventInvoked.Invoke();
        }

        #endregion
    }

    /// <summary>
    /// Listens to an event with an argument of type T
    /// </summary>
    /// <typeparam name="T">An argument of type T</typeparam>
    public abstract class EventListener<T> : MonoBehaviour, IEventListener<T>
    {
        #region Abstract Properties

        public abstract UnityEvent<T> UnityEvent { get; }
        public abstract GameEvent<T> GameEvent { get; set; }

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            if (GameEvent == null) return;
            GameEvent.AddListener(this);
        }

        private void OnDestroy()
        {
            if (GameEvent == null) return;
            GameEvent.RemoveListener(this);
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Invokes the unity event when the event is invoked
        /// </summary>
        public void OnEventInvoked(T value)
        {
            UnityEvent.Invoke(value);
        }

        #endregion
    }
}