using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;

public class CustomText : Text
{
    [SerializeField] Font customFont = null;

#if UNITY_EDITOR
    [MenuItem("Utility/UI/CustomFont/UpdateFont")]
    public static void UpdateFont()
    {
        var Fonts = FindObjectsOfType<CustomText>();
        Font newFont = PreloadingManager.Settings.MainFont;
        foreach (var item in Fonts)
        {
            if(item.customFont != null)
                item.font = newFont;
        }
    }
#endif

    private static Font _mainFont;
    public static Font MainFont {
        get
        {
            if (_mainFont == null)
                _mainFont = PreloadingManager.Settings.MainFont;
            return _mainFont;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if(customFont == null)
            this.font = MainFont;
    }
}
