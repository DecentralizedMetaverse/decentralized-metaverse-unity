using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class NetMessageManager : NetworkBehaviour, GM_Msg
{
    void Awake()
    {        
        GM.Add("send", this);
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        var data = string.Join("#", data2);
        SendServerMessage(data);
    }

    [Command(requiresAuthority = false)]
    void SendServerMessage(string data)
    {
        SendClientMessage(data);
    }

    [ClientRpc]
    void SendClientMessage(string data)
    {
        var msg = data.Split('#');
        GM.Msg(msg[0], msg.Skip(1).ToArray());
    }
}
