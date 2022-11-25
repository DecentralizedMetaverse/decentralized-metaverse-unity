using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBBGM", menuName = "DB/DBBGM")]
public class DBBGM : ScriptableObject
{
    [HideInInspector] public List<DBBGMElm> data = new List<DBBGMElm>();
    public void OnEnable()
    {
        UnLoadAll();
        Debug.Log(this + "UnLoad");
    }
    public void Load(byte i)
    {
        if (!data[i].load)
        {
            data[i].clip = Resources.Load("BGM/" + data[i].fileName, typeof(AudioClip)) as AudioClip;
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
public class DBBGMElm
{
    public string fileName;
    public string bgmName;
    public bool noLoop;
    public float loopEndTime;
    public float loopBeginTime;
    public bool load;
    public AudioClip clip;
}

