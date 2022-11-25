using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SubMenu : UI_SequentialMenu
{
    [SerializeField] protected UI_AnimFade ui = default;
    public override void Begin()
    {
        base.Begin();
        ui.active = true;
    }
    public override void End()
    {
        base.End();
        ui.active = false;
    }
    public override void ToMainMenu()
    {
        base.ToMainMenu();
        ui.active = false;
    }
}
