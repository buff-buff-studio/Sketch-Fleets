using UnityEngine;
using UnityEngine.Events;

namespace ManyTools.Events
{
    public class StringEventListener : EventListener<string>
    {
        #region Private Fields

        [SerializeField] private StringEvent _event;
        [SerializeField] private UnityEvent<string> _unityEvent;
        
        #endregion

        #region EventListener Implementation

        public override UnityEvent<string> UnityEvent => _unityEvent;
        public override GameEvent<string> GameEvent => _event;

        #endregion
    }
}