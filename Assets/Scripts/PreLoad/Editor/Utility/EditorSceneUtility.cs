using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

#if UNITY_EDITOR
public static class EditorSceneUtility
{

    public static void SaveScene()
    {
        if (UnityEditor.EditorApplication.isPlaying)
            return;

        Scene currentScene = SceneManager.GetActiveScene();

        if (!currentScene.isDirty)
            EditorSceneManager.MarkSceneDirty(currentScene);

        if (!EditorSceneManager.SaveScene(currentScene))
            UnityEngine.Debug.LogError("WARNING: Scene Not Saved!!!");
    }
}

#endif