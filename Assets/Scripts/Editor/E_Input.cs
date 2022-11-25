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
        //変更のチェック
        EditorGUI.BeginChangeCheck();

        //内部キャッシュから値をロード
        serializedObject.Update();

        //GUI初期シーン
        var list = (DB_Input)target;
        if (GUILayout.Button("更新"))
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

        //内部キャッシュに値を保存する
        serializedObject.ApplyModifiedProperties(); //データの更新

        if (EditorGUI.EndChangeCheck())
        {
            //変更があった場合
            EditorUtility.SetDirty(target);
        }
    }
}
