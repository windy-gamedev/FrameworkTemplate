using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableList<T> : List<T>, ISerializationCallbackReceiver
{
    [SerializeField]
    T[] list;

    public void OnAfterDeserialize()
    {
        if (list == null)
            return;
        this.Clear();
        this.AddRange(list);
        list = null;
    }

    public void OnBeforeSerialize()
    {
        if (this.Count > this.Capacity)
        {
            this.Clear();
            return;
        }
        list = this.ToArray();
    }
}
