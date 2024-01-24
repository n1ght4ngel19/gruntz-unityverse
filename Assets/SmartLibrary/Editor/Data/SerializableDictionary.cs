using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bewildered.SmartLibrary
{
    [Serializable]
    internal class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [Serializable]
        private struct KeyValuePair
        {
            public TKey key;
            public TValue value;
        }

        [SerializeField] private List<KeyValuePair> _keyValuePairs = new List<KeyValuePair>();

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _keyValuePairs.Clear();
            foreach (var pair in this)
            {
                _keyValuePairs.Add(new KeyValuePair() { key = pair.Key, value = pair.Value });
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();
            try
            {
                foreach (var pair in _keyValuePairs)
                {
                    Add(pair.key, pair.value);
                }
            }
            catch
            {
                Debug.LogError("SmartLibrary: A problem was encountered while deserializing a SerializableDictionary. " + 
                               "This can happen after updating if the data was changed in the new update. Please restart the editor. " +
                               "If the error persists please contact support. Exception to follow...");
                throw;
            }
        }
    } 
}
