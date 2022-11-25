using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ev", menuName = "DB/Yscript")]
public class DB_YScript : ScriptableObject
{
    /// <summary>
    /// フラグリスト
    /// </summary>
    [HideInInspector] public List<DB_YScriptE> data = new List<DB_YScriptE>();
    /// <summary>
    /// 起動方法
    /// </summary>
    public eYScript.exe exe;
    /// <summary>
    /// Yunaスクリプト
    /// </summary>
    [SerializeField, TextArea(1, 100)]
    public string yscript;
}
[System.Serializable]
public class DB_YScriptE
{
    /// <summary>
    /// フラグ変数の種類
    /// </summary>
    public eYScript.type type;
    /// <summary>
    /// フラグ値
    /// </summary>
    public byte num;
    /// <summary>
    /// 演算子
    /// </summary>
    public eYScript.flag flag; 
}
