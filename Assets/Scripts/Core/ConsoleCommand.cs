using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TC;
using UnityEngine;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public class ConsoleCommand : MonoBehaviour
{
    void Start()
    {
        GM.Add<string, string, UniTask<string>>("Exe", Exe);
    }

    async UniTask<string> Exe(string command, string args)
    {
        var psInfo = new ProcessStartInfo();
        psInfo.FileName = command;
        psInfo.Arguments = args;
        psInfo.UseShellExecute = false;
        psInfo.RedirectStandardOutput = true;
        var process = Process.Start(psInfo);

        await UniTask.SwitchToThreadPool();
        process.WaitForExit();
        await UniTask.SwitchToMainThread();

        var result = await process.StandardOutput.ReadToEndAsync().AsUniTask();
        return result;
    }
}
