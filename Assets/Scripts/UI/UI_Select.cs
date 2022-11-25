using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class UI_Select : MonoBehaviour, GM_Msg
{
    [SerializeField] UI_AnimFade ui;
    [SerializeField] GameObject prefab;
    [SerializeField] Text titleText;
    [SerializeField] UI_SetNavigation setNavigation;
    void Start()
    {
        GM.Add("yesno", this);
        GM.Add("select", this);
        GM.Add("select.onclick", this);
    }
    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if(data1 == "select.onclick")
        {
            OnClick(byte.Parse(data2[0].ToString()));
            return;
        }

        titleText.gameObject.SetActive(false);

        if (data1 == "yesno")
        {
            
            Select(new string[] { "はい", "いいえ" });
            return;
        }
        if (data2.Length >= 2 && data2[1].ToString() == ":")
        {
            if (data2.Length == 2)
            {
                GM.Msg("YsComplete");
                return;
            }
            titleText.gameObject.SetActive(true);
            titleText.text = data2[0].ToString();
            Select(((string[])data2).Skip(2).ToArray());
        }
        else
        {
            Select((string[])data2);
        }
    }

    void Select(string[] select)
    {
        GM.pause = ePause.mode.GameStop;
        GM.Msg("StopMessage");


        //初期化(ボタンを全て削除)
        foreach (Transform child in transform.GetChild(1))
        {
            if (child.gameObject.name == "Text") continue;
            Destroy(child.gameObject);
        }
        ui.active = true;
        Button[] buttons = new Button[select.Length];
        for (byte i = 0; i < select.Length; i++)
        {
            buttons[i] = Instantiate(prefab, transform.GetChild(1)).GetComponent<Button>();
            buttons[i].transform.GetChild(0).GetComponent<Text>().text = select[i];
            byte n = i;
            buttons[i].onClick.AddListener(() => OnClick(n));
        }
        setNavigation.SetNavVertical(buttons);
        //buttons[0].Select();
    }

    void OnClick(byte i)
    {
        ui.active = false;
        GM.Msg("EndMessage");
        GM.Msg("YsResult", i);
    }
}
