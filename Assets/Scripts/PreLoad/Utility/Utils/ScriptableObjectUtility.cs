using UnityEngine;
using System.IO;
using System;

#if UNITY_EDITOR
	using UnityEditor;
#endif

public static class ScriptableObjectUtility
{
#if UNITY_EDITOR
	public static void CreateAsset<T>(String name, String path = null) where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T>();

        if(path == null)
        {
            path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
        }

        String assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{path}/{name}.asset");

		AssetDatabase.CreateAsset(asset, assetPathAndName);

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}
#endif
}