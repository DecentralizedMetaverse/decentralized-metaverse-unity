using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(YScriptExe))]
public class CommandObject : MonoBehaviour, ExeEvent, GM_Msg
{
    public string data;
    static int masterID = 0;
    int id = 0;
    string path = "";
    YScriptExe exe;
    bool run = false;
    bool triggerEnter;

    void Awake()
    {
        id = masterID;
        masterID++;
        path = Application.persistentDataPath + $"/cmd_{id}.dat";
        Read();
        exe = GetComponent<YScriptExe>();
        exe.ignoreError = true;
    }

    private void OnDestroy()
    {
        Write();
    }

    void Read()
    {
        if (!File.Exists(path)) return;
        var reader = new StreamReader(path);
        data = reader.ReadToEnd();
        reader.Close();
    }

    void Write()
    {
        var writer = new StreamWriter(path);
        writer.Write(data);
        writer.Flush();
        writer.Close();
    }

    void Start()
    {
        GM.Add("commandObject", this);
    }

    string ExeEvent.GetHint()
    {
        return "";
    }

    void ExeEvent.Exe(Vector3 vec)
    {
        if (run) return;

        exe.Exe(data);
    }

    //void GM_Msg.Receive(string data1, params object[] data2)
    void GM_Msg.Receive(string data1, params object[] data2)
    {
        data = data2[0].ToString();
        run = false;
        GM.Remove("commandObject", this);
    }

    void Update()
    {
        if (triggerEnter && GM.pause != ePause.mode.GameStop && InputF.GetButtonDown(eInputMap.data.Command))
        {
            if (run) return;

            run = true;
            GM.Add("commandObj", this);
            GM.Msg("commandUI", data);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trigger")
        {
            triggerEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Trigger")
        {
            triggerEnter = false;
        }
    }
}
