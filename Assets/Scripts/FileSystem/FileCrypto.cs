using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TC;
using UnityEngine;

public class FileCrypto : MonoBehaviour
{
    const string password = "ga20i4g9j90)U)#rAFW";
    const string saltStr = "gwoigjoaw";

    byte[] salt;
    void Start()
    {
        salt = System.Text.Encoding.UTF8.GetBytes(saltStr);
        GM.Add<string, bool>("EncryptFile", Encrypt);
        GM.Add<string, bool>("DecryptFile", Decrypt);
    }

    /// <summary>
    /// 暗号化
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool Encrypt(string path)
    {        
        Rfc2898DeriveBytes keyDerive = new Rfc2898DeriveBytes(password, salt);
        byte[] key = keyDerive.GetBytes(32);
        byte[] iv = keyDerive.GetBytes(16);

        // ファイルの内容をバイト配列として読み込む
        byte[] inputBytes = File.ReadAllBytes(path);

        // AES暗号化オブジェクトを作る
        Aes aes = Aes.Create();

        // 暗号化されたデータを格納するバイト配列を作る
        byte[] encryptedData;

        // 暗号化ストリームを作る
        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(salt, 0, salt.Length); // ソルトを書き込む

            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                cs.Write(inputBytes, 0, inputBytes.Length); // データを書き込む
                cs.FlushFinalBlock(); // 最終ブロックの処理
            }

            encryptedData = ms.ToArray(); // 暗号化されたデータを取得する
        }

        // 暗号化されたデータを別のファイルに書き込む（拡張子は.encにする）
        var newFileName = path + ".enc";
        File.WriteAllBytes(newFileName, encryptedData);

        return true;
    }

    /// <summary>
    /// 復号化
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool Decrypt(string path)
    {
        // 出力するファイル名
        string outputFileName = path.Substring(0, path.LastIndexOf('.'));

        // 暗号化されたファイルからソルトとデータを読み込む
        byte[] encryptedData = File.ReadAllBytes(path);
        Array.Copy(encryptedData, 0, salt, 0, salt.Length);
        byte[] data = new byte[encryptedData.Length - salt.Length];
        Array.Copy(encryptedData, salt.Length, data, 0, data.Length);

        // パスワードとソルトからRfc2898DeriveBytesオブジェクトを作る
        Rfc2898DeriveBytes keyDerive = new Rfc2898DeriveBytes(password, salt);
        byte[] key = keyDerive.GetBytes(32);
        byte[] iv = keyDerive.GetBytes(16);

        // Aesオブジェクトを作る
        Aes aes = Aes.Create();

        // 復号化されたデータを格納するバイト配列を作る
        byte[] decryptedData;

        // 復号化ストリームを作る
        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms,
                aes.CreateDecryptor(key, iv), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length); // データを書き込む
                cs.FlushFinalBlock(); // 最終ブロックの処理
            }

            decryptedData = ms.ToArray(); // 復号化されたデータを取得する
        }

        // 復号化されたデータを別のファイルに書き込む
        File.WriteAllBytes(outputFileName, decryptedData);

        return true;
    }
}
