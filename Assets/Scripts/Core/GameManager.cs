using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using System;
using Cysharp.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Linq;
using TC;
/// <summary>
/// ゲーム管理クラス
/// TODO: Addメソッドの中身の共通部分を一つにまとめたい
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] DB_GameManager _dbMng;
    [SerializeField] GameObject debugObj;
    [SerializeField] DB_FunctionList dBFunctionList;

#if UNITY_EDITOR
    void OnDestroy()
    {
        GM.SaveFunctionList(dBFunctionList);
    }
#endif

    private void Awake()
    {
        GM.db = _dbMng;
        GM.Init();
        //DOTween.Init(); // 最初に再生したときに画面が止まらないようにここで呼び出す
#if UNITY_EDITOR

        GM.db.SetSceneName();
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
    }

    private void Start()
    {
        InputF.action.Debug.Quit.performed += OnQuit;

        debugObj.SetActive(GM.db.visiblePerformance);
    }

    void OnQuit(InputAction.CallbackContext contex)
    {
        GM.GameQuit();
    }

    [ContextMenu("Get Name")]
    public void Test(Delegate method)
    {
        var info = method.GetMethodInfo();
        var methodName = $"CS.{info.DeclaringType.FullName}.{info.Name}";
    }
}