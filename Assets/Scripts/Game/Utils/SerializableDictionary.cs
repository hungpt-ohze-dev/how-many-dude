using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{

    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public SerializableDictionary(Dictionary<TKey, TValue> dictionary = null)
    {
        if (dictionary != null)
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
                Add(pair.Key, pair.Value);
            }
        }
    }

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        if(keys == null || values == null) return;
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load the dictionary from lists
    public void OnAfterDeserialize()
    {
        Clear();

        if(keys == null || values == null) return;
        if (keys.Count != values.Count)
        {
            Debug.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys ("
                + keys.Count + ") does not match the number of values (" + values.Count
                + ") which indicates that something went wrong");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            Add(keys[i], values[i]);
        }
    }
}
