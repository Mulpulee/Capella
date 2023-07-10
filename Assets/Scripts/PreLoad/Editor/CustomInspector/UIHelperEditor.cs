using UnityEngine;
using UnityEditor;
using System;


#if UNITY_EDITOR
[CustomEditor(typeof(UIHelper))]
public class UIHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIHelper test = (UIHelper)target;
        if (GUILayout.Button("Generate"))
        {
            var list = test.Generate();
            UIGeneration.Generate(test.transform.name, list);
        }

        if (GUILayout.Button("AddComponent"))
        {
            Type type = Type.GetType($"{test.transform.name}View,Assembly-CSharp");
            if(type != null)
            {
                if(test.gameObject.GetComponent(type) == null)
                {
                    test.gameObject.AddComponent(type);
                }
                else
                {
                    DestroyImmediate(test.gameObject.GetComponent(type));
                    test.gameObject.AddComponent(type);
                }
            }

            if(test.gameObject.GetComponent<CanvasGroup>() == null)
            {
                test.gameObject.AddComponent<CanvasGroup>();
            }
        }

    }
}
#endif
