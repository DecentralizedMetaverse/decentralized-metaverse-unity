using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AvatarManager : NetworkBehaviour, GM_Msg
{
    GameObject player;

    void Start()
    {
        if (!isLocalPlayer)
        {
            GM.Add($"avatar.object.{netId}", this);
        }
        else
        {
            GM.id = netId;
            GM.Add($"avatar.object.local", this);
            transform.GetChild(2).tag = "Trigger";
        }
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if(player != null) Destroy(player);
        player = data2[0] as GameObject;
        player.transform.SetParent(transform);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;
        GetComponent<NetAnimation>().SetAnimator(player.GetComponent<Animator>());

    }
}
