using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManyTools.UnityExtended
{
    /// <summary>
    /// A dictionary serializable by Unity
    /// </summary>
    [Serializable]
    public class UnityDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        #region Private Fields

        [SerializeField]
        private List<KeyValuePair> _keyValueList = new List<KeyValuePair>();
        [SerializeField]
        private Dictionary<TKey, int> _indexByKey = new Dictionary<TKey, int>();
        [SerializeField, HideInInspector]
        private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        #pragma warning disable 0414
        [SerializeField, HideInInspector]
        private bool _keyCollision;
        #pragma warning restore 0414
        
        #endregion

        #region IDictionary Implementation

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

        /// <summary>
        /// Adds a key value pair to the dictionary
        /// </summary>
        /// <param name="item">The key value pair to add to the dictionary</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }
        
        /// <summary>
        /// Clears the dictionary and other internal collections
        /// </summary>
        public void Clear()
        {
            // Clears the internal dictionary, index key dictionary and serialized list
            _dictionary.Clear();
            _keyValueList.Clear();
            _indexByKey.Clear();
        }
        
        /// <summary>
        /// Checks if the dictionary contains a specific value
        /// </summary>
        /// <param name="item">The value to search for</param>
        /// <returns>Whether the value exists</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value;

            // If the value exists
            if (_dictionary.TryGetValue(item.Key, out value))
            {
                // Compares the found value to the expected value
                return EqualityComparer<TValue>.Default.Equals(value, item.Value);
            }
            else
            {
                return false;
            }
        }
        

        /// <summary>
        /// Copies the dictionary to an array
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="arrayIndex">The index to begin copying into</param>
        /// <exception cref="ArgumentException">The array cannot be null</exception>
        /// <exception cref="ArgumentOutOfRangeException">The starting index cannot be negative</exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentException("The array cannot be null");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "The starting array index " +
                                                                          "cannot be negative.");
            }

            if (array.Length - arrayIndex < _dictionary.Count)
            {
                throw new ArgumentException("The destination array has fewer elements than" +
                                            " the collection.");
            }

            foreach (KeyValuePair<TKey, TValue> keyValuePair in _dictionary)
            {
                array[arrayIndex] = keyValuePair;
                arrayIndex++;
            }
        }
        
        /// <summary>
        /// Removes a KeyValuePair from the dictionary and other internal collections
        /// </summary>
        /// <param name="item">The KeyValuePair to remove</param>
        /// <returns>Whether it successfully removed the KeyValuePair</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            TValue value;

            if (_dictionary.TryGetValue(item.Key, out value))
            {
                bool valueMatch = EqualityComparer<TValue>.Default.Equals(value, item.Value);

                if (valueMatch)
                {
                    return Remove(item.Key);
                }
            }

            return false;
        }

        public int Count => _dictionary.Count;
        
        public bool IsReadOnly
        {
            get;
            set;
        }
        
        /// <summary>
        /// Adds a value to the dictionary
        /// </summary>
        /// <param name="key">The key to add the value to</param>
        /// <param name="value">The value to add</param>
        public void Add(TKey key, TValue value)
        {
            // Adds value to dictionary
            _dictionary.Add(key, value);
            
            // Adds value to serialized list
            _keyValueList.Add(new KeyValuePair(key, value));
            
            // Adds value index to the index list
            _indexByKey.Add(key, _keyValueList.Count - 1);
        }

        /// <summary>
        /// Whether the dictionary contains a given key
        /// </summary>
        /// <param name="key">The key to search for</param>
        /// <returns>Whether the key exists in the Dictionary</returns>
        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        public bool Remove(TKey key)
        {
            // If the dictionary could remove the given key
            if (_dictionary.Remove(key))
            {
                // Gets the key's index
                int index = _indexByKey[key];
                
                // Removes the value and key from the KeyValue list
                _keyValueList.RemoveAt(index);
                
                UpdateIndexes(index);
                
                // Remove the key from the index list
                _indexByKey.Remove(key);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Updates all indexes in the Key by Index list
        /// </summary>
        /// <param name="removedIndex">The index that was removed</param>
        private void UpdateIndexes(int removedIndex)
        {
            for (int index = removedIndex, upper = _keyValueList.Count; index < upper; index++)
            {
                TKey key = _keyValueList[index].Key;
                _indexByKey[key]--;
            }
        }

        /// <summary>
        /// Attempts to get a value from the dictionary
        /// </summary>
        /// <param name="key">The key to get the value from</param>
        /// <param name="value">The variable to assign the value to, if it is found</param>
        /// <returns>Whether a value was found</returns>
        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

        /// <summary>
        /// Sets and gets values
        /// </summary>
        /// <param name="key">The key to set to a given value</param>
        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                // Sets value
                _dictionary[key] = value;

                if (_indexByKey.ContainsKey(key))
                {
                    // Replaces a preexisting key value index
                    int index = _indexByKey[key];
                    _keyValueList[index] = new KeyValuePair(key, value);
                }
                else
                {
                    // Creates a new key value index
                    _keyValueList.Add(new KeyValuePair(key, value));
                    _indexByKey.Add(key, _keyValueList.Count - 1);
                }
            }
        }

        public ICollection<TKey> Keys => _dictionary.Keys;
        public ICollection<TValue> Values => _dictionary.Values;

        #endregion

        #region ISerializationCallbackReceiver Implementation

        /// <summary>
        ///   <para>Implement this method to receive a callback before Unity serializes your object.</para>
        /// </summary>
        public void OnBeforeSerialize()
        {
            
        }
        
        /// <summary>
        ///   <para>Implement this method to receive a callback after Unity deserializes your object.</para>
        /// </summary>
        public void OnAfterDeserialize()
        {
            _dictionary.Clear();
            _indexByKey.Clear();
            _keyCollision = false;

            for (int index = 0, upper = _keyValueList.Count; index < upper; index++)
            {
                // Adds key and value if no key collision is detected
                if (_keyValueList[index].Key != null && !ContainsKey(_keyValueList[index].Key))
                {
                    _dictionary.Add(_keyValueList[index].Key, _keyValueList[index].Value);
                    _indexByKey.Add(_keyValueList[index].Key, index);
                }
                else
                {
                    _keyCollision = true;
                }
            }
        }

        #endregion

        #region KeyValuePair Struct

        /// <summary>
        /// A non-generic Key-Value pair struct serializable by Unity
        /// </summary>
        [Serializable]
        private struct KeyValuePair
        {
            #region Public Fields

            [SerializeField]
            public TKey Key;
            [SerializeField]
            public TValue Value;

            #endregion
            
            #region Constructor

            public KeyValuePair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }

            #endregion
        }

        #endregion
    }
}
