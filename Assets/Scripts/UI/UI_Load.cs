using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UI_AnimFade))]
public class UI_Load : MonoBehaviour, GM_Msg
{
    UI_AnimFade anim;
    void Awake()
    {
        anim = GetComponent<UI_AnimFade>();
        GM.Add("Load",this);
        anim.Finished = Finished;
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if(data2[0].ToString()== "true")
        {
            anim.active = true;
        }
        else anim.active = false;
    }

    void Finished(bool type)
    {
        if (type)
        {
            GM.Msg("Completed.LoadScreen");
            //Invoke("Completed", 0.25f);
        }
    }

    void Completed()
    {
        
    }
}
