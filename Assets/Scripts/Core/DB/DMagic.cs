using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DMagic", menuName = "Data/DMagic")]
public class DMagic : ScriptableObject
{
    public DMagicElm[] data;
    public byte selectMagic;
}
[System.Serializable]
public class DMagicElm
{
    public byte id;
    public byte quality;
    public DBMagic.effect effect1;
    public DBMagic.effect effect2;
}

