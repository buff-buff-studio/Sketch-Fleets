using System.Collections.Generic;
using ManyTools.Variables;
using UnityEditor;
using UnityEngine;

namespace ManyTools.Editor
{
    /// <summary>
    /// A property drawer for reference variables.
    /// </summary>
    [CustomPropertyDrawer(typeof(AnimationCurveReference))]
    [CustomPropertyDrawer(typeof(BoolReference))]
    [CustomPropertyDrawer(typeof(ColorReference))]
    [CustomPropertyDrawer(typeof(FloatReference))]
    [CustomPropertyDrawer(typeof(GameObjectReference))]
    [CustomPropertyDrawer(typeof(IntReference))]
    [CustomPropertyDrawer(typeof(StringReference))]
    [CustomPropertyDrawer(typeof(Vector2Reference))]
    [CustomPropertyDrawer(typeof(Vector3Reference))]
    [CustomPropertyDrawer(typeof(Vector4Reference))]
    public class ReferenceDrawer : PropertyDrawer
    {
        #region Private Fields

        // A dictionary to cache all property data
        private Dictionary<string, PropertyData> _propertyDatas = new Dictionary<string, PropertyData>();

        // Literals for dropdown menu
        private readonly string[] _dropdownOptions = {"Constant", "Variable"};

        // GUI style for dropdown
        private GUIStyle _dropdownStyle;

        #endregion

        #region PropertyDrawer Implementation

        /// <summary>
        ///   <para>Override this method to make your own IMGUI based GUI for the property.</para>
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The SerializedProperty to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Gets dropdown style if it is null
            _dropdownStyle ??= GetDropdownStyle();

            // Caches the property data
            PropertyData propertyData = GetPropertyData(property);

            // Begins and labels the property of the drawer
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            // Get config button rect
            Rect buttonRect = GetButtonRect(position);
            // Only begin field after button
            position.xMin = buttonRect.xMax;

            // Caches indent
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int result = EditorGUI.Popup(buttonRect, propertyData.UseConstant.boolValue ? 0 : 1,
                _dropdownOptions, _dropdownStyle);

            propertyData.UseConstant.boolValue = result == 0;

            EditorGUI.PropertyField(position, 
                propertyData.UseConstant.boolValue ? propertyData.Constant : propertyData.Variable, 
                GUIContent.none);

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the dropdown style
        /// </summary>
        /// <returns>The GUIStyle that composes the dropdown</returns>
        private GUIStyle GetDropdownStyle()
        {
            GUIStyle dropdownStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            dropdownStyle.imagePosition = ImagePosition.ImageOnly;

            return dropdownStyle;
        }

        /// <summary>
        /// Gets the data relating to a property
        /// </summary>
        /// <param name="property">The property to gather data about</param>
        /// <returns>A struct containing the property's data</returns>
        private PropertyData GetPropertyData(SerializedProperty property)
        {
            // Checks if a propertyData already exists for the given path. If so, return it as propertyData
            if (_propertyDatas.TryGetValue(property.propertyPath, out PropertyData propertyData))
            {
                return propertyData;
            }

            // Creates a new propertyData
            propertyData = new PropertyData(
                property.FindPropertyRelative("_useConstant"),
                property.FindPropertyRelative("_constant"),
                property.FindPropertyRelative("_variable"));

            // Adds it to the dictionary and returns it
            _propertyDatas.Add(property.propertyPath, propertyData);

            return propertyData;
        }

        /// <summary>
        /// Gets the rect of a button
        /// </summary>
        /// <param name="position">The position to base the button on</param>
        /// <returns>The rect of a button before the field</returns>
        private Rect GetButtonRect(Rect position)
        {
            // Get config button rect
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += _dropdownStyle.margin.top;
            buttonRect.xMin += _dropdownStyle.margin.left - 20;
            buttonRect.width = _dropdownStyle.fixedWidth + _dropdownStyle.margin.right;
            return buttonRect;
        }

        #endregion

        #region PropertyData Class

        /// <summary>
        /// A class containing the data of a property
        /// </summary>
        private class PropertyData
        {
            public SerializedProperty UseConstant;
            public SerializedProperty Constant;
            public SerializedProperty Variable;

            public PropertyData(SerializedProperty useConstant, SerializedProperty constant,
                SerializedProperty variable)
            {
                UseConstant = useConstant;
                Constant = constant;
                Variable = variable;
            }
        }

        #endregion
    }
}