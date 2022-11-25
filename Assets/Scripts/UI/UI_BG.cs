using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// �w�i�摜�\��
/// bg {�w�i�ԍ�(0�͉����\�����Ȃ�)}:{�؂�ւ��̑���}:{�I���܂ŃX�N���v�g��~���邩}
/// 1�Ԃ̔w�i�ɐ؂�ւ�  bg 1(:60:true)
/// bg 1
/// </summary>
public class UI_BG : MonoBehaviour, GM_Msg
{
    public DB_BG db = default;
    public UI_AnimFade ui1 = default;
    public UI_AnimFade ui2 = default;
    public Image bg1 = default; //�w�� ���C���ŕ\��
    public Image bg2 = default; //�O�� �N���X�t�F�[�h�Ȃǐ؂�ւ��p
    /// <summary>
    /// ���ݔw�i�Ƃ��Ďg�p���̔ԍ�
    /// </summary>
    byte n = 0;

    private void Awake()
    {
        if (db == null || ui1 == null || ui2 == null) 
            Debug.LogError(this + " �f�[�^������܂���");

        GM.Add("bg", this);
        ui1.Finished = Finish1;
        ui2.Finished = Finish;
    }

    bool n3 = true; // true: �I���܂�Yscript���~�߂�
    void GM_Msg.Receive(string data1, params object[] data2)
    {
        // 1: bg number
        // 2: change_speed
        // 3: wait script(bool)
        byte n1 = byte.Parse(data2[0].ToString());
        byte n2 = 0;
        n3 = true;

        if (data2.Length > 1)
        {
            n2 = byte.Parse(data2[1].ToString());
            ui1.speed = n2;
            ui2.speed = n2;

            if (data2.Length > 2)
            {
                n3 = bool.Parse(data2[2].ToString());
            }
        }
        else
        {
            ui1.speed = 60;
            ui2.speed = 60;
        }

        //�I���܂ŁA�������~�߂邩�ݒ�
        if (!n3) GM.Msg("YsComplete");

        db.Load(n1);

        if (n1 == 0)
        {
            //����
            ui1.active = false;
            return;
        }
        bg2.sprite = bg1.sprite;
        bg2.color = bg1.color;
        bg1.sprite = db.data[n1].sprite;
        bg1.color = db.data[n1].color;

        if (!ui1.active && !ui2.active)
        {
            //��ԍŏ��ɐ؂�ւ��鎞
            ui1.active = true;
            return;
        }
        //�N���X�t�F�[�h
        ui2.group.alpha = 1;
        ui2.active = false;
        n = n1;
    }

    void Finish(bool type)
    {
        if (type) return;
        db.UnLoad(n);
        if (n3) GM.Msg("YsComplete");
    }

    void Finish1(bool type)
    {
        //if (type) return;
        if (n3) GM.Msg("YsComplete");
    }
}
