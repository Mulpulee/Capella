using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataSystem<T> where T : DataTableRow
{
    private static DataTable<T> _table;

    public static void Init()
    {
        _table = DataTableLoader.GetTable<T>();
    }

    public static IEnumerable<T> GetDatas()
    {
        if (_table == null)
            Init();

        return _table.Values;
    }

    public static T GetRow(Int32 pID)
    {
        if (_table == null)
            Init();

        if(!_table.ContainsID(pID))
        {
            Debug.Log($"{pID} is unknown key");
            return null;
        }

        return _table[pID];
    }
}