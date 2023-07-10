using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class PlayerPrefUtility
{
    public static void Save<T>(String name, T instance)
    {
        using (var ms = new MemoryStream())
        {
            new BinaryFormatter().Serialize(ms, instance);
            PlayerPrefs.SetString(name, Convert.ToBase64String(ms.ToArray()));
        }
    }

    public static T Load<T>(String name) where T : new()
    {
        if (!PlayerPrefs.HasKey(name)) 
            return new T();

        Byte[] bytes = Convert.FromBase64String(PlayerPrefs.GetString(name));
        using (var ms = new MemoryStream(bytes))
        {
            System.Object obj = new BinaryFormatter().Deserialize(ms);
            return (T)obj;
        }
    }
}