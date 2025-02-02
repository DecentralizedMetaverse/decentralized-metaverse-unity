using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UI_ShowCloseAnim : UI_ShowClose
{
    Animator anim;
    readonly int animActive = Animator.StringToHash("active");

    protected override void Awake()
    {
        anim = GetComponent<Animator>();
        base.Awake();
    }

    public async override UniTask Show()
    {
        StartShow();
        
        anim.SetBool(animActive, true);
        
        EndShow();
    }

    public async override UniTask Close()
    {
        StartClose();

        anim.SetBool(animActive, true);
        
        EndClose();
    }
}
