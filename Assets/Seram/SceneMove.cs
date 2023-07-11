using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{
    [SerializeField] 

    private Image BlackScrean1;
    private Image BlackScrean2;
    private float alpha1 = 0;
    private float alpha2 = 1;

    private IEnumerator ScreanWaiting()
    {
        while (BlackScrean1.color.a < 1)
        {
            BlackScrean1.color = new Color(0, 0, 0, alpha1);
            yield return new WaitForFixedUpdate();

            alpha1 += 0.01f;
        }
        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("MoveScene");

        while (BlackScrean2.color.a > 0)
        {
            BlackScrean2.color = new Color(0, 0, 0, alpha2);
            yield return new WaitForFixedUpdate();
            alpha2 -= 0.01f;
        }
        
    }
    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            
            
            StartCoroutine(ScreanWaiting());

            
            
        }
    }
}   