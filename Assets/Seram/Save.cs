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
        string jsonData = JsonUtility.ToJson(playerData, true);
        string path = Path.Combine(Application.dataPath, "PlayerData.json");
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
    public int[] recipe;
    public int[] customer;
    public float mstVolume;
    public float bgmVolume;
    public float sfxVolume;
}