using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBMagic", menuName = "DB/DBMagic")]
public class DBMagic : ScriptableObject
{
    [HideInInspector] public List<DBMagicElm> data = new List<DBMagicElm>();
    public enum type
    {
        none, damage, healing, _event, other
    }
    public enum effect
    {
        none,
        critical1, critical2, critical3,
        damage1, damage2, damage3,
        healing1, healing2, healing3
    }
    public void SetMagic(DBMagicElm magic, Transform tra)
    {
        if (magic.parent == null)
        {
            GameObject o = new GameObject(magic.prefab.name);
            o.transform.SetParent(GameObject.FindWithTag("Stage").transform);
            magic.parent = o.transform;
        }
        foreach (Transform trans in magic.parent)
        {
            if (!trans.gameObject.activeSelf)
            {
                trans.SetPositionAndRotation(tra.position, tra.rotation);
                trans.gameObject.SetActive(true);
                return;
            }
        }
        var obj = Instantiate(magic.prefab, magic.parent);
        obj.transform.SetPositionAndRotation(tra.position, tra.rotation);

    }

}
[System.Serializable]
public class DBMagicElm
{
    public Transform parent;
    public string magicName;
    public GameObject prefab;
    public float burn;
    public float color;
    public float num;
}

