using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShowCloseFade : UI_ShowClose
{
    public float speed = 60;

    protected override void Awake()
    {
        if ( group == null) group = GetComponent<CanvasGroup>();
        base.Awake();
    }   

    public async override UniTask Show()
    {
        StartShow();
        
        while (group.alpha < 1)
        {
            if (!active) return;
            group.alpha += speed * Time.unscaledDeltaTime;
            await UniTask.Yield();
        }
        group.alpha = 1;
        EndShow();
    }

    public async override UniTask Close()
    {
        StartClose();

        while (group.alpha > 0)
        {
            if (active) return;
            group.alpha -= speed * Time.unscaledDeltaTime;
            await UniTask.Yield();
        }
        group.alpha = 0;
        EndClose();
    }
}
