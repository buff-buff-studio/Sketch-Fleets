using UnityEditor;
using UnityEngine;

namespace ManyTools.Events
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(AudioClipEvent))]
    public class AudioClipEventEditor : GameEventEditor<AudioClip>
    {
        
    }
    #endif
}