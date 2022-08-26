using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

/// <summary>
/// Summary description for Encryption
/// </summary>
public class Encryption
{
    private const string STR_ENCRIPT = "&%#@?,@*";

    public Encryption()
    {
        //
        // 
        //
    }

    private string Encrypt(string strText, string strEncrypt)
    {
        byte[] byKey = new byte[20];
        byte[] dv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        try
        {
            byKey = System.Text.Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputArray = System.Text.Encoding.UTF8.GetBytes(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, dv), CryptoStreamMode.Write); 
            cs.Write(inputArray, 0, inputArray.Length);
            cs.FlushFinalBlock(); 

            return Convert.ToBase64String(ms.ToArray());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string Decrypt(string strText, string strEncrypt)
    {
        byte[] bKey = new byte[20];
        byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        try
        {
            bKey = System.Text.Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] inputByteArray = inputByteArray = Convert.FromBase64String(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(bKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock(); 
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;

            return encoding.GetString(ms.ToArray());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string encryptPassword(string strText)
    {
        return Encrypt(strText, STR_ENCRIPT);
    }

    public string decryptPassword(string str)
    {
        return Decrypt(str, STR_ENCRIPT);
    }

    public static bool IsStrongPassword(string password)
    {
        // Minimum and Maximum Length of field - 6 to 12 Characters
        if (password.Length < 6 || password.Length > 25)
            return false;

        // Special Characters - Not Allowed
        // Spaces - Not Allowed
        if (!(password.All(c => char.IsLetter(c) || char.IsDigit(c))))
            return false;

        // Numeric Character - At least one character
        if (!password.Any(c => char.IsDigit(c)))
            return false;

        // At least one Capital Letter
        if (!password.Any(c => char.IsUpper(c)))
            return false;

        //// Repetitive Characters - Allowed only two repetitive characters
        //var repeatCount = 0;
        //var lastChar = '\0';
        //foreach (var c in password)
        //{
        //    if (c == lastChar)
        //        repeatCount++;
        //    else
        //        repeatCount = 0;
        //    if (repeatCount == 2)
        //        return false;
        //    lastChar = c;
        //}

        return true;
    }
}
