using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
public class CustomMenuItem
{
    [MenuItem("GameObject/UI/Preload/BloomingButton", false)]
    public static void CreateBloomingButton(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("BloomingButton");
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
        go.AddComponent<Image>().sprite = default;
        go.AddComponent<Button>();
        go.AddComponent<EventTrigger>();
        go.AddComponent<CustomBloomingButton>();


        GameObject textGo = new GameObject("BloomingButtonText");
        var ct = textGo.AddComponent<CustomText>();
        ct.text = "Hello Button!";
        ct.alignment = TextAnchor.MiddleCenter;
        ct.fontSize = 40;
        ct.font = CustomText.MainFont;
        RectTransform rt = textGo.GetComponent<RectTransform>();

        rt.localPosition = new Vector3(0, 0, 0);
        rt.anchoredPosition = new Vector3(0, 0, 0);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = rt.rect.size;

        rt.transform.SetParent(go.transform);

        rt.localPosition = new Vector3(0, 0, 0);
        rt.anchoredPosition = new Vector3(0,0,0);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = rt.rect.size;


    }

    [MenuItem("GameObject/UI/Preload/CustomButton", false)]
    public static void CreateCustomButton(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("CustomButton");
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
        go.AddComponent<Image>().sprite = default;
        go.AddComponent<Button>();
        go.AddComponent<CustomButton>();
    }

    [MenuItem("GameObject/UI/Preload/CustomText", false)]
    public static void CreateCustomText(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("CustomText");
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
        var ct = go.AddComponent<CustomText>();
        ct.raycastTarget = false;
        ct.text = "This is Error";
        ct.fontSize = 30;
        ct.font = CustomText.MainFont;
    }

}
#endif
