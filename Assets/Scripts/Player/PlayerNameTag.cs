using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TC;

public class PlayerNameTag : NetworkBehaviour
{
    [SerializeField] DB_User dbUser;
    [SerializeField] Text textName;
    [SyncVar] string playerName;

    void Start()
    {
        textName.text = playerName;
        if (!isLocalPlayer) return;
        GM.Add<string>("ChangeName", ChangeName);
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
