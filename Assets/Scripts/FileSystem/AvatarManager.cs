using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TC;

public class AvatarManager : NetworkBehaviour
{
    GameObject player;

    void Start()
    {
        GM.Add<string>("LoadAvatar", LoadAvatar);
    }   

    void LoadAvatar(string id)
    {

    }
}
