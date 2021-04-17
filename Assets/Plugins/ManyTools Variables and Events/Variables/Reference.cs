using UnityEngine;

namespace ManyTools.Variables
{
    /// <summary>
    /// A Reference allows for the use of a <see cref="Variable{T}"/> or a value type.
    /// </summary>
    /// <typeparam name="T">The type of the reference value</typeparam>
    /// <typeparam name="U">A variable of the same type of the reference value</typeparam>
    [System.Serializable]
    public abstract class Reference<T, U> where U : Variable<T>
    {
        #region Field Declarations
        [SerializeField]
        private bool _useConstant;
        [SerializeField]
        private T _constant;
        [SerializeField]
        private U _variable;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the value of the reference
        /// </summary>
        public T Value
        {
            get => UseConstant ? _constant : (Variable.Value ?? _constant);
            
            set
            {
                if (UseConstant == true)
                {
                    _constant = value;
                }
            
                if (Variable != null)
                {
                    Variable.Value = value;
                }
            }
        }

        /// <summary>
        /// Gets the variable of the reference
        /// </summary>
        public U Variable
        {
            get => _variable;
            set => _variable = value;
        }

        /// <summary>
        /// Gets whether the variable is using a constant or not
        /// </summary>
        public bool UseConstant
        {
            get => _useConstant;
            set => _useConstant = value;
        }

        #endregion

        #region Implicit Operator
        /// <summary>
        /// Removes the need for implementing IComparable, IEquatable and
        /// directly referencing the .Value property, by implicitly
        /// implying the use of the .Value property by default
        /// </summary>
        /// <param name="reference"></param>
        public static implicit operator T(Reference<T, U> reference)
        {
            return reference.Value;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes variable as a constant
        /// </summary>
        public Reference()
        {
            UseConstant = true;
        }

        /// <summary>
        /// Initializes variable as a constant and assigns it a value
        /// </summary>
        /// <param name="startingValue">The starting value of the constant</param>
        public Reference(T startingValue)
        {
            UseConstant = true;
            _constant = startingValue;
        }

        /// <summary>
        /// Returns the variable's value as a string
        /// </summary>
        /// <returns>A string of the variable's value</returns>
        public override string ToString()
        {
            return Value.ToString();
        }
        #endregion
    }
}