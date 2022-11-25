using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputHandler
{
    public static bool GetSubmitDown()
    {
        if (Input.GetButtonDown("Submit")|| Input.GetButtonDown("Fire1")) return true;
        return false;
    }
    public static bool GetSubmit()
    {
        if (Input.GetButton("Submit") || Input.GetButton("Fire1")) return true;
        return false;
    }
    public static bool GetCancel()
    {
        if (Input.GetButtonDown("Cancel")) return true;
        return false;
    }
    static Vector3 move;
    public static Vector3 GetMove()
    {
        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");
        return move;
    }
    public static Vector3 GetMoveRaw()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.z = Input.GetAxisRaw("Vertical");
        return move;
    }
    public static Vector3 GetMove2()
    {
        move.x = Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal2");
        move.z = Input.GetAxis("Mouse Y") + Input.GetAxis("Vertical2");
        return move;
    }
    public static bool GetAutoRun()
    {
        if (Input.GetButtonDown("Autorun")) return true;
        return false;
    }
    public static bool GetDush()
    {
        if (Input.GetButton("Dush")) return true;
        return false;
    }
    public static bool GetJump()
    {
        if (Input.GetButtonDown("Jump")) return true;
        return false;
    }
    public static bool GetTriggerR()
    {
        if (Input.GetAxis("LRTrigger") > 0) return true;
        return false;
    }
    public static bool GetTriggerL()
    {
        if (Input.GetAxis("LRTrigger") < 0) return true;
        return false;
    }
    public static bool GetRight()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) return true;
        return false;
    }
    public static bool GetLeft()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) return true;
        return false;
    }
    public static bool GetUp()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) return true;
        return false;
    }
    public static bool GetDown()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) return true;
        return false;
    }
    public static bool GetLockON()
    {
        if (Input.GetButtonDown("Lock-on")) return true;
        return false;
    }
    public static bool GetChange()
    {
        if (Input.GetButtonDown("Change")) return true;
        return false;
    }
    public static bool GetMenu()
    {
        if (Input.GetButtonDown("Menu")) return true;
        return false;
    }
    public static bool GetDebugMenu()
    {
        if (Input.GetKeyDown(KeyCode.Slash)) return true;
        return false;
    }
    public static bool GetControl()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) return true;
        return false;
    }
    public static bool GetAttack()
    {
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1")) return true;
        return false;
    }
}
