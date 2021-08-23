using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace SketchFleets
{
    /// <summary>
    /// Custom drawer for localizable dropdown
    /// </summary>
    [CustomPropertyDrawer(typeof(LocalizableDropdown.OptionData))]
    public class OptionDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {        
            EditorGUI.BeginProperty(rect, label, property);
            property.FindPropertyRelative("value").stringValue = EditorGUI.TextField(new Rect(rect.x,rect.y,rect.width,rect.height - 4),property.FindPropertyRelative("value").stringValue);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 24;
        }
    }
}
#endif