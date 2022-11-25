using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : UI_SubMenu
{
    [SerializeField] DB_Player dbPlayer;
    [SerializeField] DItem db;
    [SerializeField] Text[] textStatus;
    [SerializeField] Text[] textEquip;

    public override void Begin()
    {
        base.Begin();
        textStatus[0].text = dbPlayer._name;
    }
}
