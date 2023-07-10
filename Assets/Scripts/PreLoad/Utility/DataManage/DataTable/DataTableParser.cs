using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Extensions;


#if UNITY_EDITOR
public static class DataTableParser
{
    static String SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static String LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static Char[] TRIM_CHARS = { '\"' };


    public static Dictionary<Int32, TRow> Read<TRow>(TextAsset pData) where TRow : DataTableRow, new() => ReadData<TRow>(pData.text);

    public static Dictionary<Int32, TRow> ReadData<TRow>(String pData)
        where TRow : DataTableRow, new()
    {
        Type entryType = typeof(TRow);

        var result = new Dictionary<Int32, TRow>();
        var lines = Regex.Split(pData, LINE_SPLIT_RE);

        if (lines.Length <= 1)
            return result;

        var fieldInfos = entryType.GetFields(BindingFlags.DeclaredOnly |
    BindingFlags.Instance |
    BindingFlags.Public);

        String[] header = Regex.Split(lines[0], SPLIT_RE);

        for (var i = 2; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            TRow entry = new TRow();


            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                String value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                value = value.Replace("<br>", "\n");
                value = value.Replace("<c>", ",");

                String finalvalue = value;

                if (header[j].Equals("ID"))
                {
                    Debug.Log($"Found ID : {finalvalue}");
                    entry.ID = finalvalue.ToInt32();
                    continue;
                }

                Type type = fieldInfos[j - 1].FieldType;
                var parsedValue = StringUtility.ParseToType(finalvalue, type);

                fieldInfos[j - 1].SetValue(entry, parsedValue);
            }
            Debug.Log($"Add ID : {entry.ID} at i {i}");
            result.Add(entry.ID, entry);
        }
        return result;
    }

    public static System.Object ReadDynamic(String pData,Type pRowType)
    {
        MethodInfo method = typeof(DataTableParser).GetMethod("ReadData");
        MethodInfo generic = method.MakeGenericMethod(pRowType);
        return generic.Invoke(null, new System.Object[] { pData });
    }

}
#endif