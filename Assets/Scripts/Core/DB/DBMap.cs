using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBMap", menuName = "DB/DBMap")]
public class DBMap : ScriptableObject
{
    [HideInInspector] public List<DBMapElm> data=new List<DBMapElm>();
}
[System.Serializable]
public class DBMapElm
{
    public string sceneName;
    public string stageName;
    public bool active;
    public sbyte bgm = -1;
}