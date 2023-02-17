using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TC;

public class PlayerAvatarDownloader : NetworkBehaviour
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
            GM.Msg("avatar.load", avatarName, identity.netId); //�A�o�^�[�ǂݍ���
            return;
        }
        GM.Add<string>("ChangeAvatar", ChangeAvatar);
        GM.Add("DownloadAvatar", DownloadAvatar);
    }    

    void DownloadAvatar()
    {
        RequestAvatar(identity.connectionToClient);
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
        GM.Msg("avatar.load", avatarName, identity.netId); //�A�o�^�[�ǂݍ���
    }

    /// <summary>
    /// target 
    /// </summary>
    /// <param name="target">���M��</param>
    [TargetRpc]
    public void RequestAvatar(NetworkConnection target)
    {
        var data = fg.ConvertToByte(path + avatarName);
        ReceiveAvatar(target, data);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target">���M��</param>
    /// <param name="data"></param>
    [TargetRpc]
    public void ReceiveAvatar(NetworkConnection target, byte[] data)
    {
        fg.ConvertToFile(path+avatarName, data);
        GM.Msg("avatar.load.name", name); //�A�o�^�[�ǂݍ���
    }
}
