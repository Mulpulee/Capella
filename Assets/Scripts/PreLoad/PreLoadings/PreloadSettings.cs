using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PreloadSettings : ScriptableObject
{
    [Header("Scene")]
    public Single SceneLoadFakeDelay = 1;

    [Header("Font")]
    public Font MainFont = null;

    [Header("Sound")]
    public Int32 SoundManagerPoolSize = 100;
    public AudioClip ButtonMouseOverSound = null;
    public AudioClip ButtonClickSound = null;

    [Header("GoogleID")]
    [ReadOnly] public String ClientID = "255441676404-3hb173jjej1jm2nenu2p4nvr1utbmgib.apps.googleusercontent.com";
    [ReadOnly] public String ClientSecret = "VikDUCVIvoQoBIKXVqrTnTz5";

    [Header("SpreadSheets")]
    public String SpreadSheetID;
    public List<String> Sheets;

    [Header("Bases")]
    public Sprite DefaultSprite;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/PreloadSettings")]
    public static void CreateAsset()
        => ScriptableObjectUtility.CreateAsset<PreloadSettings>("PreloadSettings");
#endif
}
