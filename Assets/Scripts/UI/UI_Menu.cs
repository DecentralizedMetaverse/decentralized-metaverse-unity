using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] UI_AnimBase ui;

    void Update()
    {
        if (IsUsingAnotherUI()) return;
        if (!InputF.GetButtonDown(eInputMap.data.Cancel)) return;
        ui.active = ui.active ? false : true;
        GM.pause = ui.active ? ePause.mode.GameStop : ePause.mode.none;
    }

    bool IsUsingAnotherUI()
    {
        return !ui.active && GM.pause == ePause.mode.GameStop ||
            GM.pause == ePause.mode.UIStop;
    }
}
