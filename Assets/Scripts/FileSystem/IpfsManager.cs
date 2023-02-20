using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using TC;
using UnityEngine;

/// <summary>
/// IPFSÇ∆Ç‚ÇËéÊÇËÇçsÇ§ÉvÉçÉOÉâÉÄ
/// </summary>
public class IpfsManager : MonoBehaviour
{
    void Start()
    {
        GM.Add<string, UniTask<bool>>("UploadContent", Upload);
        GM.Add<string, string, UniTask<bool>>("DownloadContent", Download);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>true: ê¨å˜</returns>
    async UniTask<bool> Upload(string fileName)
    {
        var path = GetPath(fileName);
        if (!GM.Msg<bool>("EncryptFile", path)) return false;
        var ret = await GM.Msg<UniTask<string>>("Exe", "ipfs", $"add \"{path}.enc\"");

        GM.Log($"{ret}");

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cid"></param>
    /// <param name="fileName"></param>
    /// <returns>true: ê¨å˜</returns>
    async UniTask<bool> Download(string cid, string fileName)
    {
        var path = GetPath(fileName);
        //var ret = await GM.Msg<UniTask<string>>("Exe", "ipfs", $"get {cid} -o \"{path}.enc\"");
        
        if (!GM.Msg<bool>("DecryptFile", $"{path}.enc")) return false;
        
        //GM.Log(ret);
        return true;
    }

    

    string GetPath(string fileName)
    {
        return $"{Application.dataPath}/StreamingAssets/worlds/{fileName}";
    }
}
