using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using System.Threading;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class AutoTableGeneration
{
    public static void Generate()
    {
        Int32 count = 0;

        List<String> tableNames = new List<String>();
        List<KeyValuePair<String,String>> tableDatas = DownloadTables();
        Dictionary<String,List<String>[]> columnByTable = GetTableColumns(tableDatas);

        String assetPath = UnityEngine.Application.dataPath + "/Resources/DataTableAsset";

        DirectoryInfo directory = new DirectoryInfo(assetPath);

        if (!directory.Exists)
            directory.Create();

        foreach (var item in columnByTable)
        {
            String typename = $"{item.Key}Row";
            DataTableGeneration.Generate("DataTable", typename, item.Value);

            Type generatedType = Type.GetType($"Automation.DataTable.{typename},Assembly-CSharp");
            DataTableAssetGeneration.Generate("DataTableAsset", $"{item.Key}Asset", generatedType);


            Object Table = DataTableParser.ReadDynamic(tableDatas[count].Value, generatedType);
            Type constructedType = typeof(DataTable<>).MakeGenericType(generatedType);

            Object dataTable = Activator.CreateInstance(constructedType, Table);

            Type assetType = Type.GetType($"{item.Key}Asset,Assembly-CSharp");
            GenerateAsset(assetType, $"{item.Key}Asset", dataTable);


            tableNames.Add(item.Key);

            count++;
        }

        ConstGeneration.Generate("DataTable", "AutoDataTable", tableNames);

        AssetDatabase.Refresh();
    }

    private static List<KeyValuePair<String,String>> DownloadTables()
    {
        List<KeyValuePair<String, String>> resultList = new List<KeyValuePair<String, String>>();

        for (Int32 i = 0,iCount = PreloadingManager.Settings.Sheets.Count; i < iCount; i++)
        {
            var secret = new ClientSecrets();
            secret.ClientId = PreloadingManager.Settings.ClientID;
            secret.ClientSecret = PreloadingManager.Settings.ClientSecret;

            var scopes = new String[] { SheetsService.Scope.SpreadsheetsReadonly };
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(secret, scopes, "user", CancellationToken.None).Result;
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleCSVDownload"
            });

            var request = service.Spreadsheets.Values.Get(PreloadingManager.Settings.SpreadSheetID, PreloadingManager.Settings.Sheets[i]);
            var result = request.Execute().Values;
            var sb = new System.Text.StringBuilder();

            for (Int32 row = 0; row < result.Count; row++)
            {
                var data = result[row];

                for (Int32 col = 0; col < data.Count; col++)
                {
                    String cell = data[col].ToString();

                    if (cell.Contains(","))
                        cell = $"\"{cell}\"";

                    sb.Append(cell);

                    if (col + 1 < data.Count)
                        sb.Append(",");
                }
                if (row + 1 < result.Count)
                    sb.Append("\n");
            }

            UnityEngine.Debug.Log(sb.ToString());
            resultList.Add(new KeyValuePair<String, String>(PreloadingManager.Settings.Sheets[i], sb.ToString()));
        }

        return resultList;
    }

    private static Dictionary<String, List<String>[]> GetTableColumns(List<KeyValuePair<String, String>> datas)
    {
        String ID = "ID";

        String SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        String LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

        Dictionary<String, List<String>[]> resultList = new Dictionary<String, List<String>[]>();

        for (Int32 i = 0, iCount = datas.Count; i < iCount; i++)
        {
            var lines = Regex.Split(datas[i].Value, LINE_SPLIT_RE);

            List<String> header = new List<String>(Regex.Split(lines[0], SPLIT_RE));
            List<String> types = new List<String>(Regex.Split(lines[1], SPLIT_RE));

            if (!header.Contains(ID))
                continue;

            Int32 index = header.IndexOf(ID);
            header.Remove(ID);
            types.RemoveAt(index);

            resultList.Add(datas[i].Key, new List<String>[2] { header, types });
        }
        return resultList;
    }

    private static void GenerateAsset(Type pType, String pFilename, Object pTableValue)
    {
        String assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/DataTableAsset/{pFilename}.asset");

        var check = UnityEngine.Resources.Load($"DataTableAsset/{pFilename}");
        if (check != null)
        {
            pType.GetField("Table").SetValue(check, pTableValue);
            EditorUtility.SetDirty(check);
            return;
        }

        UnityEngine.ScriptableObject asset = UnityEngine.ScriptableObject.CreateInstance(pType);
        pType.GetField("Table").SetValue(asset, pTableValue);
        EditorUtility.SetDirty(asset);

        AssetDatabase.CreateAsset(asset, assetPathAndName);
    }
}
