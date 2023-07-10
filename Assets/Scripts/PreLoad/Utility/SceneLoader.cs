using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    inGame,
    MainScene,
}

public static class SceneLoader 
{
    public static Action onSceneLoaded;

    public static void LoadScene(String pSceneName)
    {
        onSceneLoaded?.Invoke();
        LoadingScene.SetNextScene(pSceneName);
        SceneManager.LoadScene("LoadingScene");
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
