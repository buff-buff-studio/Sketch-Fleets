using System;
using UnityEngine;

namespace ManyTools.UnityExtended.Editor
{
    /// <summary>
    /// A field attribute that gives it a given color, should it be null
    /// </summary>
    public class RequiredFieldAttribute : PropertyAttribute
    {
        #region Public Fields

        public Color _fieldColor;

        #endregion

        #region Properties

        public Color FieldColor
        {
            get => _fieldColor;
            set => _fieldColor = value;
        }

        #endregion
        
        #region Constructor

        /// <summary>
        /// Creates a new special field attribute with a custom color
        /// </summary>
        /// <param name="fieldColor">The color of the field</param>
        public RequiredFieldAttribute(Color fieldColor)
        {
            this._fieldColor = fieldColor;
        }

        /// <summary>
        /// Creates a new special field attribute through a number of predefined contexts
        /// </summary>
        /// <param name="fieldType">The type of the field</param>
        /// <exception cref="ArgumentOutOfRangeException">No such field type exists</exception>
        public RequiredFieldAttribute(SpecialFieldType fieldType = SpecialFieldType.Error)
        {
            switch (fieldType)
            {
                case SpecialFieldType.Error:
                    _fieldColor = Color.red;
                    break;
                case SpecialFieldType.Warning:
                    _fieldColor = Color.yellow;
                    break;
                case SpecialFieldType.Exception:
                    _fieldColor = Color.magenta;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldType), fieldType, "No such " +
                                                                                        " special field type" +
                                                                                        " exists.");
            }
        }

        #endregion

        #region Special Field Type Enum

        /// <summary>
        /// An enum containing common field types
        /// </summary>
        public enum SpecialFieldType
        {
            Exception,
            Error,
            Warning
        }

        #endregion
    }
}