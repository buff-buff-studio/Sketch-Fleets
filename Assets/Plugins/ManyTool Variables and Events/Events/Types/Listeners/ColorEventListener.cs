using UnityEngine;
using UnityEngine.Events;

namespace ManyTools.Events
{
    public class ColorEventListener : EventListener<Color>
    {
        #region Private Fields

        [SerializeField] private ColorEvent _event;
        [SerializeField] private UnityEvent<Color> _unityEvent;
        
        #endregion

        #region EventListener Implementation

        public override UnityEvent<Color> UnityEvent => _unityEvent;
        public override GameEvent<Color> GameEvent => _event;

        #endregion
    }
}