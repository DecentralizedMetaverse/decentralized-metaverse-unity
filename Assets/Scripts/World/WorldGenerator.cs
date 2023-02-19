using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TC;
using UnityEngine;

/// <summary>
/// Worldを構築する
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
    /// ワールド生成
    /// </summary>
    /// <param name="loadX"></param>
    /// <param name="loadY"></param>
    /// <returns></returns>
    async UniTask<GameObject> GenerateWorld(int loadX, int loadY)
    {
        // IPFSからコンテンツをダウンロードする
        // world tableが必要
        await DownloadWorldConfig(loadX, loadY);
        return null;
    }

    /// <summary>
    /// ワールド情報の読み込み
    /// </summary>
    /// <param name="loadX"></param>
    /// <param name="loadY"></param>
    async UniTask DownloadWorldConfig(int loadX, int loadY)
    {
        var rootConfigCID = rootCids[(loadX, loadY)];

        await DownloadContentAsync(rootConfigCID);
    }

    /// <summary>
    /// Configファイルダウンロード
    /// </summary>
    /// <param name="configCID"></param>
    /// <returns></returns>
    async UniTask DownloadContentAsync(string configCID)
    {
        if(configs.ContainsKey(configCID)) { return; }

        // Configファイルダウンロード
        await GM.MsgAsync("DownloadContent", configCID, $"{configCID}.yaml");
        var config = GM.Msg<Dictionary<string, object>>("ReadYaml", $"{configCID}.yaml");
        configs.Add(configCID, config);
        
        // 子configを取得
        var objs = (List<string>)config["objs"];
        foreach (var objCID in objs)
        {
            if (configs.ContainsKey(objCID)) continue;
            
            await DownloadContentAsync(objCID);    // 再帰呼び出し
        }
    }
}
