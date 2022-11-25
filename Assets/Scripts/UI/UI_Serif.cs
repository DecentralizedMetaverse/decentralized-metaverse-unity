using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Serif : MonoBehaviour, GM_Msg
{
    [SerializeField] UI_AnimFade ui = default;
    [SerializeField] UI_AnimFade uiNameWindow = default;
    [SerializeField] Text charaName = default;
    [SerializeField] Text content = default;

    public bool running = false;
    /// <summary>
    /// 行の間隔時間
    /// </summary>
    public float intervalTime_i = 0.06f;
    /// <summary>
    /// 列の間隔時間
    /// </summary>
    public float intervalTime_j = 0.03f;
    /// <summary>
    /// 行の長さ
    /// </summary>
    public int lineLength = 3;

    List<string> waitListData1 = new List<string>();
    List<string> waitListData2 = new List<string>();

    void Start()
    {
        InitWindow();
        GM.Add("Message", this);
        GM.Add("say", this);
        GM.Add("Name", this);
        GM.Add("StopMessage", this);
        GM.Add("EndMessage", this);
        //GM.Add("if", this);
        //GM.Add("end", this);
        //GM.Add("ret", this);
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if (!running)
        {
            if (data1 == "say")
            {
                if (data2.Length > 1)
                {
                    // 名前あり
                    Begin(data2[1].ToString(), data2[0].ToString());
                }
                else
                {
                    // 名前なし
                    Begin(data2[0].ToString());
                }
            }
            else if (data1 == "Message")
            {
                Begin(data2[0].ToString());
            }
            else if (data1 == "Name")
            {
                charaName.text = data2[0].ToString();
                SendWaitMsg();
            }
            else if (data1 == "StopMessage")
            {
                running = true;
            }
        }
        else if (data1 == "EndMessage")
        {
            running = false;
            End();
        }
        else
        {
            //待ちリストへ追加
            waitListData1.Add(data1);
            waitListData2.Add(data2[0].ToString());
        }
    }

    /// <summary>
    /// UI表示
    /// </summary>
    /// <param name="data"></param>
    public void Begin(string data, string name = "")
    {
        GM.pause = ePause.mode.GameStop;
        InitWindow();
        charaName.text = name;
        ////名前検出
        //var start = data.IndexOf(":");
        //if (start != -1)
        //{
        //    charaName.text = data.Substring(0, start);
        //    data = data.Substring(start + 1);
        //}
        running = true;
        ui.active = true;
        StartCoroutine(UpdateSerif(data.Split('+')));
    }
    /// <summary>
    /// データの初期化
    /// </summary>
    void InitWindow()
    {
        charaName.text = "";
        content.text = "";
    }
    void SendWaitMsg()
    {
        if (waitListData1.Count == 0) return;
        var data1 = waitListData1[0];
        var data2 = waitListData2[0];
        waitListData1.RemoveAt(0);
        waitListData2.RemoveAt(0);
        //待ちリストにあるメッセージを再送
        GM.Msg(data1, data2);
    }

    void End()
    {
        SendWaitMsg();
        if (!running)
        {
            ui.active = false;
            GM.pause = ePause.mode.none;
        }
    }

    public IEnumerator UpdateSerif(string[] data)
    {
        if (charaName.text == "") uiNameWindow.active = false;
        else uiNameWindow.active = true;
        for (var i = 0; i < data.Length; i++)
        {
            for (var j = 0; j < data[i].Length; j++)
            {
                content.text += data[i][j];
                yield return new WaitForSecondsRealtime(intervalTime_j);
            }
            //データが終わっているか
            if ((i + 1) == data.Length) break;  //終了
                                                //表示できる行の限界にきているか
            if ((i + 1) % lineLength == 0)
            {
                //キー入力待ち
                yield return new WaitUntil
                (
                    () => InputF.GetButtonUp(eInputMap.data.Submit)
                );
                content.text = "";
            }
            else
            {
                //改行
                yield return new WaitForSecondsRealtime(intervalTime_i);
                content.text += "\n";
            }
        }
        //キー入力待ち
        yield return new WaitUntil(
            () => InputF.GetButtonUp(eInputMap.data.Submit)
        );
        GM.Msg("YsComplete");
        running = false;
        //GM.Msg("Completed.UI_Serif");
        yield return null;
        End();
    }
}
