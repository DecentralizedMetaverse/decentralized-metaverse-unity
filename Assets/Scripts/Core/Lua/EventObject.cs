using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TC;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EventObject : MonoBehaviour, ExeEvent
{
    [SerializeField] public string hintText;
    public eYScript.exeType type = eYScript.exeType.決定キーで実行;
    
    [SerializeField]
    public TextAsset luaScript;

    void Start()
    {
        transform.tag = "Event";                
    }

    string ExeEvent.GetHint()
    {
        return hintText;
    }

    /// <summary>
    /// 決定キー実行
    /// </summary>
    /// <param name="vec"></param>
    async void ExeEvent.SubmitRun(Vector3 vec)
    {
        if (type != eYScript.exeType.決定キーで実行) return;
        GM.Msg("Hint","", false);
        await GM.MsgAsync("Run", luaScript.text);
        GM.Msg("Hint", hintText, true);
    }

    /// <summary>
    /// 接触実行
    /// </summary>
    void ExeEvent.EnterRun()
    {
        if (type != eYScript.exeType.接触実行) return;
        GM.MsgAsync("Run", luaScript.text).Forget();
    }
}
