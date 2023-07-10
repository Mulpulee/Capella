using UnityEngine;
using UnityEditor;
using Extensions;


#if UNITY_EDITOR
[CustomEditor(typeof(CanvasGroup))]
public class CanvasGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CanvasGroup canvasGroup = (CanvasGroup)target;
        if (GUILayout.Button("Show"))
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            EditorSceneUtility.SaveScene();
        }
        if (GUILayout.Button("Hide"))
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            EditorSceneUtility.SaveScene();
        }
    }
}
#endif
