using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ev", menuName = "DBScript")]
public class DBScript : ScriptableObject
{
    [HideInInspector] public List<DBScriptElm> exe2 = new List<DBScriptElm>();
    public DBFlag.exe exe;
    [SerializeField, TextArea(1, 100)]
    public string data;
}
[System.Serializable]
public class DBScriptElm
{
    public DBFlag.type type;
    public byte num;
    public DBFlag.exe2 exe;
}