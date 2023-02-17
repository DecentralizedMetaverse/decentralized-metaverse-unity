using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UI_ShowClose : MonoBehaviour
{
    [System.NonSerialized] public CanvasGroup group;
    [SerializeField] protected bool _active;

    public bool active
    {
        get
        {
            return _active;
        }
        set
        {
            if (value) Show().Forget();
            else Close().Forget();
        }
    }

    public delegate void OnCompleteDelegate();
    public delegate void OnComplete(bool a);
    /// <summary>
    /// �A�j���[�V�����I�����ɌĂяo����郁�\�b�h
    /// </summary>    
    public OnCompleteDelegate OnShow, OnClose;
    public OnComplete Finished;

    public abstract UniTask Show();
    public abstract UniTask Close();

    protected void StartShow()
    {
        _active = true;
        SetInteractable(true);
        OnShow?.Invoke();
    }

    protected void StartClose()
    {
        _active = false;
        SetInteractable(false);
        OnClose?.Invoke();
    }

    protected void EndShow()
    {
        Finished?.Invoke(true);
    }

    protected void EndClose()
    {
        Finished?.Invoke(false);
    }

    protected virtual void Awake()
    {
        if (group == null) group = GetComponent<CanvasGroup>();

        //�������@alpha�̐ݒ�
        SetInit(_active);
    }

    /// <summary>
    /// UI�������
    /// </summary>
    /// <param name="b"></param>
    protected void SetInit(bool b)
    {
        group.alpha = b ? 1 : 0;
        group.interactable = b;
        group.blocksRaycasts = b;
    }

    /// <summary>
    /// ����\�E�s�\
    /// </summary>
    /// <param name="b"></param>
    protected void SetInteractable(bool b)
    {
        group.interactable = b;
        group.blocksRaycasts = b;
    }
}
