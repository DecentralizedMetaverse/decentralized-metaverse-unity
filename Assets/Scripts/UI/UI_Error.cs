using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(UI_AnimFade))]
public class UI_Error : MonoBehaviour, GM_Msg
{
    UI_AnimFade ui;
    void Start()
    {
        ui = GetComponent<UI_AnimFade>();
        GM.Add("error", this);
    }
    void GM_Msg.Receive(string data1, params object[] data2)
    {
        ui.active = true;
    }
    void Update()
    {
        if (ui.active && InputF.GetButtonDown(eInputMap.data.Submit))
        {
            GM.GameQuit();
        }
    }
}
