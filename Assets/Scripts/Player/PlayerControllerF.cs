using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerF : NetworkBehaviour
{
    Rigidbody rb;
    [SerializeField, Range(1f, 100f)] float moveSpeed = 1.0f;
    [SerializeField, Range(1f, 10f)] float jumpPower = 1.25f;
    [System.NonSerialized] public Animator animator;

    public Vector3 moveInput; //入力値格納用

    //アニメーション
    readonly int animWalkSpeed = Animator.StringToHash("speed");
    readonly int animAction = Animator.StringToHash("action");
    int animActionNum = 0;

    bool autoRun;
    public static bool allowMove = true;
    Transform targetCamera;

    //ジャンプ
    public float gravity = 9.81f;
    protected const float _gravity = 9.81f;
    protected const float _gravity2 = 1.225f;

    int layerMask = 0;

    void Start()
    {
        if (!isLocalPlayer) return;
        gameObject.tag = "Player";
        if (!TryGetComponent(out rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        //animator = GetComponent<Animator>();

        layerMask = LayerMask.GetMask("Map", "Water");
        targetCamera = Camera.main.transform;
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        moveInput = Vector3.zero;
        animActionNum = 0;

        //移動が許可されているか
        if (!IsAllowMove()) return;
            
        //高さ範囲外 強制転移
        if (transform.position.y < -20)
        {
            //var pos = transform.position;
            //pos.y = 300;
            GM.Msg("spawn");
        }

        //入力情報取得
        GetInput();

        moveInput *= moveSpeed;

        //オートラン
        AutoRun();

        //カメラから移動量を計算
        CalcMovementFromCamera();

        //ジャンプ
        if (IsJump())
        {
            rb.AddForce(0, 10000f * jumpPower, 0);
        }

        //移動チェック
        if (IsMovement())
        {
            //キャラの向きを変える
            ChangeAngleFromInput(moveInput);

            //
            RaycastHit hit;
            if (Physics.Raycast(
                transform.position + moveInput + Vector3.up,
                -transform.up,
                out hit,
                2.0f,
                layerMask
            ))
                moveInput.y -= hit.distance - 1.0f;

            //ダッシュ
            if (InputF.GetButton(eInputMap.data.Dash))
            {
                moveInput *= 2f;
            }
        }

        //移動
        if (animator == null) return;
        animator.SetFloat(animWalkSpeed, moveInput.sqrMagnitude * 2);
        animator.SetInteger(animAction, animActionNum);
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        //移動
        //rb.velocity = moveInput * 500f * Time.deltaTime;
        rb.MovePosition(rb.position + moveInput * 5f * Time.deltaTime);
        //rb.AddForce(moveForceMultiplier * (moveInput - rb.velocity));
    }

    bool IsCameraChange()
    {
        return
            GM.pause == ePause.mode.none &&
            InputF.GetButton(eInputMap.data.Submit);
    }

    bool IsJump()
    {
        return
            GM.pause != ePause.mode.GameStop &&
            InputF.GetButtonDown(eInputMap.data.Jump) &&
            Physics.CheckSphere(transform.position, 0.25f, layerMask);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + transform.forward + Vector3.up, -transform.up * 2.0f);
    }

    bool IsAutoRun()
    {
        return
            GM.pause != ePause.mode.GameStop &&
            InputF.GetButtonUp(eInputMap.data.AutoRun);
    }

    bool IsMovement()
    {
        return moveInput != Vector3.zero;
    }

    bool IsAllowMove()
    {
        if (allowMove) return true;
        if (animator == null) return false;

        animator.SetFloat(animWalkSpeed, 0);
        return false;
    }

    /// <summary>
    /// 入力情報を取得
    /// </summary>
    void GetInput()
    {
        moveInput.x = InputF.GetAxis(eInputMap.data.MoveX);
        moveInput.z = InputF.GetAxis(eInputMap.data.MoveY);
    }

    /// <summary>
    /// オートラン
    /// </summary>
    void AutoRun()
    {
        //オートラン
        if (IsAutoRun())
        {
            autoRun = autoRun ? false : true;

            if (autoRun) GM.Msg("message", "Start Auto Run");
            else GM.Msg("message", "Stop Auto Run");
        }

        if (autoRun) moveInput.z = 1f;
    }

    /// <summary>
    /// カメラの角度から移動量を計算
    /// </summary>
    void CalcMovementFromCamera()
    {
        var forward = targetCamera.transform.forward;
        forward.y = 0;
        moveInput = (targetCamera.transform.right * moveInput.x) + (forward * moveInput.z);
    }

    /// <summary>
    /// Change the character's orientation
    /// </summary>
    /// <param name="angle"></param>
    public void ChangeAngleFromInput(Vector3 angle, float speed = 10f)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(angle), Time.deltaTime * speed);
    }
}
