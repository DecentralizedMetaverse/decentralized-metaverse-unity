using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UI_AnimFade))]
public class UI_Destroy : MonoBehaviour
{
    UI_AnimFade ui;
    bool end = false;
    public bool waitKey = true;
    float time = 0;
    private void Start()
    {
        ui = GetComponent<UI_AnimFade>();
    }
    public void DestroyByTime(float time)
    {
        this.time = time;
        end = true;
    }
    void Update()
    {
        if (!end) return;
        if (time > 0) { time -= Time.deltaTime; return; }
        if (!waitKey || Input.anyKey) ui.active = false;
        if (ui.group.alpha == 0) Destroy(gameObject);
    }
}
