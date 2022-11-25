using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Login : MonoBehaviour, GM_Msg
{
    [SerializeField] InputField fieldUid;
    [SerializeField] InputField fieldPassword;

    private void Start()
    {
        GM.Add("ynet.ui.login", this);
    }
    public void OnClickLogin()
    {
        GM.Msg("ynet", "login", fieldUid.text, fieldPassword.text);
    }
    void GM_Msg.Receive(string data1, params object[] data2)
    {

    }
}
