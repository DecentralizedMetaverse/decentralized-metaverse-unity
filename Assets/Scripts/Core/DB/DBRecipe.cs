using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBRecipe", menuName = "DB/DBRecipe")]
public class DBRecipe : ScriptableObject
{
    [HideInInspector] public List<DBRecipeElm> data = new List<DBRecipeElm>();
    public DBRecipeGroup.category category;
    private void OnEnable()
    {
        for(byte i = 0; i < data.Count; i++)
        {
            data[i].idCheck = false;
        }
    }
}
[System.Serializable]
public class DBRecipeElm
{
    public string recipeName;
    public byte id1;
    public byte id2;
    public byte id3;
    public byte id4;
    public byte id5;
    public byte cid1;
    public byte cid2;
    public byte cid3;
    public byte cid4;
    public byte cid5;
    public bool learned;
    public bool isOpen;
    public bool idCheck;
    public byte recipeCategoryId;
    public byte recipeId;
}