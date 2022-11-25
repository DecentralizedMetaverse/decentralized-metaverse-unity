using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatusBar : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image imageSub;
    [SerializeField] float speed = 0.5f;
    float rate = 1f;
    int _value = 0, _max = 0;
    
    float t = 0f;
    float waitTime = 0.5f;

    public int max
    {
        get { return _max; }
        set { rate = 1f / value; _max = value; }
    }

    public virtual int value
    {
        get { return _value; }
        set
        {
            /*if (value < _value)*/ image.fillAmount = value * rate;
            _value = value;
            t = waitTime;
        }
    }

    protected virtual void Update()
    {
        if (t < -50f) return;
        if (t > 0f)
        {
            t -= Time.deltaTime;
            return;
        }

        if (imageSub.fillAmount > image.fillAmount) imageSub.fillAmount -= Time.deltaTime * speed;
        else
        {
            imageSub.fillAmount = image.fillAmount;
            t = -100f;
        }
    }
}
