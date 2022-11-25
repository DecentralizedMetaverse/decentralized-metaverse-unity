using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAvatarDownloader : NetworkBehaviour, GM_Msg
{
    [SerializeField] GameObject loadingScreen;
    [SyncVar] string avatarName;
    string path;
    NetworkIdentity identity;

    void Awake()
    {
        identity = GetComponent<NetworkIdentity>();        
    }

    void Start()
    {
        path = Application.dataPath + "/../Avatar/";
        loadingScreen.SetActive(false);

        if (!isLocalPlayer)
        {
            if (avatarName == "") return;
            GM.Msg("avatar.load", avatarName, identity.netId); //アバター読み込み
            return;
        }
        GM.Add("avatar.change", this);
        GM.Add("avatar.download", this);
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if(data1 == "avatar.change")
        {
            ChangeAvatar((string)data2[0]);
        }
        else if(data1 == "avatar.download")
        {
            RequestAvatar(identity.connectionToClient);
        }
    }

    [Command]
    public void ChangeAvatar(string name)
    {
        avatarName = name;
        ChangeAvatarClient(name);
    }

    [ClientRpc]
    public void ChangeAvatarClient(string name)
    {
        if (isLocalPlayer) return;
        avatarName = name;
        GM.Msg("avatar.load", avatarName, identity.netId); //アバター読み込み
    }

    /// <summary>
    /// target 
    /// </summary>
    /// <param name="target">送信元</param>
    [TargetRpc]
    public void RequestAvatar(NetworkConnection target)
    {
        var data = fg.ConvertToByte(path + avatarName);
        ReceiveAvatar(target, data);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target">送信先</param>
    /// <param name="data"></param>
    [TargetRpc]
    public void ReceiveAvatar(NetworkConnection target, byte[] data)
    {
        fg.ConvertToFile(path+avatarName, data);
        GM.Msg("avatar.load.name", name); //アバター読み込み
    }
}
