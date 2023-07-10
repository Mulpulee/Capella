using System;
using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    [SerializeField] private Boolean _isInitialized;
    protected abstract void OnSingletonInstantiated();

    public void Init()
    {
        if (_isInitialized)
            return;

        OnSingletonInstantiated();
        _isInitialized = true;
    }
}

public abstract class IndestructibleSingleton<T> : Manager where T : Manager
{
    private static T _Instance;

    public static T ins => GetInstance();

    public static T GetInstance()
    {
        if (_Instance == null)
        {
            _Instance = (T)FindObjectOfType(typeof(T));

            if (_Instance == null)
            {
                var singletonObject = new GameObject();
                _Instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).ToString() + " ( Indestructible Singleton )";
                DontDestroyOnLoad(singletonObject);
                _Instance.Init();
            }
        }
        return _Instance;
    }
}