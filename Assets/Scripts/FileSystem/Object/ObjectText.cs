using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectText : ObjectBase
{
    [SerializeField] Text text;

    public void SetData(string fileName, string text)
    {
        this.fileName = fileName;
        this.text.text = text;
    }
}
