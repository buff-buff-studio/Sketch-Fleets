using UnityEngine;
using UnityEngine.Events;

namespace ManyTools.Events
{
    public class FloatEventListener : EventListener<float>
    {
        #region Private Fields

        [SerializeField] private FloatEvent _event;
        [SerializeField] private UnityEvent<float> _unityEvent;
        
        #endregion

        #region EventListener Implementation

        public override UnityEvent<float> UnityEvent => _unityEvent;
        public override GameEvent<float> GameEvent
        {
            get => _event;
            set
            {
                if (_event != null)
                {
                    _event.RemoveListener(this);
                }
            
                _event = value as FloatEvent;
            
                if (_event != null)
                {
                    _event.AddListener(this);
                }
            }
        }

        #endregion
    }
}