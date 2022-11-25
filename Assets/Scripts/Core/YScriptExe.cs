using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
/// <summary>
/// スクリプトデータを管理部に送信する
/// </summary>
public class YScriptExe : MonoBehaviour, GM_Msg
{
    public DB_YScript data;
    public bool ignoreError = false;
    [System.NonSerialized] public bool isOpen; //Editor拡張用
    [NonSerialized] public bool run = false;

    public delegate void Function();
    public Function Finished;

    private void Start()
    {
        if (data == null)
        {
            if (ignoreError) return;
            Debug.LogWarning($"{gameObject.name}  にデータが割り当てられていません"); 
        }
        else if (data.exe == eYScript.exe.first) Exe();
    }
    /// <summary>
    /// スクリプト実行
    /// </summary>
    public void Exe()
    {
        Exe(data.yscript);
    }
    
    public void Stop()
    {
        GM.Msg("CompleteScript", GetInstanceID());
    }

    public void Exe(string data)
    {
        if (run) return;
        run = true;
        GM.Add("CompleteScript", this);
        GM.Msg("Script", data);
        GM.Msg("ScriptID", GetInstanceID());
    }
    
    private void Update()
    {
        if (data != null && data.exe == eYScript.exe.always) Exe();
    }
    
    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if (Convert.ToInt32(data2[0]) == GetInstanceID())
        {
            run = false;
            GM.Remove("CompleteScript", this);
            Finished?.Invoke();
        }
    }
}
