using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
public class StringEncrypter
{
    private String Key_String = "5야옹";

    private byte[] stringToByte(string value)
    {
        Byte[] buffer = new byte[value.Length];

        for (int i = 0; i < value.Length; i++)
        {
            buffer[i] = Convert.ToByte(value.Substring(i, 1));
        }
        return buffer;
    }

    private string Decrypt(byte[] CypherText, SymmetricAlgorithm key)
    {
        // Create a memory stream to the passed buffer.
        MemoryStream ms = new MemoryStream(CypherText);

        // Create a CryptoStream using the memory stream and the
        // CSP DES key.
        CryptoStream encStream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);

        // Create a StreamReader for reading the stream.
        StreamReader sr = new StreamReader(encStream);

        // Read the stream as a string.
        string val = sr.ReadLine();

        // Close the streams.
        sr.Close();
        encStream.Close();
        ms.Close();

        return val;
    }

    public static byte[] Encrypt(string PlainText, SymmetricAlgorithm key)
    {
        // Create a memory stream.
        MemoryStream ms = new MemoryStream();

        // Create a CryptoStream using the memory stream and the
        // CSP DES key. 
        CryptoStream encStream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);

        // Create a StreamWriter to write a string
        // to the stream.
        StreamWriter sw = new StreamWriter(encStream);

        // Write the plaintext to the stream.
        sw.WriteLine(PlainText);

        // Close the StreamWriter and CryptoStream.
        sw.Close();
        encStream.Close();

        // Get an array of bytes that represents
        // the memory stream.
        byte[] buffer = ms.ToArray();

        // Close the memory stream.
        ms.Close();

        // Return the encrypted byte array.
        return buffer;
    }

    public byte[] Des_Encrypt(string inputvalue)
    {
        string output;

        // Create a new DES key.
        DESCryptoServiceProvider key = new DESCryptoServiceProvider();

        key.Key = stringToByte(Key_String);
        key.IV = stringToByte(Key_String);

        // Encrypt a string to a byte array.
        byte[] buffer = Encrypt(inputvalue, key);

        return buffer;
    }

    public string Des_Dncrypt(byte[] inputvalue)
    {
        string output;

        // Create a new DES key.
        DESCryptoServiceProvider key = new DESCryptoServiceProvider();

        key.Key = stringToByte(Key_String);
        key.IV = stringToByte(Key_String);

        // Encrypt a string to a byte array.
        output = Decrypt(inputvalue, key);

        return output;
    }
}