using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;

public static class AssetLoader 
{
    private static String GetAbsolutePath(String pRelativePath)
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Resources/" + pRelativePath;   
#else
        return Application.streamingAssetsPath + pRelativePath;
#endif

    }

    private static Byte[] GetByteArray(String pRelativePath)
    {
        try
        {
            String absolutePath = GetAbsolutePath(pRelativePath);
            Byte[] bytes = System.IO.File.ReadAllBytes(absolutePath);
            return bytes;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }

        return null;
    }



    public static TextAsset GetTextAsset(String pRelativePath)
    {
        try
        {
            String absolute = GetAbsolutePath(pRelativePath);
            StreamReader sr = new StreamReader(absolute);
            String stringSource = sr.ReadToEnd();

            TextAsset testAsset = new TextAsset(stringSource);
            return testAsset;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }

        return null;
    }

    public static Sprite GetSprite(string pRelativePath)
    {
        try
        {
            var bytes = GetByteArray(pRelativePath);

            if (bytes == null)
                throw new NullReferenceException();

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            tex.Compress(true);
            tex.filterMode = FilterMode.Point;
            Sprite fromTex = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);

            if (fromTex == null)
                throw new NullReferenceException();

            fromTex.name = pRelativePath;

            return fromTex;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }

        return null;
    }

}
