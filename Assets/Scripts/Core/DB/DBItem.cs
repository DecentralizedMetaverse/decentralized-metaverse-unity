using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBItem", menuName = "DB/DBItem")]
public class DBItem : ScriptableObject
{
    public type category;
    [HideInInspector] public List<DBItemElm> list = new List<DBItemElm>();
    public enum type
    {
        none, medicine, magic, _event, cooking, ore, crops, sea, animal, collecting, equipment
    }
    public enum season
    {
        spring, summer, fall, winter
    }
    public enum equip
    {
        none, weapon, 
    }
}
[System.Serializable]
public class DBItemElm
{
    public string itemName;
    public Sprite icon;
    public string objName;
    public string description;
    public DBItem.type type;
    public DBItem.season season;
    public DBItem.equip equip;
    public byte harvestTime;
    public int value;
    public int buy;
    public int sell;
    public bool isOpen;
    //追加効果
    public byte add1;
    public byte add2;
    public byte add3;
}
