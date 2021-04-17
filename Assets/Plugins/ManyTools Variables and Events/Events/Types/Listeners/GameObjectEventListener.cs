using UnityEngine;
using UnityEngine.Events;

namespace ManyTools.Events
{
    public class GameObjectEventListener : EventListener<GameObject>
    {
        #region Private Fields

        [SerializeField] private GameObjectEvent _event;
        [SerializeField] private UnityEvent<GameObject> _unityEvent;
        
        #endregion

        #region EventListener Implementation

        public override UnityEvent<GameObject> UnityEvent => _unityEvent;
        public override GameEvent<GameObject> GameEvent
        {
            get => _event;
            set
            {
                if (_event != null)
                {
                    _event.RemoveListener(this);
                }
            
                _event = value as GameObjectEvent;
            
                if (_event != null)
                {
                    _event.AddListener(this);
                }
            }
        }

        #endregion
    }
}