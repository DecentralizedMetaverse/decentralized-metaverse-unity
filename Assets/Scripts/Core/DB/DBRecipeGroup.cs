using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBRecipeGroup", menuName = "DB/DBRecipeGroup")]
public class DBRecipeGroup : ScriptableObject
{
    [HideInInspector] public List<DBRecipeGroupElm> data = new List<DBRecipeGroupElm>();
    public enum category
    {
        none, stapleFood, mainDish, sideDish, dessert, other, magic
    }
    /// <summary>
    /// recipeDBの完成アイテムと同じctidとidをアイテムdbから探し出す
    /// </summary>
    /// <param name="db"></param>
    /// <param name="recipeElm"></param>
    public void Find(DBItemGroup db, DBRecipeElm recipeElm)
    {
        if (recipeElm.idCheck) return;
        for (byte i = 1; i < db.data.Count; i++)
        {
            for (byte j = 0; j < db.data[i].dbItem.list.Count; j++)
            {
                if (db.data[i].dbItem.list[j].itemName == recipeElm.recipeName)
                {
                    recipeElm.idCheck = true;
                    recipeElm.recipeCategoryId = i;
                    recipeElm.recipeId = j;
                    return;
                }
            }
        }
    }
}
[System.Serializable]
public class DBRecipeGroupElm
{
    public DBRecipe recipe;
}
