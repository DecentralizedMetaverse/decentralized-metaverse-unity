using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(YScriptExe))] 
public class EventObject : MonoBehaviour, ExeEvent
{
    public string scriptContent;
    public string hintText;
    YScriptExe script;
    public eYScript.exeType type = eYScript.exeType.決定キーで実行;

    void Start()
    {
        script = GetComponent<YScriptExe>();

        if (type == eYScript.exeType.決定キーで実行)
        {
            script.Finished += Finished;
        }
    }

    string ExeEvent.GetHint()
    {
        return hintText;
    }
    
    void ExeEvent.Exe(Vector3 vec)
    {
        if (type != eYScript.exeType.決定キーで実行) return;
        GM.Msg("Hint", false);
        Exe();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (type != eYScript.exeType.接触実行) return;

        if(other.tag == "Trigger")
        {
            Exe();
        }
    }

    public void Exe()
    {
        if (script.data == null)
        {
            script.Exe(scriptContent);
        }
        else script.Exe();
    }

    void Finished()
    {
        GM.Msg("Hint", true, hintText);
    }
}
