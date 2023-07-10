using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PreloadingManager 
{
    private static PreloadSettings _settings;

    public static PreloadSettings Settings
    {
        get
        {
            if (_settings == null)
                _settings = Resources.Load("PreloadSettings") as PreloadSettings;
            return _settings;
        }
    }
}
