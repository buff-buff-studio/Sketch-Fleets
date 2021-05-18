using System.Collections;
using ManyTools.Variables;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ManyTools.Tests
{
    /// <summary>
    /// A test suite related to value modification in Reference and Variable classes with int types
    /// </summary>
    public class ValueTestSuite
    {
        #region Public Tests

        // TODO: reduce code by adding SetUp and TearDown phases
        
        /// <summary>
        /// Tests whether an initial value plus another value will equal initial + another value.
        /// </summary>
        [UnityTest]
        public IEnumerator TestConstantValueChange()
        {
            // Creates an IntReference with a random value
            int randomStartingValue = Random.Range(0, 100);
            IntReference intReference = new IntReference(randomStartingValue);

            // Creates a random change and adds it
            int randomChange = Random.Range(0, 100);
            intReference.Value += randomChange;

            // Tests if random change occurs correctly
            Assert.AreEqual(randomStartingValue + randomChange, intReference.Value);

            yield break;
        }

        /// <summary>
        /// Tests whether an initial variable value plus another value will equal initial + another value.
        /// The variable value is changed directly through the reference Value property.
        /// </summary>
        [UnityTest]
        public IEnumerator TestVariableValueInternalChange()
        {
            // Creates random int values
            int randomStartingValue = Random.Range(0, 100);
            int randomChange = Random.Range(0, 100);

            // Creates an IntVariable and assigns its initial value
            IntVariable startingVariable = ScriptableObject.CreateInstance<IntVariable>();
            startingVariable.Value = randomStartingValue;

            // Creates an IntReference variable, and assigns it the IntVariable
            IntReference intReference = new IntReference(0);
            intReference.UseConstant = false;
            intReference.Variable = startingVariable;

            // Adds the value to the variable through the IntReference
            intReference.Value += randomChange;

            // Tests if random change occurs correctly
            Assert.AreEqual(randomStartingValue + randomChange, intReference.Value);

            yield break;
        }

        /// <summary>
        /// Tests whether an initial variable value plus another value will equal initial + another value.
        /// The value is changed externally through the Variable Value property.
        /// </summary>
        [UnityTest]
        public IEnumerator TestVariableValueExternalChange()
        {
            // Creates random int values
            int randomStartingValue = Random.Range(0, 100);
            int randomChange = Random.Range(0, 100);

            // Creates an IntVariable and assigns its initial value
            IntVariable startingVariable = ScriptableObject.CreateInstance<IntVariable>();
            startingVariable.Value = randomStartingValue;

            // Creates an IntReference variable, and assigns it the IntVariable
            IntReference intReference = new IntReference(0);
            intReference.UseConstant = false;
            intReference.Variable = startingVariable;

            // Adds the value to the variable
            startingVariable.Value += randomChange;

            // Tests if random change occurs correctly
            Assert.AreEqual(randomStartingValue + randomChange, intReference.Value);

            yield break;
        }

        /// <summary>
        /// Tests whether an initial value plus another value will equal initial + another value. The value
        /// is changed through the reference Value property of a secondary reference class that holds the same
        /// variable.
        /// </summary>
        [UnityTest]
        public IEnumerator TestVariableValueTransfer()
        {
            // Creates random int values
            int randomStartingValue = Random.Range(0, 100);
            int randomChange = Random.Range(0, 100);

            // Creates an IntVariable and assigns its initial value
            IntVariable startingVariable = ScriptableObject.CreateInstance<IntVariable>();
            startingVariable.Value = randomStartingValue;

            // Creates an IntReference variable, and assigns it the IntVariable
            IntReference intReference = new IntReference(0);
            intReference.UseConstant = false;
            intReference.Variable = startingVariable;

            // Creates a secondary IntReference and assigns it the same IntVariable
            IntReference secondaryIntReference = new IntReference(0);
            secondaryIntReference.UseConstant = false;
            secondaryIntReference.Variable = startingVariable;

            // Adds the value to the variable
            intReference.Value += randomChange;

            // Tests if both reference values have the same value
            Assert.AreEqual(intReference.Value, secondaryIntReference.Value);

            yield break;
        }
        
        /// <summary>
        /// Tests whether changing a reference variable's variable works
        /// </summary>
        [UnityTest]
        public IEnumerator TestVariableChange()
        {
            // Creates an IntVariable and assigns its initial value
            IntVariable startingVariable = ScriptableObject.CreateInstance<IntVariable>();
            IntVariable secondaryVariable = ScriptableObject.CreateInstance<IntVariable>();

            // Creates an IntReference variable, and assigns it the IntVariable
            IntReference intReference = new IntReference(0);
            intReference.UseConstant = false;
            intReference.Variable = startingVariable;

            // Tests if intReference has received the expected variable
            Assert.AreEqual(startingVariable, intReference.Variable);
            
            // Changes intReference's variable
            intReference.Variable = secondaryVariable;

            // Tests if both intReference has the expected variable
            Assert.AreEqual(secondaryVariable, intReference.Variable);

            yield break;
        }
        
        /// <summary>
        /// Tests whether a reference properly detects it is using constant mode
        /// </summary>
        [UnityTest]
        public IEnumerator TestConstantUsage()
        {
            IntReference intReference = new IntReference(0);
            
            Assert.True(intReference.UseConstant);
            
            yield break;
        }
        
        /// <summary>
        /// Tests whether a reference properly detects it is using variable mode
        /// </summary>
        [UnityTest]
        public IEnumerator TestVariableUsage()
        {
            IntReference intReference = new IntReference(0);
            
            intReference.UseConstant = false;
            
            Assert.False(intReference.UseConstant);

            yield break;
        }

        #endregion
    }
}
