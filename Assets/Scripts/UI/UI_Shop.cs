using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Shop : MonoBehaviour, GM_Msg
{
    void Start()
    {
        GM.Add("shop", this);
    }
    void GM_Msg.Receive(string data1, params object[] data2)
    {
    }
}
