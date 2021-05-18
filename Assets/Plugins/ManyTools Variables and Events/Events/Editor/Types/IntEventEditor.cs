using UnityEditor;

namespace ManyTools.Events
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(IntEvent))]
    public class IntEventEditor : GameEventEditor<int>
    {
        
    }
    #endif
}