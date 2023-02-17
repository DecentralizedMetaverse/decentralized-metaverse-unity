using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TC;

public class PlayerLocation : NetworkBehaviour
{
    Vector3 startPosition;
    Quaternion startRotation;

    void Start()
    {
        if (!isLocalPlayer) return;
        GM.Add("SpawnStart", StartPosition);
        GM.Add<Vector3>("TP", Teleport);
        GM.Add("Spawn", Spawn);

        if (GM.db.startWorldPosition)
        {
            GM.Msg("spawn.start");
        }
    }

    void Spawn()
    {
        Teleport(startPosition, startRotation);
    }

    void Teleport(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }
    
    void Teleport(Vector3 pos)
    {
        transform.position = pos;
    }


    void StartPosition()
    {
        var obj = GameObject.FindGameObjectsWithTag("PlayerStart");
        if (obj == null) { Debug.LogWarning("PlayerStart not found"); return; }

        if (obj.Length == 0) return;

        var tra = obj[0].transform;
        transform.position = tra.position;
        transform.rotation = tra.rotation;
        startPosition = tra.position;
        startRotation = tra.rotation;
    }

}
