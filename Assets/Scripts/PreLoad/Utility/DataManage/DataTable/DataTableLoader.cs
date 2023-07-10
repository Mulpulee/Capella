using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DataTableLoader
{
    private static Dictionary<String, ScriptableObject> _tableByName;

    public static void Init()
    {
        _tableByName = new Dictionary<String, ScriptableObject>();
        var loaded = Resources.LoadAll<ScriptableObject>("DataTableAsset");
        for(Int32 i=0;i<loaded.Length;i++)
            _tableByName.Add(loaded[i].name.Replace("Asset",""), loaded[i]);
    }

    public static DataTable<TRow> GetTable<TRow>() where TRow : DataTableRow
    {
        if (_tableByName == null || _tableByName.Count == 0)
            Init();

        String tableName = typeof(TRow).Name.Replace("Row", "");

        if (!_tableByName.ContainsKey(tableName))
        {
            Debug.LogError($"Table : {tableName} is not Loaded!");
            return null;
        }

        return (_tableByName[tableName] as DataTableAsset<TRow>).Table;
    }
}
