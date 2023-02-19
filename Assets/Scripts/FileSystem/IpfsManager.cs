using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using TC;
using UnityEngine;

/// <summary>
/// IPFS‚Æ‚â‚èæ‚è‚ğs‚¤ƒvƒƒOƒ‰ƒ€
/// </summary>
public class IpfsManager : MonoBehaviour
{
    void Start()
    {
        GM.Add<string, string, bool>("DownloadContent", Download);
        GM.Add<string, bool>("UploadContent", Upload);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cid"></param>
    /// <param name="fileName"></param>
    /// <returns>true: ¬Œ÷</returns>
    bool Download(string cid, string fileName)
    {
        var ret = GM.Msg<string>("Exe", $"ipfs get {cid} -o {fileName}.enc");
        
        if (!GM.Msg("DecryptFile", $"{fileName}.enc")) return false;
        
        GM.Log(ret);
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>true: ¬Œ÷</returns>
    bool Upload(string fileName)
    {
        if (!GM.Msg("EncryptFile", fileName)) return false;

        var ret = GM.Msg<string>("Exe", $"ipfs add {fileName}.enc");

        var line = ret.Split("\n")[1];
        var info = line.Split(" ");

        if (info.Length != 3) return false;

        GM.Log($"{info[1]} {info[2]}");

        return true;
    }
}
