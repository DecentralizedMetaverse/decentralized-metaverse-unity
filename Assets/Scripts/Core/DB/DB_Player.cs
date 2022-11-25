using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DB_Player", menuName = "Data/DB_Player")]
public class DB_Player : DB_Status
{
    public bool isRecovery;
    public int money;
    public string avatar;
    public Vector3 position;
    public Vector3 rotation;
    public int pl_cx;
    public int pl_cz;

    /// <summary>
    /// 部位とitemId
    /// </summary>
    public Dictionary<string, string> equipData = new Dictionary<string, string>();
    public Dictionary<string, string> equipDataSub = new Dictionary<string, string>();

    [HideInInspector] public bool aimMode;
    [HideInInspector] public Transform transform;
}
