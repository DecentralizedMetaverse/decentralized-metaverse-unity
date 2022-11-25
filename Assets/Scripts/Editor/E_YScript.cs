using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using NUnit.Framework;

[CustomEditor(typeof(DB_YScript))]
public class E_YScript : Editor
{
    //ReorderableList reorderableList;
    //private void OnEnable()
    //{
    //    var list = (DB_YScript)target;

    //    //リストの設定と生成
    //    if (reorderableList == null)
    //    {
    //        reorderableList = new ReorderableList
    //        (
    //            elements: list.data,
    //            elementType: typeof(DB_YScriptE),
    //            draggable: true,
    //            displayHeader: true,
    //            displayAddButton: true,
    //            displayRemoveButton: true
    //        );
    //    }

    //    reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "ユーナスクリプト");

    //    reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
    //    {
    //        var data = list.data[index];
    //        //高さを設定
    //        rect.height -= 2f;
    //        rect.width = 90f;
    //        data.type = (eYScript.type)EditorGUI.EnumPopup(rect, data.type);

    //        //要素
    //        rect.x += rect.width;
    //        data.flag = (eYScript.flag)EditorGUI.EnumPopup(rect, data.flag);

    //        //要素
    //        rect.x += rect.width;
    //        data.num = (byte)EditorGUI.IntField(rect, data.num);
    //    };

    //}
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ////変更のチェック
        //EditorGUI.BeginChangeCheck();
        ////表示
        //serializedObject.Update();
        //reorderableList.DoLayoutList();
        //serializedObject.ApplyModifiedProperties(); //データの更新
        if (GUILayout.Button("コピー"))
        {
            var db_data = target as DB_YScript;
            EditorGUIUtility.systemCopyBuffer = db_data.yscript;
        }
        //if (EditorGUI.EndChangeCheck())
        //{
        //    //変更があった場合
        //    EditorUtility.SetDirty(target);
        //}
    }
}
