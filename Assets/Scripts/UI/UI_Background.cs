using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(UI_AnimFade))]
public class UI_Background : MonoBehaviour, GM_Msg
{
    [SerializeField] GameObject postProcess;
    UI_AnimFade ui;

    private void Start()
    {
        ui = GetComponent<UI_AnimFade>();
        GM.Add("ui.bg", this);
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if (Convert.ToBoolean(data2[0]))
        {
            ui.active = true;
            postProcess.SetActive(true);
        }
        else
        {
            ui.active = false;
            postProcess.SetActive(false);
        }
    }
}
