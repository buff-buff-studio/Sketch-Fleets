using UnityEditor;
using UnityEngine;

namespace ManyTools.Events
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(Vector3Event))]
    public class Vector3EventEditor : GameEventEditor<Vector3>
    {
        
    }
    #endif
}