using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(DBItemGroup))]
public class EItemGroup : Editor
{
    ReorderableList reorderableList;
    private void OnEnable()
    {
        var _list = (DBItemGroup)target;
        if (reorderableList == null)
        {
            //リストの設定と生成
            reorderableList = new ReorderableList
            (
                elements: _list.data,
                elementType: typeof(DBItemGroupElm),
                draggable: true,
                displayHeader: true,
                displayAddButton: true,
                displayRemoveButton: true
            );
        }
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 2;//高さを設定
        reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "アイテムグループデータベース");
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            var data = _list.data[index];
            rect.height = EditorGUIUtility.singleLineHeight;//高さを設定
            EditorGUI.LabelField(rect, "ID " + index+"\t"+ Enum.GetName(typeof(DBItem.type), index));
            data.dbItem = (DBItem)EditorGUI.ObjectField(rect," ",data.dbItem,typeof(DBItem),true);
        };
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();  //元のInspector部分を表示
        /*表示*/
        //値の更新ができるようにする
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        //変更された値を保存
        EditorUtility.SetDirty(target);
    }
}
