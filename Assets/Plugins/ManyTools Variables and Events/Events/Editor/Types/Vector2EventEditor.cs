using UnityEditor;
using UnityEngine;

namespace ManyTools.Events
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(Vector2Event))]
    public class Vector2EventEditor : GameEventEditor<Vector2>
    {
        
    }
    #endif
}