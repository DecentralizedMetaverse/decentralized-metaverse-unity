using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))] 
public class EventObject3D : EventObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (type != eYScript.exeType.接触実行) return;

        if(other.tag == "Trigger")
        {
            //script.Exe();
        }
    }
}
