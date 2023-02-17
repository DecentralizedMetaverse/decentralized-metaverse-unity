using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;
using Cysharp.Threading.Tasks;
using TC;
using System.IO;

/// <summary>
/// Luaを管理するクラス
/// </summary>
public class LuaManager : MonoBehaviour
{
    public static LuaEnv lua { get; private set; }
    public static LuaEnv luaInternal { get; private set; }

    int numFunc = 0;
    HashSet<int> waitFuncList = new(30);

    void Awake()
    {
        lua = new LuaEnv();
        luaInternal = new LuaEnv();
        RegisterGetFunction();
    }

    private void OnDestroy()
    {
        lua.Dispose();
        luaInternal.Dispose();
    }

    /// <summary>
    /// C#の関数を登録する際に使用する関数を登録する
    /// </summary>
    private void RegisterGetFunction()
    {
        lua.DoString($"Log = CS.UnityEngine.Debug.Log");
        luaInternal.DoString($"Log = CS.UnityEngine.Debug.Log");

        lua.DoString($"GetFunction = CS.TC.GM.GetFunction");
        luaInternal.DoString($"GetFunction = CS.TC.GM.GetFunction");

        lua.AddLoader(CustomLoader);
        luaInternal.AddLoader(CustomLoader);
    }

    /// <summary>
    /// Luaファイルを読み込めるようにする
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    private byte[] CustomLoader(ref string filepath)
    {
        if (File.Exists(filepath))
        {
            return File.ReadAllBytes(filepath);
        }

        return null;    // ローダ読み込み失敗
    }

    /// <summary>
    /// 関数が終了しているかどうか
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    public bool IsFinishFunc(int id)
    {
        return waitFuncList.Contains(id) ? false : true;
    }

    /// <summary>
    /// 非同期関数の生存を確認するメソッド
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public int WaitFunc(string keyword, params object[] args)
    {
        var id = numFunc++;
        waitFuncList.Add(id);
        _WaitFunc(id, keyword, args).Forget();
        GM.Log($"wait id: {id}");

        return id;
    }

    /// <summary>
    /// 関数が終了したら、idをリストから削除するメソッド
    /// </summary>
    /// <param name="id"></param>
    /// <param name="keyword"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public async UniTask _WaitFunc(int id, string keyword, params object[] args)
    {
        await GM.Msg<UniTask>(keyword, args);
        waitFuncList.Remove(id);
    }

    /// <summary>
    /// LuaにFunctionを登録する
    /// </summary>
    /// <param name="key"></param>
    /// <param name="function"></param>
    public static void RegisterLuaFunction(LuaEnv lua, string key, Delegate function)
    {
        lua.DoString($"{key} = GetFunction(\"{key}\");");
    }
}