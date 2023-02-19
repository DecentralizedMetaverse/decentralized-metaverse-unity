using System.Collections;
using System.Collections.Generic;
using TC;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GM.Add<string>("GenerateObj", Generate);
    }

    void Generate(string fileName)
    {

    }
}
