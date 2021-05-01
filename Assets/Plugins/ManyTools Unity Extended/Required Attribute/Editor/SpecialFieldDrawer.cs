using UnityEditor;
using UnityEngine;

namespace ManyTools.UnityExtended.Editor
{
    /// <summary>
    /// A property drawer for special fields
    /// </summary>
    [CustomPropertyDrawer(typeof(RequiredFieldAttribute))]
    public sealed class SpecialFieldDrawer : PropertyDrawer
    {
        #region Private Fields

        private RequiredFieldAttribute requiredFieldAttribute;

        #endregion

        #region PropertyDrawer Implementation

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // If the attribute is null, cache it
            requiredFieldAttribute ??= attribute as RequiredFieldAttribute;

            if (property.objectReferenceValue == null)
            {
                // Caches the current UI color
                Color cachedColor = GUI.color;
                // Draws field with the color
                GUI.color = requiredFieldAttribute._fieldColor;
                EditorGUI.PropertyField(position, property, label);
                // Restores the previous UI color
                GUI.color = cachedColor;
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }

        #endregion
    }
}