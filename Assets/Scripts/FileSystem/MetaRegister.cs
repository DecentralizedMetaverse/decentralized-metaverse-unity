using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TC;
using UnityEngine;

/// <summary>
/// �z�u���ꂽ�I�u�W�F�N�g��Meta�t�@�C�����쐬����
/// </summary>
public class MetaRegister : MonoBehaviour
{
    [SerializeField] public Transform root;

    /// <summary>
    /// Meta�t�@�C�����쐬����
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    async UniTask<string> UpdateMeta(Transform transform)
    {
        List<string> objs = new();

        // �q�K�w�ɃA�N�Z�X
        foreach (Transform child in transform)
        {
            var childMetaCID = await UpdateMeta(child);
            objs.Add(childMetaCID);
        }

        // Meta�t�@�C���̍쐬
        var fileName = transform.GetComponent<ObjectBase>().fileName;
        var metaCID = await CreateYamlData(fileName, transform, objs);
        
        return metaCID;
    }

    /// <summary>
    /// Yaml�ɏ����o�����e���쐬����
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

        // �t�@�C���̏����o��
        var metaCID = await WriteMeta(data);

        return metaCID;
    }


    /// <summary>
    /// Meta�t�@�C����������
    /// </summary>
    /// <param name="yamlData"></param>
    /// <returns>�ۑ������t�@�C���p�X</returns>
    async UniTask<string> WriteMeta(Dictionary<string, object> yamlData)
    {
        // ���̃t�@�C�����쐬
        var path = $"{Application.dataPath}/{GM.db.metaPath}/temp.yaml";
        GM.Msg("WriteYaml", path, yamlData);

        // IPFS�֓o�^
        var cid = await GM.Msg<UniTask<string>>("UploadContent", path);

        // �����o��
        var outputPath = $"{Application.dataPath}/{GM.db.metaPath}/{cid}.yaml";
        GM.Msg("WriteYaml", outputPath, yamlData);

        return outputPath;
    }
}
