using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AnimFade : UI_AnimBase
{
    public float speed = 60;
    public bool change { get; protected set; }

    /// <summary>
    /// アニメーション終了時に呼び出されるメソッド
    /// </summary>
    float newAlpha = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    public override bool active
    {
        get
        {
            return _active;
        }
        set
        {
            _active = value;

            if (_active) newAlpha = 1;
            else newAlpha = 0;
            
            change = true;
        }
    }

    void Update()
    {
        if (!change) return;

        if (group.alpha < newAlpha)
        {
            group.interactable = true;
            group.blocksRaycasts = true;
            group.alpha += speed * Time.unscaledDeltaTime;
            System.Math.Max(group.alpha, 1);
        }
        else if (group.alpha > newAlpha)
        {
            group.interactable = false;
            group.blocksRaycasts = false;
            group.alpha -= speed * Time.unscaledDeltaTime;
            System.Math.Min(group.alpha, 0);
        }
        else
        {
            change = false;
            Finished?.Invoke(active);
        }
    }
}
