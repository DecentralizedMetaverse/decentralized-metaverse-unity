using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UI_AnimFade))]
public class UI_FieldStatusBar : UI_StatusBar
{
    UI_AnimFade ui;
    float time = 0f;
    private void Awake()
    {
        ui = GetComponent<UI_AnimFade>();
    }

    public override int value
    {
        get => base.value;
        set
        {
            time = 5f;
            ui.speed = 60f;
            ui.active = true;
            base.value = value;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (time > 0)
        {
            time -= Time.deltaTime;
            return;
        }

        if (time < -1f) return;

        ui.speed = 1f;
        ui.active = false;
        time = -2f;
    }
}
