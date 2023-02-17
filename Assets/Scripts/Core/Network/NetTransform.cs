using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetTransform : MonoBehaviour, GM_Msg
{
    Vector3 _position;
    Quaternion _rotation;

    public void SetID(string id)
    {
        //GM.Add(id, this);
    }

    void Update()
    {
        if(transform.position != _position)
        {
            _position = transform.position;
        }

        if(transform.rotation != _rotation)
        {
            _rotation = transform.rotation;
        }
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        var keyward = data2[0].ToString();
        if(keyward == "position")
        {
            transform.position = (Vector3)data2[1];
            _position = transform.position;
        }
        else
        {
            transform.rotation = Quaternion.Euler((Vector3)data2[1]);
            _rotation = transform.rotation;
        }
    }
}
