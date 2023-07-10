using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

[System.Serializable]
public class Save : MonoBehaviour
{
    public Playerdata playerData;

    [ContextMenu("To Json Data")]
    void SavePlayerDataToJson()
    {
        string jsonData = JsonUtility.ToJson(playerData);
        string path = Path.Combine(Application.dataPath, "PlayerData.json");
        File.WriteAllText(path, jsonData);
    }
    [ContextMenu("from Json Data")]
    void LordPlayerDataFromJson()
    {
        string path = Path.Combine(Application.dataPath, "playerData.json");
        string jsonData = File.ReadAllText(path);
        playerData=JsonUtility.FromJson<Playerdata>(jsonData);
    }
}
[System.Serializable]
public class Playerdata
{
    public int money;
    public int day;
    public int[] recipe;
    public int[] customer;
    public float bgmVolume;
    public float sfxVolume;
}