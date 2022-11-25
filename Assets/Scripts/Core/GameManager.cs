using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// ゲーム管理クラス
/// </summary>
public class GameManager : MonoBehaviour, GM_Msg
{
    bool quit = false;
    private void Start()
    {
        GM.pause = ePause.mode.none;
        GM.Add("quit", this);
        GM.Add("db.save", this);
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if (data1 == "quit") GM.GameQuit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            GM.GameQuit();
        }
    }
}
/// <summary>
/// メッセージ管理
/// </summary>
public static class GM
{
    public static uint id;

    static Dictionary<string, List<GM_Msg>> data = new Dictionary<string, List<GM_Msg>>();
    /// <summary>
    /// 送信
    /// </summary>
    /// <param name="data1">データ1</param>
    /// <returns>true:成功 false:失敗</returns>
    public static bool Msg(string data1)
    {
        return Msg(data1, "");
    }
    

    /// <summary>
    /// 送信
    /// </summary>
    /// <param name="data1">データ1</param>
    /// <param name="data2">データ1,データ2,データ3,...</param>
    /// <returns>true:成功 false:失敗</returns>
    public static bool Msg(string data1, params object[] data2)
    {
        if (!data.ContainsKey(data1))
        {
            Debug.Log($"<color=#ff4500>送信失敗 failed:</color> <b>{data1}</b>");
            return false;
        }
        for (int i = 0; data.ContainsKey(data1) && i < data[data1].Count; i++)
        {
            GM.data[data1][i].Receive(data1, data2);

            var msg = $"<color=cyan>実行:</color> <b><color=#ffa500>{data1}</color> ";
            for (int j = 0; j < data2.Length; j++)
            {
                msg += $"<color=#ffd700>{data2[j]}</color> ";
            }
            msg += "</b>";
            Debug.Log(msg);
        }
        return true;
    }    

    /// <summary>
    /// 受信先追加
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj">ここでは「this」を入力してください</param>
    public static void Add(string key, GM_Msg obj)
    {
        if (!data.ContainsKey(key))
        {
            data.Add(key, new List<GM_Msg>());
            //printWarning($"{key}:既に受信先に追加されています");
            //return;
        }
        data[key].Add(obj);
        Debug.Log($"<color=#1e90ff>受信先追加:</color> <b>{key}</b>");
    }
    /// <summary>
    /// 受信先削除
    /// </summary>
    /// <param name="key"></param>
    /// <returns>true:成功 false:失敗</returns>
    public static bool Remove(string key, GM_Msg obj)
    {
        if (!data.ContainsKey(key))
        {
            Debug.Log($"<color=#ff4040>受信先削除失敗:</color> <b>{key}</b>");
            return false;
        }
        GM.data[key].Remove(obj);
        
        if(GM.data[key].Count == 0) 
            GM.data.Remove(key);
        Debug.Log($"<color=#dda0dd>受信先削除:</color> <b>{key}</b>");
        return true;
    }

    /// <summary>
    /// ポーズモード
    /// 入力例(
    /// Gm.pose = ePose.mode.UIStop;
    /// Gm.pose = ePose.mode.GameStop;
    /// Gm.pose = ePose.mode.none;
    /// )
    /// </summary>
    static ePause.mode _pause;
    public static ePause.mode pause
    {
        get { return _pause; }
        set
        {
            _pause = value;
            if (value == ePause.mode.GameStop)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (value == ePause.mode.none)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    /// <returns></returns>
    public static void GameQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif
    }
}