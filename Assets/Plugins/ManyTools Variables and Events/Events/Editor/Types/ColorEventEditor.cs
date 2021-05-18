using UnityEditor;
using UnityEngine;

namespace ManyTools.Events
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(ColorEvent))]
    public class ColorEventEditor : GameEventEditor<Color>
    {
        
    }
    #endif
}