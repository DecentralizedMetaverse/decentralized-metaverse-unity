using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(DBMap))]
public class EMap : Editor
{
    ReorderableList reorderableList;
    private void OnEnable()
    {
        var _list = (DBMap)target;
        if (reorderableList == null)
        {
            //リストの設定と生成
            reorderableList = new ReorderableList
            (
                elements: _list.data,
                elementType: typeof(DBMapElm),
                draggable: true,
                displayHeader: true,
                displayAddButton: true,
                displayRemoveButton: true
            );
        }
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 5;//高さを設定
        reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "マップデータベース");
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            var data = _list.data[index];
            rect.height = EditorGUIUtility.singleLineHeight;//高さを設定
            EditorGUI.LabelField(rect, "ID " + index);
            data.sceneName = EditorGUI.TextField(rect, " ", data.sceneName);
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            data.stageName = EditorGUI.TextField(rect, "マップ名", data.stageName);
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            data.bgm = (sbyte)EditorGUI.IntField(rect, "再生BGM", data.bgm);
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            data.active = EditorGUI.Toggle(rect, "行ける", data.active);
        };
    }
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();  //元のInspector部分を表示
        /*表示*/
        //値の更新ができるようにする
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        //変更された値を保存
        EditorUtility.SetDirty(target);
    }
}
