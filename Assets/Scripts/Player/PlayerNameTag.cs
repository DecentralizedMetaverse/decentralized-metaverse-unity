using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerNameTag : NetworkBehaviour, GM_Msg
{
    [SerializeField] DB_User dbUser;
    [SerializeField] Text textName;
    [SyncVar] string playerName;

    void Start()
    {
        textName.text = playerName;
        if (!isLocalPlayer) return;
        GM.Add("player.name", this);
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        ChangeName((string)data2[0]);
    }

    [Command]
    public void ChangeName(string name)
    {
        textName.text = name;
        playerName = name;
        dbUser.GetData(netIdentity.netId).name = name;
        ChangeNameClient(name);
    }

    [ClientRpc]
    public void ChangeNameClient(string name)
    {
        dbUser.GetData(netIdentity.netId).name = name;
        textName.text = name;
        playerName = name;
    }    
}
