using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SequentialMenu : MonoBehaviour
{
    [SerializeField] protected GameObject firstSelectObj;
    public GameObject selectedObj = null;
    public bool run;
    protected UI_SequentialMenu mainMenu;
    protected bool skipFrame = false;
    /// <summary>
    /// UI表示
    /// </summary>
    public virtual void Begin()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectObj);
        run = true;
    }

    /// <summary>
    /// UI再表示
    /// </summary>
    public virtual void ReStart()
    {
        EventSystem.current.SetSelectedGameObject(selectedObj);
        run = true;
        skipFrame = true;
    }

    /// <summary>
    /// UI閉じる
    /// </summary>
    public virtual void End()
    {
        selectedObj = null;
        EventSystem.current.SetSelectedGameObject(null);
        run = false;
    }

    /// <summary>
    /// サブ画面に移動
    /// </summary>
    /// <param name="subMenu"></param>
    public virtual void ToSubMenu(UI_SequentialMenu subMenu)
    {
        //現在選択されているゲームオブジェクトを記録
        selectedObj = EventSystem.current.currentSelectedGameObject;
        run = false;
        subMenu.Begin();
        subMenu.mainMenu = this;
    }

    /// <summary>
    /// 前の画面に戻る
    /// </summary>
    public virtual void ToMainMenu()
    {
        run = false;
        if (mainMenu == null) End();
        else mainMenu.ReStart();
        selectedObj = null;
    }
    protected virtual void LateUpdate()
    {
        if (skipFrame)
        {
            skipFrame = false;
            return;
        }

        if (GetButtonDown(eInputMap.data.Cancel))
        {
            ToMainMenu();
        }
    }
    /// <summary>
    /// シーケンシャルメニュー用 GetButton
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected bool GetButton(eInputMap.data key)
    {
        if (!run) return false;
        return InputF.GetButtonDown(key);
    }
    /// <summary>
    /// シーケンシャルメニュー用 GetButtonDown
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected bool GetButtonDown(eInputMap.data key)
    {
        if (!run) return false;
        return InputF.GetButtonDown(key);
    }
    /// <summary>
    /// シーケンシャルメニュー用 GetButtonUp
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected bool GetButtonUp(eInputMap.data key)
    {
        if (!run) return false;
        return InputF.GetButtonDown(key);
    }
}
