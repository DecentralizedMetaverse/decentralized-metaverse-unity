using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TC;
using UnityEngine;

/// <summary>
/// World���\�z����
/// 
/// world table
/// id: chunk x, chunk z
/// </summary>
public class WorldGenerator : MonoBehaviour
{
    Dictionary<(int, int), string> rootCids = new(16)
    {
        {(0, 0), "QmQNgTGwkZ4MDicM1vR8sUPyTYocC8gJtJ7DVBU5qrt79C"}
    };

    Dictionary<string, Dictionary<string, object>> configs = new();

    private void Awake()
    {
        GM.Add<int, int, UniTask<GameObject>>("GenerateWorld", GenerateWorld);
    }

    /// <summary>
    /// ���[���h����
    /// </summary>
    /// <param name="loadX"></param>
    /// <param name="loadY"></param>
    /// <returns></returns>
    async UniTask<GameObject> GenerateWorld(int loadX, int loadY)
    {
        // IPFS����R���e���c���_�E�����[�h����
        // world table���K�v
        await DownloadWorldConfig(loadX, loadY);
        return null;
    }

    /// <summary>
    /// ���[���h���̓ǂݍ���
    /// </summary>
    /// <param name="loadX"></param>
    /// <param name="loadY"></param>
    async UniTask DownloadWorldConfig(int loadX, int loadY)
    {
        var rootConfigCID = rootCids[(loadX, loadY)];

        await DownloadContentAsync(rootConfigCID);
    }

    /// <summary>
    /// Config�t�@�C���_�E�����[�h
    /// </summary>
    /// <param name="configCID"></param>
    /// <returns></returns>
    async UniTask DownloadContentAsync(string configCID)
    {
        if(configs.ContainsKey(configCID)) { return; }

        // Config�t�@�C���_�E�����[�h
        await GM.MsgAsync("DownloadContent", configCID, $"{configCID}.yaml");
        var config = GM.Msg<Dictionary<string, object>>("ReadYaml", $"{configCID}.yaml");
        configs.Add(configCID, config);
        
        // �qconfig���擾
        var objs = (List<string>)config["objs"];
        foreach (var objCID in objs)
        {
            if (configs.ContainsKey(objCID)) continue;
            
            await DownloadContentAsync(objCID);    // �ċA�Ăяo��
        }
    }
}
