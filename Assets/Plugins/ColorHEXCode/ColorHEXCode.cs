using UnityEngine;
using UnityEditor;

namespace SketchFleets.Plugins
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ColorHEXCodeAttribute))]
    public class ColorHEXCode : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect htmlField = new Rect(position.x, position.y, position.width - 100, position.height);
            Rect colorField = new Rect(position.x + htmlField.width, position.y, position.width - htmlField.width,
                position.height);

            string htmlValue = EditorGUI.TextField(htmlField, label,
                "#" + ColorUtility.ToHtmlStringRGBA(property.colorValue));

            EditorGUI.BeginChangeCheck();

            Color newCol;
            if (ColorUtility.TryParseHtmlString(htmlValue, out newCol))
                property.colorValue = newCol;

            newCol = EditorGUI.ColorField(colorField, property.colorValue);

            if (EditorGUI.EndChangeCheck())
            {
                property.colorValue = newCol;
                property.serializedObject.ApplyModifiedProperties();

                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
            EditorGUI.EndProperty();
        }
    }
#endif
}