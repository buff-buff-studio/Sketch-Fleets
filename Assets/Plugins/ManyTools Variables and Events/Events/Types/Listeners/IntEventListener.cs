using UnityEngine;
using UnityEngine.Events;

namespace ManyTools.Events
{
    public class IntEventListener : EventListener<int>
    {
        #region Private Fields

        [SerializeField] private IntEvent _event;
        [SerializeField] private UnityEvent<int> _unityEvent;
        
        #endregion

        #region EventListener Implementation

        public override UnityEvent<int> UnityEvent => _unityEvent;
        public override GameEvent<int> GameEvent
        {
            get => _event;
            set
            {
                if (_event != null)
                {
                    _event.RemoveListener(this);
                }
            
                _event = value as IntEvent;
            
                if (_event != null)
                {
                    _event.AddListener(this);
                }
            }
        }

        #endregion
    }
}
