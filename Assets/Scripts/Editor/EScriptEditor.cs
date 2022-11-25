using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DBScript))]
public class EScriptEditor : Editor
{
    ReorderableList reorderableList;
    private void OnEnable()
    {
        var _list = (DBScript)target;
        if (reorderableList == null)
        {
            //reorderableList = new ReorderableList
            //(
            //    elements: _list._list,
            //    elementType: typeof(DBScriptElm),
            //    draggable: true,
            //    displayHeader: true,
            //    displayAddButton: true,
            //    displayRemoveButton: true
            //);
            reorderableList = new ReorderableList(_list.exe2,typeof(DBScriptElm));
        }
        reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "実行条件");
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            var data = _list.exe2[index];
            rect.height -= 2f;
            rect.width = 90f;
            data.type = (DBFlag.type)EditorGUI.EnumPopup(rect, data.type);
            rect.x += rect.width;//右にずらす
            data.exe = (DBFlag.exe2)EditorGUI.EnumPopup(rect, data.exe);
            rect.x += rect.width;//右にずらす
            data.num = (byte)EditorGUI.IntField(rect, data.num);
        };
    }
    public override void OnInspectorGUI()
    {
        /*表示*/
        reorderableList.DoLayoutList();
        base.OnInspectorGUI();  //元のInspector部分を表示

        if (GUILayout.Button("コピー"))
        {
            var db_data = target as DBScript;
            EditorGUIUtility.systemCopyBuffer = db_data.data;
        }
        //変更された値を保存
        EditorUtility.SetDirty(target);
    }
}
