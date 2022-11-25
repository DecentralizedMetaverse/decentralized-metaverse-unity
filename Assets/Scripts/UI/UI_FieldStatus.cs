using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FieldStatus : MonoBehaviour
{
    [SerializeField] DB_Player db;
    [SerializeField] UI_StatusBar hp;
    [SerializeField] UI_StatusBar mp;
    [SerializeField] UI_StatusBar sp;
    [SerializeField] UI_AnimFade[] uI;
    [SerializeField] Animator animStamina;
    float[] time = new float[2];
    bool _menu;
    private void Start()
    {
        db.mp = db.mpMax;
    }
    public bool active
    {
        get { return _menu; }
        set { _menu = value; if (value) SetUI(); }
    }    
    void Update()
    {
        if (db.hpMax != hp.max) { hp.max = db.hpMax; SetUI(); }
        if (db.mpMax != mp.max) { mp.max = (int)db.mpMax; SetUI(1); }
        if (db.spMax != sp.max) { sp.max = db.spMax; }
        if (db.hp != hp.value) { hp.value = db.hp; SetUI(); }
        if (db.mp != mp.value) { mp.value = (int)db.mp; SetUI(1); }
        if (db.sp != sp.value) { sp.value = db.sp; }
        
        if(db.mp < 300) animStamina.SetBool("low", true);
        else animStamina.SetBool("low", false);
        
        if(db.isRecovery) animStamina.SetBool("recovery", true);
        else animStamina.SetBool("recovery", false);

        //メニュー画面表示時は表示されるようにする
        if (_menu) return;

        //HPに変動がない場合は、一定の時間経過後、非表示にする
        for (int i = 0; i < time.Length; i++)
        {
            if (time[i] > 0) { time[i] -= Time.deltaTime; continue; }
            
            if (time[i] < -1f) continue;
            
            uI[i].speed = 1f;
            uI[i].active = false;
            time[i] = -2f;
        }
    }
    void SetUI(int i = 0)
    {
        //時間をリセットし、HPを表示させる
        time[i] = 5f; 
        uI[i].speed = 60;
        uI[i].active = true;
    }    
}
