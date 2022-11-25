using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Message : MonoBehaviour, GM_Msg
{
    [SerializeField] Animator anim;
    [SerializeField] Text text;

    void Start()
    {
        GM.Add("message", this);
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        text.text = data2[0].ToString();
        anim.SetTrigger("active");
    }
}
