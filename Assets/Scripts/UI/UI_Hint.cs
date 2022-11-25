using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UI_AnimFade))]
public class UI_Hint : MonoBehaviour, GM_Msg
{
    [SerializeField] Text text;
    [SerializeField] string defaultText;
    UI_AnimFade ui;
    void Start()
    {
        ui = GetComponent<UI_AnimFade>();
        GM.Add("Hint", this);
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        var flag = (bool)data2[0];

        if (flag)
        {
            if (data2.Length >= 2) text.text = data2[1].ToString();
            else text.text = defaultText;
        }

        ui.active = flag;
    }
}
