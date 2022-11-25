using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    bool isGrabbing;
    RaycastHit hit;
    Transform grabbingObj;
    Transform mainCamera;
    LayerMask mask;

    Transform parent;

    bool stateEnter = false;

    private void Start()
    {
        mainCamera = Camera.main.transform;
        mask = LayerMask.GetMask("Movable");
    }    

    void Update()
    {
        if (GM.pause == ePause.mode.GameStop) return;

        if (isGrabbing)
        {
            //つかんでいる時の処理
            HoldingHundle();            
        }
        
        if (InputF.GetButtonDown(eInputMap.data.Submit)) OnClick();

        bool isHit = IsHittingObject(); //オブジェクトにレイが当たっているかどうか
        if (isHit != stateEnter && !isGrabbing)
        {
            stateEnter = isHit;
            OnEnter(isHit);
        }
        if (!isHit) return;
    }

    bool IsHittingObject()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!Physics.Raycast(pos, mainCamera.forward, out hit, 500, mask)) return false;
        return true;
    }

    void HoldingHundle()
    {
        //オブジェクトの回転
        var x = InputF.GetAxis(eInputMap.data.Vertical2);
        var y = InputF.GetAxis(eInputMap.data.Horizontal2);
        grabbingObj.Rotate(x, y, 0);

        //オブジェクトの距離の変更
        var d = InputF.GetAxis(eInputMap.data.Scroll);
        grabbingObj.position = Vector3.LerpUnclamped(grabbingObj.position, mainCamera.position, -d);
    }

    /// <summary>
    /// 決定キーを押した場合の処理
    /// </summary>
    void OnClick()
    {
        if (!isGrabbing)
        {
            if(hit.transform == null) return;
            //オブジェクトを持っていない場合
            GrabObject(hit);
            GM.Msg("Hint", true, "離す(左クリック)");
            GM.pause = ePause.mode.UIStop;
            isGrabbing = true;
        }
        else
        {
            //オブジェクトを既に持っている場合
            ReleseObject();
            GM.Msg("Hint", true, "つかむ(左クリック)");
            GM.pause = ePause.mode.none;
            isGrabbing = false;
        }
    }

    void OnEnter(bool enter)
    {
        if (enter)
        {
            GM.Msg("Hint", true, "つかむ(左クリック)");
        }
        else
        {
            GM.Msg("Hint", false);
        }
    }    

    void GrabObject(RaycastHit hit)
    {
        grabbingObj = hit.transform;
        parent = grabbingObj.parent;
        hit.transform.SetParent(mainCamera);
    }

    void ReleseObject()
    {
        grabbingObj.transform.SetParent(parent);

        ObjectData data;
        if (grabbingObj.TryGetComponent<ObjectData>(out data))
        {
            GM.Msg("object.set", data.id); // 保存
        }
        grabbingObj = null;
        parent = null;
    }    
}
