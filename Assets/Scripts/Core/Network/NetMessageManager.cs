using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using TC;

public class NetMessageManager : NetworkBehaviour
{
    void Awake()
    {
        GM.Add<string[]>("send", SendServerMessage);
    }    

    [Command(requiresAuthority = false)]
    void SendServerMessage(string[] messages)
    {
        var message = string.Join("#", messages);
        SendClientMessage(message);
    }

    [ClientRpc]
    void SendClientMessage(string data)
    {
        var msg = data.Split('#');
        GM.Msg(msg[0], msg.Skip(1).ToArray());
    }
}
