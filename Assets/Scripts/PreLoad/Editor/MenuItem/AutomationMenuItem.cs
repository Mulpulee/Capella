using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


public class AutomationMenuItem
{

    [MenuItem("Automation/Generate/Tags")]
    public static void GenerateTags()
    {
        var list = UnityEditorInternal.InternalEditorUtility.tags.ToList();
        ConstGeneration.Generate("Tag", $"AutoTag", list);
    }

    [MenuItem("Automation/Generate/Layer")]
    public static void GenerateLayer()
    {
        var list = UnityEditorInternal.InternalEditorUtility.layers.ToList();
        ConstGeneration.Generate("Layer", $"AutoLayer", list);
    }

    [MenuItem("Automation/Generate/Sound")]
    public static void Generate()
    {
        List<string> returnList = new List<string>();
        AudioClip aud = null;

        object[] temp = Resources.LoadAll<AudioClip>("Sounds");
        for (int i = 0; i < temp.Length; i++)
        {
            aud = temp[i] as AudioClip;
            returnList.Add(aud.name);
        }

        ConstGeneration.Generate("Sound", "AutoSound", returnList);
    }


    [MenuItem("Automation/UI")]
    public static void TestObjectFind()
    {
        UIController.OnUIObjectUpdate();
        EditorSceneUtility.SaveScene();
    }

    [MenuItem("Automation/Generate/DownloadTable")]
    public static void DownloadTable()
    {
        AutoTableGeneration.Generate();
    }
}
