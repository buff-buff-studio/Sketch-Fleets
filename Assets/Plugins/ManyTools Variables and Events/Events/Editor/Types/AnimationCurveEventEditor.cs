using UnityEditor;
using UnityEngine;

namespace ManyTools.Events
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(AnimationCurveEvent))]
    public class AnimationCurveEventEditor : GameEventEditor<AnimationCurve>
    {
        
    }
    #endif
}