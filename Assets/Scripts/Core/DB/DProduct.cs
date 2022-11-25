using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DProduct", menuName = "Data/DProduct")]
public class DProduct : ScriptableObject
{
    public DProductElm[] data;
}
[System.Serializable]
public class DProductElm:DItemElm
{
    public int buy;
}