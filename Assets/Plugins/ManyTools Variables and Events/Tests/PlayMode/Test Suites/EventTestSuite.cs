using System.Collections;
using ManyTools.Variables;
using ManyTools.Events;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ManyTools.Tests
{
    /// <summary>
    /// A test suite related to events and event listeners
    /// </summary>
    public class EventTestSuite
    {
        #region Private Fields

        private EventListener _listener;
        private IntEventListener _genericListener;
        private TestValueContainer _testValueContainer;

        #endregion
        
        #region SetUp and TearDown
        
        [UnitySetUp]
        public IEnumerator SetUpTest()
        {
            GameObject listenerObject = GameObject.Instantiate(
                Resources.Load<GameObject>(
                    "Tests/Listener Holder"));
            
            _listener = listenerObject.GetComponent<EventListener>();
            _genericListener = listenerObject.GetComponent<IntEventListener>();
            _testValueContainer = listenerObject.GetComponent<TestValueContainer>();

            yield break;
        }

        [UnityTearDown]
        public IEnumerator TearDownTest()
        {
            GameObject.Destroy(_listener.gameObject);
            
            yield break;
        }
        #endregion
        
        #region Public Tests

        /// <summary>
        /// Tests whether the listener event exists
        /// </summary>
        [UnityTest]
        public IEnumerator TestListenerExists()
        {
            UnityEngine.Assertions.Assert.IsNotNull(_listener);
            
            yield break;
        }
        
        /// <summary>
        /// Tests whether an initial value plus another value will equal initial + another value.
        /// This is a form of indirect event invocation.
        /// </summary>
        [UnityTest]
        public IEnumerator TestVariableOnChangeEventInvoke()
        {
            // Creates a variable and assigns it a change event
            IntVariable intVariable = ScriptableObject.CreateInstance<IntVariable>();
            intVariable.OnChangeEvent = ScriptableObject.CreateInstance<GameEvent>();
            
            // Creates an reference variable and assigns it a variable
            IntReference intReference = new IntReference(0);
            intReference.UseConstant = false;
            intReference.Variable = intVariable;

            // Sets the listener to listen to the on change event
            _listener.GameEvent = intVariable.OnChangeEvent;
            
            // Attempts to trigger OnChangeEvent by modifying variable value
            intReference.Value += 1;

            // Tests if OnChangeEvent has set the variable to true
            Assert.True(_testValueContainer.Raised);

            yield break;
        }

        /// <summary>
        /// Tests whether a listener will listen to an event being directly invoked.
        /// </summary>
        [UnityTest]
        public IEnumerator TestEventInvoke()
        {
            // Creates and assigns an event
            GameEvent testEvent = ScriptableObject.CreateInstance<GameEvent>();
            _listener.GameEvent = testEvent;
            
            testEvent.Invoke();
            
            Assert.True(_testValueContainer.Raised);
            
            yield break;
        }

        /// <summary>
        /// Tests whether a listener will listen to a generic event being directly invoked. 
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator TestGenericEventInvoke()
        {
            
            // Creates and assigns an event
            IntEvent testEvent = ScriptableObject.CreateInstance<IntEvent>();
            _genericListener.GameEvent = testEvent;

            // Defines a random value
            int randomValue = Random.Range(0, 100);

            // Invokes the event
            testEvent.Invoke(randomValue);
            
            Assert.AreEqual(_testValueContainer.Value, randomValue);
            
            yield break;
        }

        #endregion
    }
}