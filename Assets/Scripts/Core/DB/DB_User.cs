using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DB_User", menuName = "DB/DB_User")]
public class DB_User : ScriptableObject
{
    public List<DB_UserE> data = new List<DB_UserE>();
    public Dictionary<uint, int> index = new Dictionary<uint, int>();
    public DB_UserE GetData(uint id)
    {
        if (!index.ContainsKey(id))
        {
            DB_UserE user = new DB_UserE();
            user.id = id;
            data.Add(user);
            index.Add(id, data.Count - 1);
        }
        return data[index[id]];
    }
}

[System.Serializable]
public class DB_UserE
{
    public uint id;
    public string name;
    public string worldId;
    public string avatar;
    public string world;
    public Sprite thumbnail;
    public GameObject obj;

}
            