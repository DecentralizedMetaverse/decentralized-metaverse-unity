using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DB_Input", menuName = "DB/DB_Input")]
public class DB_Input : ScriptableObject
{
    public List<DB_InputE> data = new List<DB_InputE>();
}
[System.Serializable]
public class DB_InputE
{
    public string name;
    public eInput.type type;
    public string[] key;
}
