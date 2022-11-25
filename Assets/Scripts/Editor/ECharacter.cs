using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(DBCharacter))]
public class ECharacter : Editor
{
    ReorderableList reorderableList;
    private void OnEnable()
    {
        var _list = (DBCharacter)target;
        if (reorderableList == null)
        {
            //リストの設定と生成
            reorderableList = new ReorderableList
            (
                elements: _list.data,
                elementType: typeof(DBCharacterElm),
                draggable: true,
                displayHeader: true,
                displayAddButton: true,
                displayRemoveButton: true
            );
        }
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 3;//高さを設定
        reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "キャラクターデータベース");
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            var data = _list.data[index];
            rect.height = EditorGUIUtility.singleLineHeight;//高さを設定
            EditorGUI.LabelField(rect, "ID " + index+"　　名前");
            data.name = EditorGUI.TextField(rect, " ", data.name);
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            data.stnMax = (byte)EditorGUI.IntField(rect, "立ち絵の数", data.stnMax);
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
