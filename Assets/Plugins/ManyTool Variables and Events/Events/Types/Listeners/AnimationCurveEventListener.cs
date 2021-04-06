using UnityEngine;
using UnityEngine.Events;

namespace ManyTools.Events
{
    public class AnimationCurveEventListener : EventListener<AnimationCurve>
    {
        #region Private Fields

        [SerializeField] private AnimationCurveEvent _event;
        [SerializeField] private UnityEvent<AnimationCurve> _unityEvent;
        
        #endregion

        #region EventListener Implementation

        public override UnityEvent<AnimationCurve> UnityEvent => _unityEvent;
        public override GameEvent<AnimationCurve> GameEvent => _event;

        #endregion
    }
}