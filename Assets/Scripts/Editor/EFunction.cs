using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
[CustomEditor(typeof(DBFunction))]
public class EFunction : Editor
{
    ReorderableList reorderableList;
    private void OnEnable()
    {
        var _list = (DBFunction)target;
        if (reorderableList == null)
        {
            //リストの設定と生成
            reorderableList = new ReorderableList
            (
                elements: _list.data,
                elementType: typeof(DBFunctionElm),
                draggable: true,
                displayHeader: true,
                displayAddButton: true,
                displayRemoveButton: true
            );
        }
        //reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 2;//高さを設定
        //要素ごとに高さを設定
        reorderableList.elementHeightCallback = (index) =>
        {
            if (_list.data[index].isOpen)
            {
                return (EditorGUIUtility.singleLineHeight) * (_list.data[index].data.LineCount() + 3);
            }
            else return EditorGUIUtility.singleLineHeight * 3;
        };
        reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "スクリプト関数リスト");
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            var data = _list.data[index];
            rect.height = EditorGUIUtility.singleLineHeight;//高さを設定
            data.name = EditorGUI.TextField(rect, "関数名", data.name);
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            data.isOpen = EditorGUI.Foldout(rect, data.isOpen, "スクリプト▼");
            if (data.isOpen)
            {
                rect.y += EditorGUIUtility.singleLineHeight + 2;
                rect.height = EditorGUIUtility.singleLineHeight * data.data.LineCount();
                data.data = EditorGUI.TextArea(rect, data.data);
            }
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
