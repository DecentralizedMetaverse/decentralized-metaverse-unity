using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AnimBase : MonoBehaviour
{
    [System.NonSerialized] public CanvasGroup group;
    [SerializeField] protected bool _active;
    //切り替えのメソッドを呼び出す
    public delegate void Function(bool type);
    /// <summary>
    /// アニメーション終了時に呼び出されるメソッド
    /// </summary>
    public Function Finished;

    public virtual bool active { get => _active; set => _active = value; }

    protected virtual void Awake()
    {
        group = gameObject.AddComponent<CanvasGroup>();

        //初期化　alphaの設定
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
