using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using NUnit.Framework;

[CustomEditor(typeof(DB_Scene))]
public class E_Scene : Editor
{    
    public override void OnInspectorGUI()
    {
        //変更のチェック
        EditorGUI.BeginChangeCheck();

        //内部キャッシュから値をロード
        serializedObject.Update();

        //GUI初期シーン
        EditorGUILayout.PropertyField(serializedObject.FindProperty("offlineMode"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skipSignInScreen"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startWorldPosition"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bgmOff"), true);
        var list = (DB_Scene)target;
        list.scene = (eScene.Scene)EditorGUILayout.EnumPopup("初期ロードScene", list.scene);
        list.world = (eScene.Scene)EditorGUILayout.EnumPopup("初期ワールドScene", list.world);

        if (GUILayout.Button("更新"))
        {
            //Enum生成
            List<string> str = new List<string>();
            for (var i = 0; i < list.data.Count; i++)
            {
                str.Add(list.data[i].name);
            }
            EnumManager.Create("Scene", "Assets/Scripts/Core/Enum/eScene.cs", str);
            AssetDatabase.Refresh();
        }
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
