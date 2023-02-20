using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TC;
using UnityEngine;

/// <summary>
/// 配置されたオブジェクトのMetaファイルを作成する
/// </summary>
public class MetaRegister : MonoBehaviour
{
    [SerializeField] public Transform root;

    /// <summary>
    /// Metaファイルを作成する
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    async UniTask<string> UpdateMeta(Transform transform)
    {
        List<string> objs = new();

        // 子階層にアクセス
        foreach (Transform child in transform)
        {
            var childMetaCID = await UpdateMeta(child);
            objs.Add(childMetaCID);
        }

        // Metaファイルの作成
        var fileName = transform.GetComponent<ObjectBase>().fileName;
        var metaCID = await CreateYamlData(fileName, transform, objs);
        
        return metaCID;
    }

    /// <summary>
    /// Yamlに書き出す内容を作成する
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    async UniTask<string> CreateYamlData(string fileName, Transform transform, List<string> objs)
    {
        var cid = await GM.Msg<UniTask<string>>("UploadContent", fileName);

        Dictionary<string, object> data = new();
        data.Add("name", transform.gameObject.name);
        data.Add("cid", cid);

        Dictionary<string, string> location = new();
        location.Add("position", transform.position.ToSplitString());
        location.Add("rotation", transform.rotation.eulerAngles.ToSplitString());
        location.Add("scale", transform.localScale.ToSplitString());
        data.Add("transform", location);
        
        data.Add("objs", objs);

        // ファイルの書き出し
        var metaCID = await WriteMeta(data);

        return metaCID;
    }


    /// <summary>
    /// Metaファイル書き込み
    /// </summary>
    /// <param name="yamlData"></param>
    /// <returns>保存したファイルパス</returns>
    async UniTask<string> WriteMeta(Dictionary<string, object> yamlData)
    {
        // 仮のファイルを作成
        var path = $"{Application.dataPath}/{GM.db.metaPath}/temp.yaml";
        GM.Msg("WriteYaml", path, yamlData);

        // IPFSへ登録
        var cid = await GM.Msg<UniTask<string>>("UploadContent", path);

        // 書き出し
        var outputPath = $"{Application.dataPath}/{GM.db.metaPath}/{cid}.yaml";
        GM.Msg("WriteYaml", outputPath, yamlData);

        return outputPath;
    }
}
