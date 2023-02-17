using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveData : MonoBehaviour, GM_Msg
{
    [SerializeField] DB_Player db;
    //[SerializeField] DItem db_bag;
    [SerializeField] DB_Settings dbSettings;
    string path = "";

    void Awake()
    {
        //Read(0);
        //Read(1);
        Read(2);
    }

    void Start()
    {
        path = Application.persistentDataPath + "/save.dat";
        //GM.Add("save", this);
        //GM.Msg("player.mng");
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if (data2[0].ToString() == "read") Read(2);
        else if (data2[0].ToString() == "write") Write(2);
    }

    void OnDisable()
    {
        //Write(0);
        //Write(1);
        Write(2);
    }

   

    public void Read(int i)
    {
        if (!File.Exists(path))
            return;

        var reader = new StreamReader(path);
        var data = reader.ReadToEnd();
        reader.Close();
        ReadData(i, data);
    }

    public void Write(int i)
    {
        var data = WriteData(i);
        var writer = new StreamWriter(path);
        writer.Write(data);
        writer.Flush();
        writer.Close();
    }

    void ReadData(int id, string data)
    {
        if (id == 0) db = JsonUtility.FromJson<DB_Player>(data);
        //else if (id == 1) db_bag = JsonUtility.FromJson<DItem>(data);
        else if (id == 2)
        {
            dbSettings = JsonUtility.FromJson<DB_Settings>(data);
            dbSettings.screenRes = dbSettings.screenRes;
            dbSettings.fullScreen = dbSettings.fullScreen;
        }
    }
    
    string WriteData(int id)
    {
        if (id == 0) return JsonUtility.ToJson(db);
       // else if (id == 1) return JsonUtility.ToJson(db_bag);
        else if (id == 2) return JsonUtility.ToJson(dbSettings);

        return "";
    }
}
