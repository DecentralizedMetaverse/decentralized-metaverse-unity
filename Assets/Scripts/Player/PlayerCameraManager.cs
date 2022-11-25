using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerCameraManager : MonoBehaviour, GM_Msg
{
    [SerializeField] DB_Settings db;

    [SerializeField] float length = 3;
    [SerializeField] float minLength = -1f;
    [SerializeField] float maxLength = 5f;
    [SerializeField] float[] cameraLength = new float[] { 3, -0.5f };
    [SerializeField] byte cameraType = 0;
    [SerializeField] float limitAngle = 45;
    [SerializeField] float speed = 2.0f;
    [SerializeField] float inputSpeed = 0.5f;
    [SerializeField] float offsetY = 1.5f;
    [SerializeField] float eyeY = 0.5f;
    [SerializeField, Range(0, 1.0f)] float posSpeed = 0.25f;

    Transform mainCamera;
    Transform transform_x;
    Transform transform_y;
    Transform player;
    Transform audioListener;

    /// <summary>
    /// 値変更　Debug用
    /// </summary>
    [SerializeField, Tooltip("値変更　Debug用")] bool initDone;

    float _speed = 0f;

    int layerMask = 0;
    Vector3 boxSize = new Vector3(0.2f, 0.2f, 0.2f);

    void Awake()
    {
        GM.Add("player.loaded", this);
        GM.Add("camera.settings", this);
        layerMask = LayerMask.GetMask("Map", "Water");
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if (data1 == "camera.settings")
        {
            speed = db.cameraSpeed;
            return;
        }

        player = (Transform)data2[0];
        mainCamera = Camera.main.transform;

        transform_y = transform.GetChild(0);
        transform_x = transform.GetChild(0).GetChild(0);
        audioListener = transform.GetChild(1);

        var pos = mainCamera.localPosition;
        pos.z = -length;
        mainCamera.localPosition = pos;

        transform.position = player.position;
        pos = player.position;
        p1 = pos;
        initDone = true;
    }

    Vector3 p1;
    Vector3 pos, cpos;
    float x = 0, y = 0;
    float xs = 0, ys = 0;
    RaycastHit hit;
    float ln = 3, lns = 3;
    float scroll;
    float rot_x, rot_y;

    void Update()
    {
        if (!initDone) return;

        //----------------壁すり抜け回避----------------
        WallSlipPrevention();

        //----------------カメラの距離----------------
        CameraDistance();

        //----------------カメラ切り替え----------------
        ChangeCamera();
    }

    void FixedUpdate()
    {
        if (!initDone) return;
        _speed = Time.deltaTime * 60f * speed;

        //----------------座標の更新----------------
        UpdatePosition();

        //----------------カメラ回転----------------
        RotateCamera();
    }

    /// <summary>
    /// 壁すり抜け回避
    /// </summary>
    void WallSlipPrevention()
    {
        if (Physics.CheckSphere(transform.position, 0.35f, layerMask))
        {
            ln = -0.1f;
            lns = ln;
        }
        else if (Physics.SphereCast(transform.position, 0.35f, -mainCamera.forward, out hit, maxLength, layerMask))
        {
            ln = hit.distance - 0.1f;
            if (ln > length) ln = length;
            lns = ln;
        }
        else
        {
            ln = length;
            lns = lns.Move(ln, inputSpeed);
        }

        //lns = ln;

        cpos = mainCamera.localPosition;
        cpos.z = -lns;
        mainCamera.localPosition = cpos;
    }

    void OnDrawGizmos()
    {
        if (mainCamera == null)
            mainCamera = Camera.main.transform;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.DrawSphere(mainCamera.position, 0.25f);
    }

    /// <summary>
    /// 座標の更新
    /// </summary>
    void UpdatePosition()
    {
        if (player == null) return;
        pos = player.position;

        pos.y += offsetY;
        p1 = p1.Move(pos, posSpeed);

        transform.position = p1;
        audioListener.position = p1;
        audioListener.rotation = mainCamera.rotation;
    }

    /// <summary>
    /// カメラの回転
    /// </summary>
    void RotateCamera()
    {
        if (GM.pause != ePause.mode.UIStop)
        {
            x = InputF.GetAxis(eInputMap.data.CameraX) * _speed;
            y = InputF.GetAxis(eInputMap.data.CameraY) * _speed;
        }
        else
        {
            x = InputF.GetAxis(eInputMap.data.CameraX2) * _speed;
            y = InputF.GetAxis(eInputMap.data.CameraY2) * _speed;
        }        

        xs = xs.Move(x, inputSpeed);
        ys = ys.Move(y, inputSpeed);

        //0～360 -> -180 ～ 180 
        rot_x = (transform_x.localRotation.eulerAngles.x > 180f) ?
            transform_x.localRotation.eulerAngles.x - 360 : transform_x.localRotation.eulerAngles.x;
        rot_y = transform_y.localRotation.eulerAngles.y;
        rot_x -= xs;
        rot_y += ys;
        rot_x = Mathf.Clamp(rot_x, -limitAngle, limitAngle);
        //-180 ～ 180 -> 0～360
        rot_x = (rot_x < 0) ?
            rot_x + 360 : rot_x;

        transform_x.localRotation = Quaternion.Euler(rot_x, 0, 0);
        transform_y.localRotation = Quaternion.Euler(0, rot_y, 0);
    }

    /// <summary>
    /// カメラの距離
    /// </summary>
    void CameraDistance()
    {
        if (GM.pause == ePause.mode.UIStop) return;
        scroll = InputF.GetAxis(eInputMap.data.Scroll) * _speed;
        length -= scroll;
        length = Mathf.Clamp(length, minLength, maxLength);
    }

    /// <summary>
    /// 視点切り替え
    /// </summary>
    void ChangeCamera()
    {
        if (!InputF.GetButtonDown(eInputMap.data.ChangeCamera)) return;

        cameraType++;

        if (cameraType >= cameraLength.Length)
        {
            cameraType = 0;
        }

        length = cameraLength[cameraType];
    }
}
