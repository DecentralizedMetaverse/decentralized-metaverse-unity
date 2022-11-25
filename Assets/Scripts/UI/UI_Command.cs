using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UI_AnimFade))]
public class UI_Command : MonoBehaviour, GM_Msg
{
    [SerializeField] InputField field;
    UI_AnimFade ui;
    string data;

    void Start()
    {
        ui = GetComponent<UI_AnimFade>();
        GM.Add("commandUI", this);
        ui.Finished = Return;
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        field.text = data2[0].ToString();
        ui.active = true;
        field.Select();
        GM.pause = ePause.mode.GameStop;
    }

    public void Return(bool type)
    {
        if (type) return;

        GM.Msg("commandObj", data);
        GM.pause = ePause.mode.none;
    }

    public void Update()
    {
        if (!ui.active) return;

        //if (GM.pause != ePause.mode.GameStop) return;        

        if (!InputF.GetButtonDown(eInputMap.data.Cancel))
        {
            data = field.text;
            return;
        }

        field.text = data; //キャンセルするとtextがリセットされるので、その違和感をなくす
        ui.active = false;
    }
}
