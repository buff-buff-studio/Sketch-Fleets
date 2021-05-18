using UnityEditor;

namespace ManyTools.Events
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(StringEvent))]
    public class StringEventEditor : GameEventEditor<string>
    {
        
    }
    #endif
}