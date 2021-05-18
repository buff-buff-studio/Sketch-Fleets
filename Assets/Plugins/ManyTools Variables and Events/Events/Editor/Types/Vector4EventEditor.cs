using UnityEditor;
using UnityEngine;

namespace ManyTools.Events
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(Vector4Event))]
    public class Vector4EventEditor : GameEventEditor<Vector4>
    {
        
    }
    #endif
}