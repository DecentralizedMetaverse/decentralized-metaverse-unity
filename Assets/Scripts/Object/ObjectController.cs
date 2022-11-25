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
            //����ł��鎞�̏���
            HoldingHundle();            
        }
        
        if (InputF.GetButtonDown(eInputMap.data.Submit)) OnClick();

        bool isHit = IsHittingObject(); //�I�u�W�F�N�g�Ƀ��C���������Ă��邩�ǂ���
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
        //�I�u�W�F�N�g�̉�]
        var x = InputF.GetAxis(eInputMap.data.Vertical2);
        var y = InputF.GetAxis(eInputMap.data.Horizontal2);
        grabbingObj.Rotate(x, y, 0);

        //�I�u�W�F�N�g�̋����̕ύX
        var d = InputF.GetAxis(eInputMap.data.Scroll);
        grabbingObj.position = Vector3.LerpUnclamped(grabbingObj.position, mainCamera.position, -d);
    }

    /// <summary>
    /// ����L�[���������ꍇ�̏���
    /// </summary>
    void OnClick()
    {
        if (!isGrabbing)
        {
            if(hit.transform == null) return;
            //�I�u�W�F�N�g�������Ă��Ȃ��ꍇ
            GrabObject(hit);
            GM.Msg("Hint", true, "����(���N���b�N)");
            GM.pause = ePause.mode.UIStop;
            isGrabbing = true;
        }
        else
        {
            //�I�u�W�F�N�g�����Ɏ����Ă���ꍇ
            ReleseObject();
            GM.Msg("Hint", true, "����(���N���b�N)");
            GM.pause = ePause.mode.none;
            isGrabbing = false;
        }
    }

    void OnEnter(bool enter)
    {
        if (enter)
        {
            GM.Msg("Hint", true, "����(���N���b�N)");
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
            GM.Msg("object.set", data.id); // �ۑ�
        }
        grabbingObj = null;
        parent = null;
    }    
}
