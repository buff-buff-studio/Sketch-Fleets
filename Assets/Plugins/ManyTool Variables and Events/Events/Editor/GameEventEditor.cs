using UnityEngine;
using UnityEditor;

namespace ManyTools.Events
{
    /// <summary>
    /// An editor with a simple invoke button for the Event class
    /// </summary>
    [CustomEditor(typeof(GameEvent))]
    public sealed class GameEventEditor : UnityEditor.Editor
    {
        #region Private Fields

        private GameEvent _event;

        #endregion


        #region Editor Implementation

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Invoke Event"))
            {
                CacheTarget();
                _event.Invoke();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Caches the target
        /// </summary>
        /// <exception cref="MissingComponentException">The target could not be converted to
        /// an event</exception>
        private void CacheTarget()
        {
            _event = target as GameEvent;

            if (_event == null) throw new MissingComponentException();
        }

        #endregion
        
    }

    /// <summary>
    /// An editor with a simple invoke button for the Event class
    /// </summary>
    public abstract class GameEventEditor<T> : UnityEditor.Editor
    {
        #region Private Fields

        private GameEvent<T> _event;

        #endregion


        #region Editor Implementation

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Invoke Event"))
            {
                CacheTarget();
                _event.Invoke();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Caches the target
        /// </summary>
        /// <exception cref="MissingComponentException">The target could not be converted to
        /// an event</exception>
        private void CacheTarget()
        {
            _event = target as GameEvent<T>;

            if (_event == null) throw new MissingComponentException();
        }

        #endregion
    }
}