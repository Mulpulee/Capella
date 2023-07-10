using UnityEngine;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using System.Threading;
using UnityEditor;
using System;
using System.Collections.Generic;
using Extensions;
using System.IO;


#if UNITY_EDITOR


public class SpreadSheetUtility
{

    public static List<Dictionary<String, String>> Donwload(String pSheetName)
    {
        PreloadSettings setting = PreloadingManager.Settings;
        Int32 Maxvalue = setting.Sheets.Count;

        var secret = new ClientSecrets();
        secret.ClientId = setting.ClientID;
        secret.ClientSecret = setting.ClientSecret;

        var scopes = new String[] { SheetsService.Scope.SpreadsheetsReadonly };
        var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(secret, scopes, "user", CancellationToken.None).Result;
        var service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "GoogleCSVDownload"
        });

        var request = service.Spreadsheets.Values.Get(setting.SpreadSheetID, pSheetName);
        var result = request.Execute().Values;
        var sb = new System.Text.StringBuilder();
        for (Int32 row = 0; row < result.Count; row++)
        {
            var data = result[row];

            for (Int32 col = 0; col < data.Count; col++)
            {
                String cell = data[col].ToString();
                if (cell.Contains(","))
                {
                    cell = $"\"{cell}\"";
                }
                sb.Append(cell);
                if (col + 1 < data.Count)
                    sb.Append(",");
            }
            if (row + 1 < result.Count)
                sb.Append("\n");
        }
        Debug.Log(sb.ToString());
        return CSVReader.ReadData(sb.ToString());
    }
}
#endif