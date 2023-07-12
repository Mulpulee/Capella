using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextDelay : MonoBehaviour
{
    [SerializeField] private string[] text;
    public Text targetText;
    private float delay = 0.125f;
    private bool isTyping;
    private int index = 0;

    private void Start()
    {
        StartCoroutine(TextPrint(delay));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopCoroutine(TextPrint(delay));
                targetText.text = text[index];
                isTyping = false;
            }
            else
            {
                if (index < text.Length - 1)
                {
                    index++;
                    StartCoroutine(TextPrint(delay));
                }
            }
        }
    }

    IEnumerator TextPrint(float delay)
    {
        isTyping = true;
        targetText.text = "";

        for (int count = 0; count < text[index].Length; count++)
        {
            if(!isTyping) {  break; }
            targetText.text += text[index][count].ToString();
            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
    }
}
