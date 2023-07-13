using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextDelay : MonoBehaviour
{
    [SerializeField] private string[] text;
    public Text targetText;
    private float delay = 12.5f;
    private bool isTyping;
    private int index = 0;

    private CustomerManager cm;

    private void Start()
    {
        cm = GetComponent<CustomerManager>();
    }

    public void SetTexts(string[] _text)
    {
        text = _text;
    }

    public void StartPrint(float _delay)
    {
        index = 0;
        delay = _delay;
        StartCoroutine(TextPrint(delay));
    }

    private void Update()
    {
        if (index < text.Length && Input.GetKeyDown(KeyCode.Space))
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
                else
                {
                    cm.EndDialog();
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
            yield return new WaitForSeconds(delay/100f);
        }

        isTyping = false;
    }
}
