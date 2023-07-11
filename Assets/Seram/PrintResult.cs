using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrintResult : MonoBehaviour
{
    private float materialCost = 100;
    private float income = 10;
    private float tip = 10;
    private float money = 2000;

    [SerializeField]
    private TextMeshProUGUI textMeshPro;
    private void Start()
    {
        StartCoroutine(OutputDelayedValues());
    }

    private IEnumerator OutputDelayedValues()
    {
        textMeshPro.text = string.Join(" ", "materialCost: ", materialCost, "\n");
        yield return new WaitForSeconds(2.0f);
        textMeshPro.text += string.Join(" ", "income: ", income, "\n");
        yield return new WaitForSeconds(2.0f);
        textMeshPro.text += string.Join(" ", "tip: ", tip, "\n");
        yield return new WaitForSeconds(2.0f);
        textMeshPro.text += string.Join(" ", "money: ", money, "\n");
    }
}