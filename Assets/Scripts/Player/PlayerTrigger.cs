using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] NetworkIdentity identity;

    List<GameObject> obj2 = new List<GameObject>();
    List<ExeEvent> exeEvent = new List<ExeEvent>();

    private void Update()
    {
        if (GM.id != identity.netId) return;

        if (GM.pause == ePause.mode.GameStop) return;

        if (obj2.Count == 0) return;

        if (obj2[0] == null)
        {
            //イベント実行によりゲームオブジェクトが消えた場合
            obj2.RemoveAt(0);
            exeEvent.RemoveAt(0);

            if (obj2.Count == 0)
            {
                GM.Msg("Hint", false);
                return;
            }
            else
            {
                GM.Msg("Hint", true, exeEvent[0].GetHint());
            }
        }

        if (InputF.GetButtonDown(eInputMap.data.Submit2))
        {
            exeEvent[0].Exe(transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (GM.id != identity.netId) return;
        if (other.tag != "Event") return;

        var exe = other.GetComponent<ExeEvent>();
        obj2.Add(other.gameObject);
        exeEvent.Add(exe);

        GM.Msg("Hint", true, exeEvent[0].GetHint());
    }

    void OnTriggerExit(Collider other)
    {
        if (GM.id != identity.netId) return;
        if (other.tag != "Event") return;

        obj2.Remove(other.gameObject);
        exeEvent.Remove(other.GetComponent<ExeEvent>());

        if (obj2.Count == 0)
        {
            GM.Msg("Hint", false);
        }
        else
        {
            GM.Msg("Hint", true, exeEvent[0].GetHint());
        }
    }
}
