using UnityEditor;
using UnityEngine;

namespace ManyTools.UnityExtended.Editor
{
    /// <summary>
    /// A property drawer for a serialized dictionary
    /// </summary>
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(UnityDictionary<,>))]
    public class UnityDictionaryDrawer : PropertyDrawer
    {
        #region Private Fields

        private static float _lineHeight = EditorGUIUtility.singleLineHeight;
        private static float _verticalSpace = EditorGUIUtility.standardVerticalSpacing;
        private const float WarningBoxHeight = 1.5f;

        #endregion

        #region PropertyDrawer Implementation
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draws a list with the KeyValueClass
            SerializedProperty keyValueList = property.FindPropertyRelative("_keyValueList");
            EditorGUI.PropertyField(position, keyValueList, label, true);

            // Check if there are duplicate keys
            bool keyCollision = property.FindPropertyRelative("_keyCollision").boolValue;
            // If there are, display warning
            if (keyCollision)
            {
                // Descends Y position to draw warning
                position.y += EditorGUI.GetPropertyHeight(keyValueList, true);
                
                // If the list is not expanded
                if (!keyValueList.isExpanded)
                {
                    position.y += _verticalSpace;
                }
                
                // Updates warning box rect
                position.height = _lineHeight * WarningBoxHeight;
                position = EditorGUI.IndentedRect(position);
                
                // Draws warning box
                EditorGUI.HelpBox(position, "Duplicate keys will be ignored!", MessageType.Warning);
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = 0f;
            
            // Gets height of the KeyValuePair class
            SerializedProperty listProperty = property.FindPropertyRelative("_keyValueList");
            totalHeight += EditorGUI.GetPropertyHeight(listProperty, true);
            
            // Gets height of key collision warning
            bool keyCollision = property.FindPropertyRelative("_keyCollision").boolValue;
            if (keyCollision)
            {
                totalHeight += WarningBoxHeight * _lineHeight;

                if (!listProperty.isExpanded)
                {
                    totalHeight += _verticalSpace;
                }
            }

            return totalHeight;
        }

        #endregion
    }
    #endif
}