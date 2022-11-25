using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBFunction", menuName = "DB/DBFunction")]
public class DBFunction : ScriptableObject
{
    public List<DBFunctionElm> data = new List<DBFunctionElm>();
}
[System.Serializable]
public class DBFunctionElm
{
    public string name;
    [TextArea(2, 100)]
    public string data;
    public bool isOpen;
}
