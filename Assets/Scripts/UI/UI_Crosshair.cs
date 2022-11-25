using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UI_AnimFade))]
public class UI_Crosshair : MonoBehaviour
{
    [SerializeField] DB_Player player;
    [SerializeField] DB_Cash db;
    [SerializeField] Image crosshairCharge;
    [SerializeField] Image crosshairLockOn;
    [SerializeField] UI_AnimFade uiLockOn;
    bool aim;
    bool flag;
    UI_AnimFade ui;

    Color32 defaultColor = new Color32(255, 255, 255, 255);
    Color32 targetColor = new Color32(50, 255, 200, 255);

    Vector2 middle;

    Transform playerTransform;
    Vector3 point;
    bool target;
    RaycastHit hit;

    void Start()
    {
        ui = GetComponent<UI_AnimFade>();
    }

    void LateUpdate()
    {
        AllChangeUIActive();

        if (playerTransform == null)
        {
            if (player.transform == null) return;
            playerTransform = player.transform.Find("castMagicPos");
        }

        if (player.aimMode)
        {
            RayCastFromPlayer();
            AimUpdate();
        }
        else if (flag)
        {
            flag = false;
            uiLockOn.active = false;
        }
    }

    void RayCastFromPlayer()
    {
        if (Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, Mathf.Infinity))
        {
            point = hit.point;

            if (hit.transform.tag == "Damage") target = true;
            else target = false;
        }
        else
        {
            target = false;
            point = Vector3.zero;
        }
    }

    void AimUpdate()
    {
        //ç¿ïWÇÃïœçX
        if (point == Vector3.zero)
        {
            middle.x = Screen.width * 0.5f;
            middle.y = Screen.height * 0.5f;
            crosshairCharge.rectTransform.position = middle;
        }
        else
        {
            var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, point);
            crosshairCharge.rectTransform.position = pos;
        }

        crosshairCharge.fillAmount = db.magicPower;

        //ìGÇ…è∆èÄÇçáÇÌÇπÇƒÇ¢ÇÈéû
        if (IsTargetEnemy())
        {
            if (flag) return;
            flag = true;
            uiLockOn.active = true;
        }
        else
        {
            if (!flag) return;
            flag = false;
            uiLockOn.active = false;
        }
    }

    bool IsTargetEnemy()
    {
        return target;
    }

    void AllChangeUIActive()
    {
        if (aim == player.aimMode)
            return;

        aim = player.aimMode;

        ui.active = player.aimMode;
    }
}
