using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBCharacter", menuName = "DB/DBCharacter")]
public class DBCharacter : ScriptableObject
{
    [HideInInspector] public List<DBCharacterElm> data = new List<DBCharacterElm>();
    private void OnEnable()
    {
        UnLoad();
        Load(0);
    }
    public void Load(byte n1, byte n2, byte n3, byte n4)
    {
        Load(n1);
        Load(n2);
        Load(n3);
        Load(n4);
    }
    public void Load(byte id)
    {
        if (data[id].load) return;
        string path = "2D/Stn/ch" + id + "/ch" + id + "_";
        for (byte i = 0; i < data[id].stnMax; i++)
        {
            data[id].sprite[i] = Resources.Load(path + i, typeof(Sprite)) as Sprite;
        }
        data[id].load = true;
    }
    public void UnLoad()
    {
        for (byte i = 0; i < data.Count; i++)
        {
            if (!data[i].load) continue;
            for (byte j = 0; j < data[i].sprite.Length; j++)
            {
                Resources.UnloadAsset(data[i].sprite[i]);
            }
            data[i].load = false;
        }
    }
}
[System.Serializable]
public class DBCharacterElm
{
    public string name;
    public byte stnMax;
    public Sprite[] sprite = new Sprite[20];
    public bool load;
    public bool open;
}
