using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.IO;

public static class FileStreamUtility
{
    public static void Write(String pPath,String pData)
    {
        using (FileStream fs = File.Open(pPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            lock (fs)
            {
                fs.SetLength(0);
            }
        }

        using (StreamWriter sw = new StreamWriter(new FileStream(pPath, FileMode.OpenOrCreate)))
        {
            sw.WriteLine(pData);
            sw.Close();
        }
    }

    public static String Read(String pPath)
    {
        String data = null;

        if (!File.Exists(pPath))
            return null;

        using (StreamReader sr = new StreamReader(new FileStream(pPath, FileMode.Open)))
        {
            data = sr.ReadToEnd();
            sr.Close();
        }

        return data;
    }
}