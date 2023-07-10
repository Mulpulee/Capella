using UnityEngine;
using UnityEngine.UI;

public class PrintPlayerData : MonoBehaviour
{
    public Save saveScript;
    public Text playerDataText;

    public void OnButtonClick()
    {
        playerDataText.text = "Money: " + saveScript.playerData.money +
                              "\nDay: " + saveScript.playerData.day +
                              "\nRecipe: " + ArrayToString(saveScript.playerData.recipe) +
                              "\nCustomer: " + ArrayToString(saveScript.playerData.customer) +
                              "\nBGM Volume: " + saveScript.playerData.bgmVolume.ToString("F2") +
                              "\nSFX Volume: " + saveScript.playerData.sfxVolume.ToString("F2");
    }

    private string ArrayToString(int[] array)
    {
        string result = "[";
        for (int i = 0; i < array.Length; i++)
        {
            result += array[i].ToString();
            if (i < array.Length - 1)
                result += ", ";
        }
        result += "]";
        return result;
    }
}