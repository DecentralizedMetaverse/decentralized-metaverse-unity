using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_WorldSelect : MonoBehaviour
{
    [SerializeField] Button prefabButton;
    [SerializeField] Transform group;
    [SerializeField] UI_AnimBase ui;
    string[] dirs;
    string path;

    private void Awake()
    {
        path = Application.dataPath + "/../World";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        ui.Finished += Show;
    }

    public void Show(bool active)
    {
        if (!active) return;

        foreach(Transform obj in group)
        {
            Destroy(obj.gameObject);
        }

        dirs = Directory.GetDirectories(path);
        int count = 0;
        foreach(var dir in dirs)
        {
            var button = Instantiate(prefabButton, group);
            var worldName = Path.GetFileName(dir);
            button.transform.GetChild(0).GetComponent<Text>().text = worldName;
            button.onClick.RemoveAllListeners();
            int i = count;
            button.onClick.AddListener(() => OnClick(i));
            count++;
        }
    }

    void OnClick(int i)
    {
        GM.Msg("world.load", dirs[i], false);
        ui.active = false;
    }
}
