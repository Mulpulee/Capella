using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisappearBlack : MonoBehaviour
{
    public Image DisappearScreen;
    private float Disappearalpha = 1;

    private void Start()
    {
        StartCoroutine(ScreenDisappear());
    }
    private IEnumerator ScreenDisappear()
    {
        while (DisappearScreen.color.a >= 0)
        {
            DisappearScreen.color = new Color(0, 0, 0, Disappearalpha);
            yield return new WaitForFixedUpdate();

            Disappearalpha -= 0.01f;
        }
    }
}