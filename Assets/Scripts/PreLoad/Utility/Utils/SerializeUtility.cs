using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SerializeUtility
{
    private static BinaryFormatter _binaryFormatter;

    public static String Serialize<T>(T pInstance)
    {
        String result = null;

        if (_binaryFormatter == null)
            _binaryFormatter = new BinaryFormatter();

        using (var ms = new MemoryStream())
        {
            _binaryFormatter.Serialize(ms,pInstance);
            result = Convert.ToBase64String(ms.ToArray());
        }

        return result;
    }

    public static T Deserialize<T>(String pData) where T : class
    {
        if (String.IsNullOrEmpty(pData))
        {
            UnityEngine.Debug.Log($"SERIALIZEUTILITY :: data is null, returning null");
            return null;
        }

        if (_binaryFormatter == null)
            _binaryFormatter = new BinaryFormatter();

        Byte[] bytes = Convert.FromBase64String(pData);
        using (var ms = new MemoryStream(bytes))
        {
            System.Object obj = _binaryFormatter.Deserialize(ms);
            return (T)obj;
        }
    }
}