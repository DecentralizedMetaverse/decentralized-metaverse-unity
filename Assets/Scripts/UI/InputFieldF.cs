using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldF : InputField
{
    protected override void LateUpdate()
    {
        base.LateUpdate();

        MoveTextEnd(false);
    }
}
