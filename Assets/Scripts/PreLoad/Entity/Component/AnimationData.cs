using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using System.Reflection;
using UnityEditor;
#endif


[Serializable]
public class AnimationData : ScriptableObject
{
    public List<Sprite> sprites;
    public Single spritePerSeconds = 16;
    public Boolean loop = true;
    public Single exitTime;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Custom/AnimationData")]
    public static void CreateAnimationData()
    {
        var assetInstance = ScriptableObject.CreateInstance<AnimationData>();
        assetInstance.name = "AnimationData";

        EditorUtility.SetDirty(assetInstance);

        Type projectWindowUtilType = typeof(ProjectWindowUtil);
        MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
        string pathToCurrentFolder = getActiveFolderPath.Invoke(null, new object[0]).ToString();
        
        AssetDatabase.CreateAsset(assetInstance, Path.Combine(pathToCurrentFolder,$"{assetInstance.name}.asset"));
    }
#endif
}