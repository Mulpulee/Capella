using System;
using UnityEngine;


public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));

                if(_instance == null)
                {
                    Debug.LogError("Calling an Empty Singleton which is not IndestructibleSingleton");
                    return null;
                }
            }
            return _instance;
        }
    }
    public static Boolean HasIns() => _instance != null;
}