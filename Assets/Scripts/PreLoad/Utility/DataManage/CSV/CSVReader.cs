using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
    static String SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static String LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static Char[] TRIM_CHARS = { '\"' };


    public static List<Dictionary<String, String>> Read(TextAsset data) => ReadData(data?.text);

    public static List<Dictionary<String, String>> Read(String file) => Read(Resources.Load<TextAsset>(file));

    public static List<Dictionary<String, String>> ReadData(String data)
    {
        if (String.IsNullOrEmpty(data))
            return null;

        var list = new List<Dictionary<String, String>>();

        var lines = Regex.Split(data, LINE_SPLIT_RE);

        if (lines.Length <= 1)
            return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<String, String>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                String value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                value = value.Replace("<br>", "\n"); // �߰��� �κ�. ���๮�ڸ� \n��� <br>�� ����Ѵ�.
                value = value.Replace("<c>", ",");

                String finalvalue = value;
                Int32 n;
                Single f;

                if (int.TryParse(value, out n))
                {
                    finalvalue = n.ToString();
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f.ToString();
                }

                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}