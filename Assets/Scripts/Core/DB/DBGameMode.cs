using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DBGameMode", menuName = "DB/System/DBGameMode")]
public class DBGameMode : ScriptableObject
{
    [SerializeField] bool _debugMode; //これをoffにすると全部offになる
    [SerializeField] bool _debugMenu;
    [SerializeField] bool _title;
    [SerializeField] bool _bgm;
    [SerializeField] bool _se;
    [SerializeField] bool _dush;
    [SerializeField] bool _scriptTest;
    [SerializeField] bool _log;
    public bool debugMenu
    {
        get
        {
            if (_debugMode) return _debugMenu;
            else return true;
        }
    }
    public bool title
    {
        get
        {
            if (_debugMode) return _title;
            else return true;
        }
    }
    public bool loadBgm
    {
        get
        {
            if (_debugMode) return _bgm;
            else return true;
        }
        set { _bgm = value; }
    }
    public bool loadSe
    {
        get
        {
            if (_debugMode) return _se;
            else return true;
        }
        set { _se = value; }
    }
    public bool dush
    {
        get
        {
            if (_debugMode) return _dush;
            else return false;
        }
        set { _dush = value; }
    }
    public bool scriptTest
    {
        get
        {
            if (_debugMode) return _scriptTest;
            else return false;
        }
        set { _scriptTest = value; }
    }
    public bool log
    {
        get
        {
            return _log;
        }
        set
        {
            if (!_debugMenu) _log = false;
            else
            {
                _log = value;
                Debug.unityLogger.logEnabled = _log; //ログの出力
            }
        }
    }
}
