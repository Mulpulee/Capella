using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Save : MonoBehaviour
{
    public Playerdata playerData;

    [ContextMenu("To Json Data")]
    public void SavePlayerDataToJson()
    {
        string path = Path.Combine(Application.dataPath, "PlayerData.json");

        string jsonData;
        if (File.Exists(path)) jsonData = JsonUtility.ToJson(playerData, true);
        else jsonData = "{\r\n    \"money\": 0,\r\n    \"day\": 1,\r\n    \"recipe\": [ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],\r\n    \"customer\": [ 0, 0, 0, 0, 0 ],\r\n    \"mstVolume\": 0.0,\r\n    \"bgmVolume\": 0.0,\r\n    \"sfxVolume\": 0.0\r\n}";

        File.WriteAllText(path, jsonData);
    }
    [ContextMenu("from Json Data")]
    public void LoadPlayerDataFromJson()
    {
        string path = Path.Combine(Application.dataPath, "playerData.json");

        if(!File.Exists(path)) SavePlayerDataToJson();

        string jsonData = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<Playerdata>(jsonData);
    }
}
[System.Serializable]
public class Playerdata
{
    public int money;
    public int day;
    public int[] recipe = new int[29];
    public int[] customer = new int[5];
    public float mstVolume;
    public float bgmVolume;
    public float sfxVolume;
}