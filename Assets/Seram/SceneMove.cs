using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{
    [SerializeField] private GameObject nemo;
    private string nextScene;

    private float alpha = 0;

    public void ChangeScene(string _scene, Action callback, Action changedCallback)
    {
        nextScene = _scene;
        StartCoroutine(ScreenChange(callback, changedCallback));
    }

    private IEnumerator ScreenChange(Action callback, Action changedCallback)
    {
        SpriteRenderer screen = Instantiate(nemo).GetComponent<SpriteRenderer>();
        while (screen.color.a < 1)
        {
            screen.color = new Color(0, 0, 0, alpha);
            yield return new WaitForFixedUpdate();

            alpha += 0.01f;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);

        while (!operation.isDone) yield return null;
        changedCallback.Invoke();

        screen = Instantiate(nemo).GetComponent<SpriteRenderer>();
        screen.color = Color.black;
        yield return new WaitForSeconds(2.0f);

        while (screen.color.a > 0)
        {
            screen.color = new Color(0, 0, 0, alpha);
            yield return new WaitForFixedUpdate();

            alpha -= 0.01f;
        }

        callback.Invoke();
    }
}   