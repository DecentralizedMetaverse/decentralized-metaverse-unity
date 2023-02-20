using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TC;
using UnityEngine;

/// <summary>
/// ワールド空間にオブジェクトを生成する
/// </summary>
public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] DB_Player db;
    [SerializeField] ObjectText prefabText;
    [SerializeField] VideoObject prefabVideo;

    Dictionary<string, Delegate> functions = new(64);


    void Start()
    {
        GM.Add<string>("GenerateObj", Generate);
        AddFunc<string>(".txt", ObjText);
        AddFunc<string>(".mp4", ObjVideo);
    }

    /// <summary>
    /// ワールド空間にオブジェクトを生成する
    /// </summary>
    /// <param name="fileName"></param>
    void Generate(string fileName)
    {
        var extension = System.IO.Path.GetExtension(fileName);
        if (!functions.ContainsKey(extension))
        {
            GM.Msg("ShortMessage","対応していないファイルです");
            return;
        }

        functions[extension].DynamicInvoke(fileName);
        // GM.Msg("", "対応していないファイルです");
    }

    /// <summary>
    /// 関数を登録する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="func"></param>
    void AddFunc<T>(string key, Action<T> func)
    {
        functions.Add(key, func);
    }
    private Vector3 GetPosition()
    {
        return db.transform.position + db.transform.forward;
    }

    // ------------------------------------------------
    void ObjText(string path)
    {
        var text = File.ReadAllText(path, System.Text.Encoding.UTF8);
        var pos = GetPosition();
        var obj = Instantiate(prefabText, pos, db.transform.rotation);
        obj.SetData(path, text);
    }

    void ObjVideo(string path)
    {
        var pos = GetPosition();
        var obj = Instantiate(prefabVideo, pos, db.transform.rotation);
        obj.LoadVideo(path);
    }
}
