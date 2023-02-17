using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TC;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ConsoleCommand : MonoBehaviour
{
    void Start()
    {
        GM.Add<string, string, string>("Exe", Exe);
    }

    public string Exe(string command, string args)
    {
        var psInfo = new ProcessStartInfo();
        psInfo.FileName = command;
        psInfo.Arguments = args;
        psInfo.UseShellExecute = false;
        psInfo.RedirectStandardOutput = true;
        var p = Process.Start(psInfo);
        return p.StandardOutput.ReadToEnd();
    }
}
