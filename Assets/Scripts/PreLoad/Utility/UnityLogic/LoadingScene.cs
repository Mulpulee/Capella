using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScene : MonoBehaviour
{
    private static string SceneName = "inGame";

    [SerializeField] Image FadeImage;
    [SerializeField] CanvasGroup HideGroup;
    [SerializeField] private CustomText m_loadText;
    void Start() => StartCoroutine(LoadAsynSceneCoroutine());
    public static void SetNextScene(string name) => SceneName = name;
    IEnumerator LoadAsynSceneCoroutine()
    {
        yield return new WaitForSeconds(PreloadingManager.Settings.SceneLoadFakeDelay);

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            FadeImage.fillAmount = Mathf.Lerp(FadeImage.fillAmount, operation.progress,0.5f);

            if (operation.progress >= 0.9f)
            {
                FadeImage.fillAmount = 1f;
                OnLoad(operation);
                yield break;
            }

            yield return null;
        }
    }

    private void OnLoad(AsyncOperation pOperation)
    {
        m_loadText.text = "LOAD COMPLETE!";

        HideGroup.DOFade(1, 1F).OnComplete(() =>
        {
            HideGroup.DOFade(0,1f).OnComplete(() =>
            {
                pOperation.allowSceneActivation = true;
            });
        });
    }
}