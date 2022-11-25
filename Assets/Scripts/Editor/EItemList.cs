using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(DBItem))]
public class EItemList : Editor
{
    ReorderableList reorderableList;
    private void OnEnable()
    {
        //SerializedProperty sp = serializedObject.FindProperty("list");
        var _list = (DBItem)target;
        if (reorderableList == null)
        {
            //リストの設定と生成
            reorderableList = new ReorderableList
            (
                elements: _list.list,
                elementType: typeof(DBItemElm),
                draggable: true,
                displayHeader: true,
                displayAddButton: true,
                displayRemoveButton: true
            );
        }
        //reorderableList = new ReorderableList(serializedObject, sp);
        //要素ごとに高さを設定
        reorderableList.elementHeightCallback = (index) =>
        {
            if (_list.list[index].isOpen)
            {
                return EditorGUIUtility.singleLineHeight * 13;
            }
            else return EditorGUIUtility.singleLineHeight * 4;
        };
        reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "アイテムデータベース");
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            //SerializedProperty element = sp.GetArrayElementAtIndex(index);
            //EditorGUI.LabelField(rect,index.ToString());
            //rect.x += 30;
            //rect.y += EditorGUIUtility.singleLineHeight;
            //EditorGUI.PropertyField(rect, element);
            //rect.height -= 4;
            //rect.y += 8;

            //要素を書き換えられるようにフィールドを表示
            //rect.x += 20;
            var data = _list.list[index];
            rect.height = EditorGUIUtility.singleLineHeight;//高さを設定
            EditorGUI.LabelField(rect, "ID " + index+"\t\t名前");
            data.itemName = EditorGUI.TextField(rect, "  ", data.itemName);
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            data.objName = EditorGUI.TextField(rect, "オブジェクト", data.objName);
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            data.isOpen = EditorGUI.Foldout(rect, data.isOpen, "詳細");
            if (data.isOpen)
            {
                rect.y += EditorGUIUtility.singleLineHeight + 2;
                data.icon = (Sprite)EditorGUI.ObjectField(rect, "アイコン", data.icon, typeof(Sprite), false);
                if (_list.category == DBItem.type.crops)
                {
                    rect.y += EditorGUIUtility.singleLineHeight + 2;
                    data.season = (DBItem.season)EditorGUI.EnumPopup(rect, "季節", data.season);
                    rect.y += EditorGUIUtility.singleLineHeight + 2;
                    data.harvestTime = (byte)EditorGUI.IntField(rect, "栽培期間", data.harvestTime);
                }
                else if 
                (   _list.category == DBItem.type.cooking || 
                    _list.category == DBItem.type.medicine ||
                    _list.category == DBItem.type.equipment
                )
                {
                    rect.y += EditorGUIUtility.singleLineHeight + 2;
                    data.value = EditorGUI.IntSlider(rect, "値", data.value, 0, fg.valueMax);
                }
                if(_list.category == DBItem.type.equipment)
                {
                    rect.y += EditorGUIUtility.singleLineHeight + 2;
                    data.equip = (DBItem.equip)EditorGUI.EnumPopup(rect, "種類", data.equip);
                }
                rect.y += EditorGUIUtility.singleLineHeight + 2;
                data.buy = EditorGUI.IntSlider(rect, "買値", data.buy, 0, fg.moneyMax);
                rect.y += EditorGUIUtility.singleLineHeight + 2;
                data.sell = EditorGUI.IntSlider(rect, "売値", data.sell, 0, fg.moneyMax);
                rect.y += EditorGUIUtility.singleLineHeight + 2;
                EditorGUI.LabelField(rect, "説明");
                rect.y += EditorGUIUtility.singleLineHeight + 2;
                rect.height = EditorGUIUtility.singleLineHeight * 2;//高さを設定
                data.description = EditorGUI.TextArea(rect, data.description);
            }
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
#endif
//#if UNITY_EDITOR
//[CustomPropertyDrawer(typeof(DBItemElm))]
//public class Drawer : PropertyDrawer
//{
//    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
//    {
//        using (new EditorGUI.PropertyScope(rect, label, property))
//        {
//            EditorGUIUtility.labelWidth = 100;
//            rect.height = EditorGUIUtility.singleLineHeight;
//            //各プロパティーの SerializedProperty を求める
//            SerializedProperty panelProperty0 = property.FindPropertyRelative("itemName");
//            SerializedProperty panelProperty1 = property.FindPropertyRelative("sprite");
//            SerializedProperty panelProperty2 = property.FindPropertyRelative("description");
//            SerializedProperty panelProperty3 = property.FindPropertyRelative("type");
//            SerializedProperty panelProperty4 = property.FindPropertyRelative("value");
//            SerializedProperty panelProperty5 = property.FindPropertyRelative("buy");
//            SerializedProperty panelProperty6 = property.FindPropertyRelative("sell");

//            //各プロパティーの GUI を描画
//            panelProperty0.stringValue = EditorGUI.TextField(rect, "名前", panelProperty0.stringValue);
//            rect.y += EditorGUIUtility.singleLineHeight+3;
//            panelProperty1.intValue = EditorGUI.IntField(rect, "アイコン", panelProperty1.intValue);
//            rect.y += EditorGUIUtility.singleLineHeight+3;
//            panelProperty2.stringValue = EditorGUI.TextField(rect, "説明", panelProperty2.stringValue);
//            rect.y += EditorGUIUtility.singleLineHeight + 3;
//            panelProperty3.enumValueIndex = (int)(DBItem.type)EditorGUI.EnumPopup(rect, "種類", (DBItem.type)System.Enum.GetValues(typeof(DBItem.type)).GetValue(panelProperty3.enumValueIndex));
//            rect.y += EditorGUIUtility.singleLineHeight + 3;
//            panelProperty4.intValue = EditorGUI.IntField(rect, "値", panelProperty4.intValue);
//            rect.y += EditorGUIUtility.singleLineHeight + 3;
//            panelProperty5.intValue = EditorGUI.IntField(rect, "買値", panelProperty5.intValue);
//            rect.y += EditorGUIUtility.singleLineHeight + 3;
//            panelProperty6.intValue = EditorGUI.IntField(rect, "売値", panelProperty6.intValue);
//        }
//    }
//}
//#endif