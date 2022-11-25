using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AnimBase : MonoBehaviour
{
    [System.NonSerialized] public CanvasGroup group;
    [SerializeField] protected bool _active;
    //�؂�ւ��̃��\�b�h���Ăяo��
    public delegate void Function(bool type);
    /// <summary>
    /// �A�j���[�V�����I�����ɌĂяo����郁�\�b�h
    /// </summary>
    public Function Finished;

    public virtual bool active { get => _active; set => _active = value; }

    protected virtual void Awake()
    {
        group = gameObject.AddComponent<CanvasGroup>();

        //�������@alpha�̐ݒ�
        SetGroup(_active);
        active = _active;
    }

    protected void SetGroup(bool b)
    {
        group.alpha = b ? 1 : 0;
        group.interactable = b;
        group.blocksRaycasts = b;
    }

    protected void SetInteractable(bool b)
    {
        group.interactable = b;
        group.blocksRaycasts = b;
    }

    public void Switch()
    {
        active = active ? false : true;
    }
}
