using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{
    [SerializeField] 

    public Image BlackScreen;
    private float alpha = 0;

    private IEnumerator ScreenChange()
    {
        while (BlackScreen.color.a < 1)
        {
            BlackScreen.color = new Color(0, 0, 0, alpha);
            yield return new WaitForFixedUpdate();

            alpha += 0.01f;
        }
        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("MoveScene");
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {            
            StartCoroutine(ScreenChange());
        }
    }
}   