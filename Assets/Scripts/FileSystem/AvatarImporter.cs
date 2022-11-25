using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;
using UniGLTF;
using System;
using System.IO;

public class AvatarImporter : MonoBehaviour, GM_Msg
{
    [SerializeField] DB_User dbUser;
    [SerializeField] Shader shader;
    [SerializeField] RuntimeAnimatorController controller;
    string avatarPath;
    void Awake()
    {
        GM.Add("avatar.load", this);// id��0�̏ꍇ�͎��g�����[�h���鎞
        avatarPath = $"{Application.dataPath}/../Avatar";
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if (data2[1].ToString() == "local")
        {
            GetAvatar((string)data2[0], GM.id, true);
        }
        else GetAvatar((string)data2[0], (uint)data2[1], false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="mode">0:���[�J��</param>
    public void GetAvatar(string avatarName, uint id, bool local)
    {
        var path = $"{avatarPath}/{avatarName}.vrm";
        if (!File.Exists(path)) return;

        var avatar = Load(path);
        var thumbnail = avatar.GetComponent<VRMMeta>().Meta.Thumbnail;
        var obj = avatar.gameObject;
        var anim = obj.GetComponent<Animator>();
        anim.runtimeAnimatorController = controller;
        var user = dbUser.GetData(id);
        user.thumbnail = Sprite.Create(thumbnail, new Rect(0,0,thumbnail.width,thumbnail.height), new Vector2(0.5f, 0.5f));
        user.obj = obj;

        if (!local)
        {
            GM.Msg($"avatar.object.{id}", obj);
            return;
        }

        var parentObject = GameObject.FindWithTag("Player");
        parentObject.GetComponent<PlayerControllerF>().animator = anim;
        GM.Msg("player.loaded", obj.transform);
        GM.Msg($"avatar.object.local", obj);
        GM.Msg("avatar.change", avatarName);
    }

    public RuntimeGltfInstance Load(string path, byte[] bytes = null)
    {
        GltfData data = null;
        try
        {
            if (bytes != null)
            {
                data = new GlbLowLevelParser(path, bytes).Parse();
            }
            else
            {
                data = new GlbFileParser(path).Parse();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"ParseError: {path}");
            Debug.LogException(ex);
            return null;
        }

        try
        {
            using (data)
            using (var importer = new VRMImporterContext(new VRMData(data)))
            {
                var avatar = importer.Load();
                avatar.ShowMeshes();
                return avatar;
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }
}
