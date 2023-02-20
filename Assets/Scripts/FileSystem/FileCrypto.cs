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
    /// �Í���
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool Encrypt(string path)
    {        
        Rfc2898DeriveBytes keyDerive = new Rfc2898DeriveBytes(password, salt);
        byte[] key = keyDerive.GetBytes(32);
        byte[] iv = keyDerive.GetBytes(16);

        // �t�@�C���̓��e���o�C�g�z��Ƃ��ēǂݍ���
        byte[] inputBytes = File.ReadAllBytes(path);

        // AES�Í����I�u�W�F�N�g�����
        Aes aes = Aes.Create();

        // �Í������ꂽ�f�[�^���i�[����o�C�g�z������
        byte[] encryptedData;

        // �Í����X�g���[�������
        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(salt, 0, salt.Length); // �\���g����������

            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                cs.Write(inputBytes, 0, inputBytes.Length); // �f�[�^����������
                cs.FlushFinalBlock(); // �ŏI�u���b�N�̏���
            }

            encryptedData = ms.ToArray(); // �Í������ꂽ�f�[�^���擾����
        }

        // �Í������ꂽ�f�[�^��ʂ̃t�@�C���ɏ������ށi�g���q��.enc�ɂ���j
        var newFileName = path + ".enc";
        File.WriteAllBytes(newFileName, encryptedData);

        return true;
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool Decrypt(string path)
    {
        // �o�͂���t�@�C����
        string outputFileName = path.Substring(0, path.LastIndexOf('.'));

        // �Í������ꂽ�t�@�C������\���g�ƃf�[�^��ǂݍ���
        byte[] encryptedData = File.ReadAllBytes(path);
        Array.Copy(encryptedData, 0, salt, 0, salt.Length);
        byte[] data = new byte[encryptedData.Length - salt.Length];
        Array.Copy(encryptedData, salt.Length, data, 0, data.Length);

        // �p�X���[�h�ƃ\���g����Rfc2898DeriveBytes�I�u�W�F�N�g�����
        Rfc2898DeriveBytes keyDerive = new Rfc2898DeriveBytes(password, salt);
        byte[] key = keyDerive.GetBytes(32);
        byte[] iv = keyDerive.GetBytes(16);

        // Aes�I�u�W�F�N�g�����
        Aes aes = Aes.Create();

        // ���������ꂽ�f�[�^���i�[����o�C�g�z������
        byte[] decryptedData;

        // �������X�g���[�������
        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms,
                aes.CreateDecryptor(key, iv), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length); // �f�[�^����������
                cs.FlushFinalBlock(); // �ŏI�u���b�N�̏���
            }

            decryptedData = ms.ToArray(); // ���������ꂽ�f�[�^���擾����
        }

        // ���������ꂽ�f�[�^��ʂ̃t�@�C���ɏ�������
        File.WriteAllBytes(outputFileName, decryptedData);

        return true;
    }
}
