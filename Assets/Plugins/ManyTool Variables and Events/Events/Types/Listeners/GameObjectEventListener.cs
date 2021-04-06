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
        public override GameEvent<GameObject> GameEvent => _event;

        #endregion
    }
}