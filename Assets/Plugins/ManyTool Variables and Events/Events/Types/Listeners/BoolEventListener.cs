using UnityEngine;
using UnityEngine.Events;

namespace ManyTools.Events
{
    public class BoolEventListener : EventListener<bool>
    {
        #region Private Fields

        [SerializeField] private BoolEvent _event;
        [SerializeField] private UnityEvent<bool> _unityEvent;
        
        #endregion

        #region EventListener Implementation

        public override UnityEvent<bool> UnityEvent => _unityEvent;
        public override GameEvent<bool> GameEvent => _event;

        #endregion
    }
}