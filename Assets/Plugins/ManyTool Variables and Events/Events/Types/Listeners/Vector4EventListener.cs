using UnityEngine;
using UnityEngine.Events;

namespace ManyTools.Events
{
    public class Vector4EventListener : EventListener<Vector4>
    {
        #region Private Fields

        [SerializeField] private Vector4Event _event;
        [SerializeField] private UnityEvent<Vector4> _unityEvent;
        
        #endregion

        #region EventListener Implementation

        public override UnityEvent<Vector4> UnityEvent => _unityEvent;
        public override GameEvent<Vector4> GameEvent => _event;

        #endregion
    }
}