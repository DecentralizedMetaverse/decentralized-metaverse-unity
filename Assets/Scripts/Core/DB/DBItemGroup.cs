using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DBItemGroup", menuName = "DB/DBItemGroup")]

public class DBItemGroup : ScriptableObject
{
    [HideInInspector] public List<DBItemGroupElm> data = new List<DBItemGroupElm>();
}
[System.Serializable]
public class DBItemGroupElm
{
    public DBItem dbItem;
}
