using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TC;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EventObject : MonoBehaviour, ExeEvent
{
    [SerializeField] public string hintText;
    public eYScript.exeType type = eYScript.exeType.����L�[�Ŏ��s;
    
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
    /// ����L�[���s
    /// </summary>
    /// <param name="vec"></param>
    async void ExeEvent.SubmitRun(Vector3 vec)
    {
        if (type != eYScript.exeType.����L�[�Ŏ��s) return;
        GM.Msg("Hint","", false);
        await GM.MsgAsync("Run", luaScript.text);
        GM.Msg("Hint", hintText, true);
    }

    /// <summary>
    /// �ڐG���s
    /// </summary>
    void ExeEvent.EnterRun()
    {
        if (type != eYScript.exeType.�ڐG���s) return;
        GM.MsgAsync("Run", luaScript.text).Forget();
    }
}
