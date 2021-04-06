using UnityEngine;
using UnityEngine.Events;

namespace ManyTools.Events
{
    public class AudioClipEventListener : EventListener<AudioClip>
    {
        #region Private Fields

        [SerializeField] private AudioClipEvent _event;
        [SerializeField] private UnityEvent<AudioClip> _unityEvent;
        
        #endregion

        #region EventListener Implementation

        public override UnityEvent<AudioClip> UnityEvent => _unityEvent;
        public override GameEvent<AudioClip> GameEvent => _event;

        #endregion
    }
}