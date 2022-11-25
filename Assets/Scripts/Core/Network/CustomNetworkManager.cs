using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class CustomNetworkManager : NetworkManager
{
    [SerializeField] DB_User dbUser;
    new void Awake()
    {
        base.Awake();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        //var obj = Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity);
        //var id = conn.identity.netId;
        //DB_UserE data = new DB_UserE();
        //data.id = id;
        //data.obj = obj;
        //dbUser.data.Add(data);
        //dbUser.index.Add(data.id, dbUser.data.Count - 1);
        base.OnServerAddPlayer(conn);
    }
}
