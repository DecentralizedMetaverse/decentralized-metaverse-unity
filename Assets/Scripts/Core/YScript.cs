using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// [禁止] コンポーネントに追加しないで
/// Game Manager Only
/// </summary>
public class YScript : MonoBehaviour, GM_Msg
{
    [SerializeField] DB_User dbUser;
    /// <summary>
    /// 強制終了
    /// </summary>
    public static bool stop;
    bool complete = false;
    int scriptID = 0;

    private void Awake()
    {
        GM.Add("Script", this);
        GM.Add("ScriptID", this);
        GM.Add("YsComplete", this); //各イベント終了通知
        GM.Add("YsResult", this); //各イベント終了通知
        //Execute
        //(
        //    "変数 = 30\n" +
        //    "変数2\n" +
        //    "if 変数 == 30\n" +
        //    "   say 変数1が宣言されています\n" +
        //    "else if 変数2\n" +
        //    "   say 変数2が宣言されています\n" +
        //    "else\n" +
        //    "   say 変数が宣言されていません\n" +
        //    "end\n"
        //);
    }
    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if (data1 == "Script") StartCoroutine(Execute(data2[0].ToString()));
        else if (data1 == "ScriptID") scriptID = int.Parse(data2[0].ToString());
        else if (data1 == "YsComplete") complete = true;
        else if (data1 == "YsResult") { Variable("ret", data2[0].ToString()); complete = true; }
        else if (data1 == "unloaded")
        {
            //シーン読み込み後にプレイヤーはシーンの開始位置にテレポートさせる
            GM.Remove("unloaded", this);
            GM.Msg("spawn.start");
        }
    }
    /// <summary>
    /// 命令実行
    /// </summary>
    /// <param name="commandText"></param>
    IEnumerator Execute(string commandText)
    {
        string[][] data1 = null;      
        data1 = YScriptTest.GetToken(commandText);
        yield return StartCoroutine(Exe(data1));

        GM.Msg("CompleteScript", scriptID);
        yield break;
    }
    IEnumerator ExecuteC(string commandText)
    {
        string[][] data1 = null;
        data1 = YScriptTest.GetToken(commandText);
        yield return StartCoroutine(Exe(data1));

        yield break;
    }
    
    IEnumerator Exe(string[][] data1)
    {
        bool toEnd = false;
        for (int i = 0; i < data1.Length; i++)
        {
            complete = false;

            if (stop)
            {
                stop = false;
                yield break;
            }

            if (data1[i].Length == 0) continue;
            switch (data1[i][0])
            {
                default:
                    //変数操作
                    if (data1[i].Length < 3) break;
                    if (data1[i][1] != "=") break;
                    Variable(data1[i][0], data1[i][2]);
                    break;
                case "":
                case "//":
                    break;
                case "if":
                    if (Compare(data1[i].Skip(1).ToArray()))
                    {
                        //真
                        toEnd = true;
                    }
                    else
                    {
                        //偽
                        //else or endまでスキップ
                        try
                        {
                            do { i++; }
                            while (i < data1.Length &&
                                data1[i].Length != 0 &&
                                data1[i][0] != "else" &&
                                data1[i][0] != "end"
                                );
                            i--;
                        }
                        catch (Exception ex)
                        {
                            Debug.LogWarning(ex.Message);
                        }
                    }
                    break;
                case "else":
                    if (toEnd)
                    {
                        //endまでスキップ
                        do { i++; } 
                        while (i < data1.Length &&
                                data1[i].Length != 0 &&
                                data1[i][0] != "end");
                        i--;
                        break;
                    }
                    if (data1[i].Length == 1) break;

                    if (data1[i].Length > 1 && data1[i][1] == "if")
                    {
                        if (Compare(data1[i].Skip(2).ToArray()))
                        {
                            //真
                            toEnd = true;
                            break;
                        }
                    }
                    //偽 or else
                    do { i++; }
                    while (i < data1.Length &&
                                data1[i].Length != 0 &&
                                data1[i][0] != "else" &&
                                data1[i][0] != "end"
                    );
                    i--;
                    break;

                case "end":
                    toEnd = false;
                    break;

                case "say":
                    //変数検索
                    var array = data1[i].Skip(1).ToArray();
                    ReplaceVariable(ref array[0]);
                    if (array.Length > 1) ReplaceVariable(ref array[1]);
                    GM.Msg("say", array);
                    yield return new WaitUntil(() => complete);
                    break;

                case "select":
                    //変数検索
                    ReplaceVariable(ref data1[i]);
                    GM.Msg("select", data1[i].Skip(1).ToArray());
                    yield return new WaitUntil(() => complete);
                    break;

                case "yesno":
                    GM.Msg("yesno");
                    yield return new WaitUntil(() => complete);
                    break;

                case "pause":
                    if (data1[i].Length == 1) break;
                    if (data1[i][1] == "ui") GM.pause = ePause.mode.UIStop;
                    else if (data1[i][1] == "none") GM.pause = ePause.mode.none;
                    else if (data1[i][1] == "game") GM.pause = ePause.mode.GameStop;
                    break;

                case "bg":
                    GM.Msg("bg", data1[i].Skip(1).ToArray());
                    yield return new WaitUntil(() => complete);
                    break;

                case "wait":
                    yield return new WaitForSeconds(float.Parse(data1[i][1]));
                    break;

                case "gm":
                    GM.Msg(data1[i][1], data1[i].Skip(2).ToArray());
                    break;

                case "bgm":
                    GM.Msg("bgm", data1[i][1]);
                    break;

                case "se":
                    GM.Msg("se", data1[i][1]);
                    break;

                case "scene":
                    GM.Add("unloaded", this);
                    SM.Load((eScene.Scene)Enum.Parse(typeof(eScene.Scene), data1[i][1]));
                    break;                

                case "fn":
                    //アセットのロード
                    var handle = Addressables.LoadAssetAsync<TextAsset>($"DB/Scripts/{data1[i][1]}.ys");
                    yield return handle;

                    //失敗した場合
                    if (handle.Status != AsyncOperationStatus.Succeeded) 
                    {
                        Debug.LogError(this + $" functions {data1[i][1]} が読み込めませんでした");
                        yield break;
                    }
                    var text = handle.Result.text;
                    yield return StartCoroutine(ExecuteC(text));
                    break;

                case "return":
                    yield break;

                case "title":
                    GM.Msg("title", data1[i].Skip(1).ToArray());
                    break;

                case "msg":
                    GM.Msg("message", data1[i].Skip(1).ToArray());
                    break;

                case "active":
                    GM.Msg("object.active", data1[i].Skip(1).ToArray());
                    break;
                
                case "is_active":
                    GM.Msg("object.is_active", data1[i].Skip(1).ToArray());
                    break;

                case "location":
                    GM.Msg("say", 
                        $"position: {dbUser.GetData(GM.id).obj.transform.position}+" +
                        $"rotation: {dbUser.GetData(GM.id).obj.transform.rotation.eulerAngles}"
                    );
                    break;

                case "give":
                    if (data1[i].Length == 4) GM.Msg("player.inventory", "add", data1[i][1], data1[i][2], data1[i][3]);
                    else GM.Msg("player.inventory", "add", data1[i][1], data1[i][2]);
                    break;

                case "tp":
                    foreach(var dt in data1[i])
                    {
                        print(dt);
                    }
                    Vector3 pos = new Vector3(float.Parse(data1[i][1]), float.Parse(data1[i][2]), float.Parse(data1[i][3]));
                    GM.Msg("tp", pos);
                    break;
            }
        }
        yield break;
    }
    string[] op = new string[] { "==", ">=", "<=", "<", ">" };
    bool Compare(string[] data)
    {
        //{"if", "ret", "==", "0", "&&", "a", "==", "0", "&&"}
        //{"if", "!", "ret", "==", "0", "&&", "a", "==", "0", "&&"}
        bool result = false;
        bool add = true;
        int n1, n2;
        int i = 0;
        if (data[0] == "!")
        {
            i++;
            add = false;
        }

        //変数の値取得
        n1 = CheckVariable(data[i]);
        if (data.Length < i + 2)
        {
            if (n1 == 1) return true;
            else return false;
        }
        n2 = CheckVariable(data[i + 2]);

        //operator番号取得
        int opNum = 0;
        for (int j = 0; j < op.Length; j++)
        {
            if (data[i + 1] == op[j])
            {
                opNum = j;
                break;
            }
        }
        //値の比較
        if (opNum == 0 && n1 == n2) result = true;
        else if (opNum == 1 && n1 >= n2) result = true;
        else if (opNum == 2 && n1 <= n2) result = true;
        else if (opNum == 3 && n1 < n2) result = true;
        else if (opNum == 4 && n1 > n2) result = true;
        else result = false;

        //複数の条件分岐
        if (data.Length > 3)
        {
            if (data[i + 3] == "&&")
            {
                //&&以降の部分を取得
                string[] data2 = new string[data.Length - (i + 4)];
                Array.Copy(data, i + 4, data2, 0, data2.Length);
                return add && result && Compare(data2);
            }
            else if (data[i + 3] == "||")
            {
                //&&以降の部分を取得
                string[] data2 = new string[data.Length - (i + 4)];
                Array.Copy(data, i + 4, data2, 0, data2.Length);
                return add && (result || Compare(data2));
            }
        }
        return add && result;
    }
    int CheckVariable(string data)
    {
        int n = 0;
        if (!int.TryParse(data, out n))
        {
            if (num.ContainsKey(data)) n = num[data];
            else if (flag.ContainsKey(data) && flag[data]) n = 1;
            else n = 0;
        }
        return n;
    }
    Dictionary<string, bool> flag = new Dictionary<string, bool>();
    Dictionary<string, string> str = new Dictionary<string, string>();
    Dictionary<string, int> num = new Dictionary<string, int>();
    /// <summary>
    /// 変数追加
    /// </summary>
    /// <param name="data1">変数名</param>
    /// <param name="data2">値</param>
    void Variable(string data1, string data2)
    {
        int n = 0;
        bool b = false;
        if (bool.TryParse(data2, out b))
        {
            if (!flag.ContainsKey(data1)) flag.Add(data1, b);
            else flag[data1] = b;
        }
        if (int.TryParse(data2, out n))
        {
            if (!num.ContainsKey(data1)) num.Add(data1, n);
            else num[data1] = n;
        }
        else
        {
            if (!str.ContainsKey(data1)) str.Add(data1, data2);
            else str[data1] = data2;
        }
    }
    /// <summary>
    /// 変数検索
    /// </summary>
    /// <param name="data">対象データ</param>
    void ReplaceVariable(ref string[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (flag.ContainsKey(data[i]))
            {
                data[i] = flag[data[i]].ToString();
                continue;
            }
            if (num.ContainsKey(data[i]))
            {
                data[i] = num[data[i]].ToString();
                continue;
            }
            if (str.ContainsKey(data[i]))
            {
                data[i] = str[data[i]];
                continue;
            }
        }
    }
    void ReplaceVariable(ref string data)
    {
        int s = -1;
        foreach (var n in str.Keys)
        {
            s = data.IndexOf(n);
            if (s != -1)
            {
                data = data.Replace(n, str[n]);
            }
        }
        foreach (var n in num.Keys)
        {
            s = data.IndexOf(n);
            if (s != -1)
            {
                data = data.Replace(n, num[n].ToString());
            }
        }
    }
}

