using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using NUnit.Framework;

[CustomEditor(typeof(DB_Input))]
public class E_Input : Editor
{
    public override void OnInspectorGUI()
    {
        //�ύX�̃`�F�b�N
        EditorGUI.BeginChangeCheck();

        //�����L���b�V������l�����[�h
        serializedObject.Update();

        //GUI�����V�[��
        var list = (DB_Input)target;
        if (GUILayout.Button("�X�V"))
        {
            List<string> str = new List<string>();
            for (var i = 0; i < list.data.Count; i++)
            {
                str.Add(list.data[i].name);
            }
            EnumManager.Create("data", "Assets/Scripts/Core/Enum/eInputMap.cs", str);
            AssetDatabase.Refresh();
        }
        //base.OnInspectorGUI();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("data"), true);

        //�����L���b�V���ɒl��ۑ�����
        serializedObject.ApplyModifiedProperties(); //�f�[�^�̍X�V

        if (EditorGUI.EndChangeCheck())
        {
            //�ύX���������ꍇ
            EditorUtility.SetDirty(target);
        }
    }
}
