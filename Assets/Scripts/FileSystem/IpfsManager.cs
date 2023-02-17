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
        GM.Add<string, string>("Download", Download);
        GM.Add<string, bool>("Upload", Upload);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cid">Content ID</param>
    /// <param name="fileName"></param>
    void Download(string cid, string fileName)
    {
        var ret = GM.Msg<string>("Exe", $"ipfs get {cid} -o {fileName}");
        GM.Log(ret);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    bool Upload(string fileName)
    {
        var ret = GM.Msg<string>("Exe", $"ipfs add {fileName}");
        

        var line = ret.Split("\n")[1];
        var info = line.Split(" ");

        if (info.Length != 3) return false;

        GM.Log($"{info[1]} {info[2]}");

        return true;
    }
}
