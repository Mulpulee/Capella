using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class DataTable<T> : IEnumerable<T>, IEnumerator<T> where T : DataTableRow
{
    [SerializeField, ReadOnly]
    private SerializableDictionary<Int32, T> _data;

    public Int32 Count => _data.Count;

    public IEnumerable<T> Values => _data.Values;

    public Boolean ContainsID(Int32 pID)
    {
        return _data.ContainsKey(pID);
    }

    public T ElementAt(Int32 pIndex)
    {
        return _data.ElementAt(pIndex).Value;
    }

    public T this[Int32 pID]
    {
        get
        {
            return _data[pID];
        }
    }


#if UNITY_EDITOR

    public DataTable(Dictionary<Int32, T> initData)
    {
        this._data = new SerializableDictionary<Int32, T>(initData);
    }

    public DataTable(object initObject)
    {
        this._data = new SerializableDictionary<Int32, T>((initObject as Dictionary<Int32, T>));
    }

#endif

    #region Implemetation for Foreach

    private Int32 position = -1;

    public T Current => _data.ElementAt(position).Value;

    System.Object IEnumerator.Current => Current;

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in _data)
        {
            yield return item.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Boolean MoveNext()
    {
        if (position == _data.Count - 1)
        {
            Reset();
            return false;
        }

        position++;
        return (position < _data.Count);
    }

    public void Reset() => position = -1;

    public void Dispose()
    {
        Debug.LogError($"Dispose Called!");
    }
    #endregion
}
