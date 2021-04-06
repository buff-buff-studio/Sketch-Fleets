using System;
using ManyTools.Events.Types;
using UnityEngine;
using UnityEngine.Events;

namespace ManyTools.Events
{
    /// <summary>
    /// Listens to an <see cref="GameEventBase"/>.
    /// </summary>
    public class EventListener : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameEvent _gameEvent;
        [SerializeField] private UnityEvent _onEventInvoked;

        #endregion

        #region Properties

        public GameEvent GameEvent
        {
            get => _gameEvent;
        }

        #endregion

        #region MonoBehaviour Implementation

        private void OnEnable()
        {
            _gameEvent.AddListener(this);
        }

        private void OnDisable()
        {
            _gameEvent.RemoveListener(this);
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
        public abstract GameEvent<T> GameEvent { get; }

        #endregion

        #region MonoBehaviour Implementation

        private void OnEnable()
        {
            GameEvent.AddListener(this);
        }

        private void OnDisable()
        {
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