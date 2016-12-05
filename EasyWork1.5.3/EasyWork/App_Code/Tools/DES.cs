using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// 数据加密类
/// </summary>
public static class DES
{

    /// <summary>
    /// MD5加密方式
    /// </summary>
    /// <param name="password">原始密码</param>
    /// <returns>返回MD5加密后的字符串</returns>
    public static string MD5Encrypt(string password)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = md5.ComputeHash(passwordBytes, 0, passwordBytes.Length);
        md5.Clear();
        (md5 as IDisposable).Dispose();
        //转换成特定密匙的字符串
        string hashString = ToHexString(hashBytes).ToUpper();

        return hashString;
    }

    /// <summary>
    /// 采用SHA1加密方式
    /// </summary>
    /// <param name="password">原始密码</param>
    /// <returns>返回加密后的字符串</returns>
    public static string SHA1Encrypt(string password)
    {
        SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = sha1.ComputeHash(passwordBytes);
        sha1.Clear();
        (sha1 as IDisposable).Dispose();
        //转换成特定密匙的字符串
        string hashString = ToHexString(hashBytes);

        return hashString;
    }

    /// <summary>
    /// 采用SHA256加密方式
    /// </summary>
    /// <param name="password">原始密码</param>
    /// <returns>返回加密后的字符串</returns>
    public static string SHA256Encrypt(string password)
    {
        //byte[] passwordAndSaltBytes = Encoding.UTF8.GetBytes(password);
        //byte[] hashBytes = new SHA256Managed().ComputeHash(passwordAndSaltBytes);
        //string hashString = ToHexString(hashBytes);
        SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = sha256.ComputeHash(passwordBytes);
        sha256.Clear();
        (sha256 as IDisposable).Dispose();
        //转换成特定密匙的字符串
        string hashString = ToHexString(hashBytes);

        return hashString;
    }

    static string ToHexString(byte[] bytes)
    {
        var hex = new StringBuilder();
        foreach (byte b in bytes)
        {
            hex.AppendFormat("{0:x2}", b);
        }
        return hex.ToString();
    }
}
