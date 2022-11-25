using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(UI_AnimFade))]
public class UI_TitleText : MonoBehaviour, GM_Msg
{
    [SerializeField] Text titleText;
    [SerializeField] Text subText;
    UI_AnimFade ui;
    [SerializeField, Range(1f, 10f)] float time = 3f;
    float _time = 0;
    private void Start()
    {
        ui = GetComponent<UI_AnimFade>();
        GM.Add("title", this);
    }
    void GM_Msg.Receive(string data1, params object[] data2)
    {
        titleText.text = data2[0].ToString();
        subText.text = "";

        if (data2.Length > 1)
        {
            for (int i = 1; i < data2.Length; i++) subText.text += data2[i];
        }
        _time = time;
        ui.active = true;
        ui.speed = 5f;
    }
    void Update()
    {
        if (_time <= 0) return;
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            ui.active = false;
            ui.speed = 2f;
        }
    }
}
