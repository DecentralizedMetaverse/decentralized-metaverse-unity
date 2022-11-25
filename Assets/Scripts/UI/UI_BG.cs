using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 背景画像表示
/// bg {背景番号(0は何も表示しない)}:{切り替わりの速さ}:{終了までスクリプト停止するか}
/// 1番の背景に切り替え  bg 1(:60:true)
/// bg 1
/// </summary>
public class UI_BG : MonoBehaviour, GM_Msg
{
    public DB_BG db = default;
    public UI_AnimFade ui1 = default;
    public UI_AnimFade ui2 = default;
    public Image bg1 = default; //背面 メインで表示
    public Image bg2 = default; //前面 クロスフェードなど切り替え用
    /// <summary>
    /// 現在背景として使用中の番号
    /// </summary>
    byte n = 0;

    private void Awake()
    {
        if (db == null || ui1 == null || ui2 == null) 
            Debug.LogError(this + " データがありません");

        GM.Add("bg", this);
        ui1.Finished = Finish1;
        ui2.Finished = Finish;
    }

    bool n3 = true; // true: 終わるまでYscriptを止める
    void GM_Msg.Receive(string data1, params object[] data2)
    {
        // 1: bg number
        // 2: change_speed
        // 3: wait script(bool)
        byte n1 = byte.Parse(data2[0].ToString());
        byte n2 = 0;
        n3 = true;

        if (data2.Length > 1)
        {
            n2 = byte.Parse(data2[1].ToString());
            ui1.speed = n2;
            ui2.speed = n2;

            if (data2.Length > 2)
            {
                n3 = bool.Parse(data2[2].ToString());
            }
        }
        else
        {
            ui1.speed = 60;
            ui2.speed = 60;
        }

        //終了まで、処理を止めるか設定
        if (!n3) GM.Msg("YsComplete");

        db.Load(n1);

        if (n1 == 0)
        {
            //消す
            ui1.active = false;
            return;
        }
        bg2.sprite = bg1.sprite;
        bg2.color = bg1.color;
        bg1.sprite = db.data[n1].sprite;
        bg1.color = db.data[n1].color;

        if (!ui1.active && !ui2.active)
        {
            //一番最初に切り替える時
            ui1.active = true;
            return;
        }
        //クロスフェード
        ui2.group.alpha = 1;
        ui2.active = false;
        n = n1;
    }

    void Finish(bool type)
    {
        if (type) return;
        db.UnLoad(n);
        if (n3) GM.Msg("YsComplete");
    }

    void Finish1(bool type)
    {
        //if (type) return;
        if (n3) GM.Msg("YsComplete");
    }
}
