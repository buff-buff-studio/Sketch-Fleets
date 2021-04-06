using UnityEngine;
using UnityEngine.Events;

namespace ManyTools.Events
{
    public class Vector2EventListener : EventListener<Vector2>
    {
        #region Private Fields

        [SerializeField] private Vector2Event _event;
        [SerializeField] private UnityEvent<Vector2> _unityEvent;
        
        #endregion

        #region EventListener Implementation

        public override UnityEvent<Vector2> UnityEvent => _unityEvent;
        public override GameEvent<Vector2> GameEvent => _event;

        #endregion
    }
}