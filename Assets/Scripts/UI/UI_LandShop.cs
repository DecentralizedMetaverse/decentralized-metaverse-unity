using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_LandShop : MonoBehaviour, GM_Msg
{
    [SerializeField] GameObject landObj;
    Transform player;

    void Start()
    {
        GM.Add("land", this);
        landObj.SetActive(false);
    }    

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        if (data2[0].ToString()== "start")
        {
            landObj.SetActive(true);
            GM.pause = ePause.mode.GameStop;

            var size = float.Parse(data2[1].ToString());
            landObj.transform.localScale = new Vector3(size, size, size);
            
            var pos = new Vector3();
            pos.x = (float)Math.Round(player.position.x) - 0.5f;
            pos.y = (float)Math.Round(player.position.y);
            pos.z = (float)Math.Round(player.position.z) + size * 0.75f;
            landObj.transform.position = pos;
        }
        else
        {
            GM.pause = ePause.mode.none;
            landObj.SetActive(false);
        }
    }
}
