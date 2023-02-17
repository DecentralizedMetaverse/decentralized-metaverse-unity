using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] 
public class EventObject2D : EventObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type != eYScript.exeType.接触実行) return;

        if(collision.tag == "Trigger")
        {
            //script.Exe();
        }
    }
}
