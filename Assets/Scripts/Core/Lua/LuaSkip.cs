using System.Collections;
using System.Collections.Generic;
using TC;
using UnityEngine;

public class LuaSkip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GM.Add("SkipStart", SetSkipStart);
        GM.Add("SkipEnd", SetSkipEnd);
    }

    void SetSkipStart()
    {

    }

    void SetSkipEnd()
    {

    }
}
