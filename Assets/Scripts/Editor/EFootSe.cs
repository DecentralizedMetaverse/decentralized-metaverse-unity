using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(DBFootSe))]
public class EFootSe : Editor
{
    ReorderableList reorderableList;
    private void OnEnable()
    {
        var _list = (DBFootSe)target;
        if (reorderableList == null)
        {
            //リストの設定と生成
            reorderableList = new ReorderableList
            (
                elements: _list.data,
                elementType: typeof(DBFootSeElm),
                draggable: true,
                displayHeader: true,
                displayAddButton: true,
                displayRemoveButton: true
            );
        }
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 3;//高さを設定
        reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "足音データベース");
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            var data = _list.data[index];
            rect.height = EditorGUIUtility.singleLineHeight;//高さを設定
            EditorGUI.LabelField(rect, "ID " + index + "　　ファイル名");
            data.fileName = EditorGUI.TextField(rect, " ", data.fileName);
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            data.seName = EditorGUI.TextField(rect, "種類", data.seName);
        };
    }
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();//変更のチェック
        //base.OnInspectorGUI();  //元のInspector部分を表示
        /*表示*/
        //値の更新ができるようにする
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        //変更された値を保存
        //if (DrawDefaultInspector())
        if (EditorGUI.EndChangeCheck())
        {
            Debug.Log("Changed");
            EditorUtility.SetDirty(target);
        }
    }
}
