using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DItem", menuName = "Data/DItem")]
[System.Serializable]
public class DItem : ScriptableObject
{
    public DItemElm[] data;
}
[System.Serializable]
public class DItemElm
{
    public byte categoryId;
    public byte id;
    public byte num;
    public byte quality;
    public bool equip;
    public DItemElm() { }
    public DItemElm(byte ctid, byte id, byte num = 1)
    {
        categoryId = ctid;
        this.id = id;
        this.num = num;
    }
}
