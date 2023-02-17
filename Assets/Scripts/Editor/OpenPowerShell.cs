using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class OpenPowerShell : MonoBehaviour
{
    [MenuItem("Tools/Open PowerShell")]
    static void Execute()
    {        
        Process.Start("powershell");
    }
}
