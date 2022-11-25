using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(DBBGM))]
public class EBGM : Editor
{
    ReorderableList reorderableList;
    private void OnEnable()
    {
        var _list = (DBBGM)target;
        if (reorderableList == null)
        {
            //リストの設定と生成
            reorderableList = new ReorderableList
            (
                elements: _list.data,
                elementType: typeof(DBBGMElm),
                draggable: true,
                displayHeader: true,
                displayAddButton: true,
                displayRemoveButton: true
            );
        }
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 6;//高さを設定
        reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "BGMデータベース");
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            var data = _list.data[index];
            rect.height = EditorGUIUtility.singleLineHeight;//高さを設定
            EditorGUI.LabelField(rect, "ID " + index+"　　ファイル名");
            data.fileName = EditorGUI.TextField(rect, " ", data.fileName);
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            data.bgmName = EditorGUI.TextField(rect, "曲名", data.bgmName);
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            data.noLoop = EditorGUI.Toggle(rect, "ループなし", data.noLoop);
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            data.loopBeginTime = EditorGUI.FloatField(rect, "始まりの時間", data.loopBeginTime);
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            data.loopEndTime = EditorGUI.FloatField(rect, "終わりの時間", data.loopEndTime);
            //data.loop = EditorGUI.Toggle(rect, "解放", data.loop);
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
