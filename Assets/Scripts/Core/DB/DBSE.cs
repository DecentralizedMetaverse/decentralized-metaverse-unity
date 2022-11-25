using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBSE", menuName = "DB/DBSE")]
public class DBSE : ScriptableObject
{
    [HideInInspector] public List<DBSEElm> data = new List<DBSEElm>();
    public void OnEnable()
    {
        UnLoadAll();
    }
    public void Load(byte i)
    {
        if (!data[i].load)
        {
            data[i].clip = Resources.Load("SE/" + data[i].fileName, typeof(AudioClip)) as AudioClip;
            data[i].load = true;
        }
    }
    public void UnLoad(byte i)
    {
        if (!data[i].load) return;
        Resources.UnloadAsset(data[i].clip);
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
public class DBSEElm
{
    public string fileName;
    public string seName;
    public bool load;
    public AudioClip clip;
}
