using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBFootSe", menuName = "DB/DBFootSe")]
public class DBFootSe : ScriptableObject
{
    [HideInInspector] public List<DBFootSeElm> data = new List<DBFootSeElm>();
    public void OnEnable()
    {
        UnLoadAll();
        Debug.Log(this + "UnLoad");
    }
    public void Load(byte i)
    {
        if (!data[i].load)
        {
            data[i].clip = Resources.Load("FootSE/" + data[i].fileName, typeof(AudioClip)) as AudioClip;
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
        for (byte i = 0; i < data.Count; i++)
        {
            UnLoad(i);
        }
    }
}
[System.Serializable]
public class DBFootSeElm
{
    public string fileName;
    public string seName;
    public bool load;
    public AudioClip clip;
}
