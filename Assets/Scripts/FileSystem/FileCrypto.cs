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
    const string password = "";
    const string saltStr = "";

    byte[] salt;
    void Start()
    {
        byte[] saltByte = System.Text.Encoding.UTF8.GetBytes(saltStr);
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
        // ファイル名とパスワードを指定
        string fileName = "test.txt";

        // パスワードからキーとIVを生成
        //byte[] salt = new byte[8];
        //using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        //{
        //    rng.GetBytes(salt);
        //}
        Rfc2898DeriveBytes keyDerive = new Rfc2898DeriveBytes(password, salt);
        byte[] key = keyDerive.GetBytes(16);
        byte[] iv = keyDerive.GetBytes(16);

        // ファイルの内容をバイト配列として読み込む
        byte[] data = File.ReadAllBytes(fileName);

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
                cs.Write(data, 0, data.Length); // データを書き込む
                cs.FlushFinalBlock(); // 最終ブロックの処理
            }

            encryptedData = ms.ToArray(); // 暗号化されたデータを取得する
        }

        // 暗号化されたデータを別のファイルに書き込む（拡張子は.encにする）
        var newFileName = fileName + ".enc";
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
        // 復号化したファイル名
        string outputFile = path;

        // ファイルをバイト配列に読み込む
        byte[] inputBytes = File.ReadAllBytes(path);



        // パスワードから鍵と初期化ベクトルを生成する
        Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, salt);
        byte[] key = deriveBytes.GetBytes(32); // 鍵長256ビット
        byte[] iv = deriveBytes.GetBytes(16);  // ブロックサイズ128ビット

        // AES暗号化オブジェクトの作成
        RijndaelManaged aes = new RijndaelManaged();
        aes.KeySize = 256;     // 鍵長256ビット
        aes.BlockSize = 128;   // ブロックサイズ128ビット
        aes.Mode = CipherMode.CBC;   // CBCモード
        aes.Padding = PaddingMode.PKCS7;  // パディング

        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs =
                    new CryptoStream(ms, aes.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                }
                byte[] outputBytes = ms.ToArray();

                File.WriteAllBytes(outputFile, outputBytes);
            }
            Console.WriteLine("復号化しました。");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("エラー: {0}", ex.Message);
            return false;
        }
    }
}
