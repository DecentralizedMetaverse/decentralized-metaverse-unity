using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBBg", menuName = "DB/DBBg")]
public class DBBg : ScriptableObject
{
    [HideInInspector] public List<DBBgElm> data = new List<DBBgElm>();
    public void OnEnable()
    {
        UnLoadAll();
        Debug.Log(this + "UnLoad");
    }
    public void Load(byte i)
    {
        if (data[i].load) return;
        data[i].sprite = Resources.Load("2D/Bg/" + data[i].fileName, typeof(Sprite)) as Sprite;
        data[i].load = true;
    }
    public void UnLoad(byte i)
    {
        if (!data[i].load) return;
        Resources.UnloadAsset(data[i].sprite);
        data[i].load = false;
    }
    public void UnLoadAll()
    {
        for(byte i = 0; i < data.Count; i++)
        {
            UnLoad(i);
        }
    }
}
[System.Serializable]
public class DBBgElm
{
    public string fileName;
    public Color32 color;
    public Sprite sprite;
    public bool load;
}
