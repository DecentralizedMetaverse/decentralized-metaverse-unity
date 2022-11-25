using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UI_Anim : UI_AnimBase
{
    Animator anim;
    readonly int animActive = Animator.StringToHash("active");

    protected override void Awake()
    {
        anim = GetComponent<Animator>();
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
            if (value && group.alpha != 1.0f) group.alpha = 1.0f;
            _active = value;
            SetInteractable(value);
            anim.SetBool(animActive, value);
            Finished?.Invoke(value);
        }
    }
}
