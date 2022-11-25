using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerLocation : NetworkBehaviour, GM_Msg
{
    Vector3 startPosition;
    Quaternion startRotation;
    
    void Start()
    {
        if (!isLocalPlayer) return;
        GM.Add("spawn.start", this);
        GM.Add("tp", this);
        GM.Add("spawn", this);

        if (SM.dbScene.startWorldPosition)
        {
            GM.Msg("spawn.start");
        }        
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if (data1 == "spawn.start") StartPosition(data2);
        else if (data1 == "spawn") Teleport(startPosition, startRotation);
        else if (data1 == "tp")
        {
            if (data2.Length == 2) Teleport((Vector3)data2[0], (Quaternion)data2[1]);
            else Teleport((Vector3)data2[0], transform.rotation);
        }
    }

    void StartPosition(params object[] data2)
    {
        int id = data2[0].ToString() == "" ? 0 : int.Parse(data2[0].ToString());

        var obj = GameObject.FindGameObjectsWithTag("PlayerStart");
        if (obj == null) { Debug.LogWarning("PlayerStart not found"); return; }

        if (obj.Length == 0) return;
        if (id >= obj.Length) id = 0;

        var tra = obj[id].transform;
        transform.position = tra.position;
        transform.rotation = tra.rotation;
        startPosition = tra.position;
        startRotation = tra.rotation;
    }

    void Teleport(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }
}
