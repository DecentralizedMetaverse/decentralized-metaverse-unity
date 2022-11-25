using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_AvatarSettings : MonoBehaviour
{
    [SerializeField] InputField fieldName;
    [SerializeField] Toggle toggleHideNextTime;
    [SerializeField] Text textAvatar;
    [SerializeField] UI_AnimBase ui;

    [SerializeField] Transform groupAvatars;
    [SerializeField] Button prefabButton;

    const string fileName = "user.dat";
    string path;
    string pathAvatar;
    string pathAvatarList;

    void Start()
    {
        path = $"{Application.dataPath}/../{fileName}";
        pathAvatarList = $"{Application.dataPath}/../Avatar";
        if (!Directory.Exists(pathAvatarList)) Directory.CreateDirectory(pathAvatarList);
        ui.Finished += ReadAvatarFile;
        Read();
        StartCoroutine(StartAvatarSettings());
    }

    IEnumerator StartAvatarSettings()
    {
        while (GameObject.FindWithTag("Player") == null)
        {
            yield return null;
        }

        if (toggleHideNextTime.isOn)
        {
            //画面をスキップ
            OnSubmit();
            yield break;
        }

        //アバター設定画面を開く
        GM.pause = ePause.mode.GameStop;
        ui.active = true;
    }

    string[] files;
    public void ReadAvatarFile(bool b)
    {
        if (!b) return;
        foreach (Transform obj in groupAvatars)
        {
            Destroy(obj.gameObject);
        }

        files = Directory.GetFiles(pathAvatarList, "*.vrm");
        int count = 0;
        foreach (var file in files)
        {
            var button = Instantiate(prefabButton, groupAvatars);
            var worldName = Path.GetFileNameWithoutExtension(file);
            button.transform.GetChild(0).GetComponent<Text>().text = worldName;
            button.onClick.RemoveAllListeners();
            int i = count;
            button.onClick.AddListener(() => OnClickAvatar(i));
            count++;
        }
    }

    public void OnClickAvatar(int i)
    {
        pathAvatar = Path.GetFileNameWithoutExtension(files[i]);
        textAvatar.text = pathAvatar;
    }

    public void OnSubmit()
    {
        if (!File.Exists($"{pathAvatarList}/{pathAvatar}.vrm"))
        {
            //アバターがない場合
            GM.Msg("message", "Error: Avatar not found");
            return;
        }

        if (string.IsNullOrEmpty(fieldName.text))
        {
            //名前が記入されていない場合
            GM.Msg("message", "Error: Name is not written");
            return;
        }

        Write();
        ui.active = false;
        GM.pause = ePause.mode.none;
        GM.Msg("avatar.load", pathAvatar, "local");
        GM.Msg("player.name", fieldName.text);
    }

    void Read()
    {
        if (!File.Exists(path)) return;
        var text = File.ReadAllText(path);
        var data = text.Split('\n');

        fieldName.text = data.Length >= 1 ? data[0] : "";
        pathAvatar = data.Length >= 2 ? data[1] : "";
        textAvatar.text = pathAvatar;
        toggleHideNextTime.isOn = data.Length >= 3 ? bool.Parse(data[2]) : false;
    }

    void Write()
    {
        var text = $"{fieldName.text}\n{pathAvatar}\n{toggleHideNextTime.isOn}";
        File.WriteAllText(path, text);
    }
}
