using UnityEditor;
using UnityEngine;

namespace ManyTools.Events
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(GameObjectEvent))]
    public class GameObjectEventEditor : GameEventEditor<GameObject>
    {
        
    }
    #endif
}