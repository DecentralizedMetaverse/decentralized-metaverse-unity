using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Time : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textTime;

    void Start()
    {
        StartCoroutine(UpdateTime());
    }

    IEnumerator UpdateTime()
    {
        while (true)
        {
            textTime.text = $"{DateTime.Now.ToString("HH:mm:ss")}"; //"yyyy/MM/dd HH:mm:ss"
            yield return new WaitForSeconds(1.0f);
        }
    }
}
