using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ColorHEXCodeAttribute))]
public class ColorHEXCode : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect htmlField = new Rect(position.x, position.y, position.width - 100, position.height);
        Rect colorField = new Rect(position.x + htmlField.width, position.y, position.width - htmlField.width, position.height);

        string htmlValue = EditorGUI.TextField(htmlField, label, "#" + ColorUtility.ToHtmlStringRGBA(property.colorValue));

        Color newCol;
        if (ColorUtility.TryParseHtmlString(htmlValue, out newCol))
            property.colorValue = newCol;

        property.colorValue = EditorGUI.ColorField(colorField, property.colorValue);
    }
}