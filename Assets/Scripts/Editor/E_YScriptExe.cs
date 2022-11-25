using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
/// <summary>
/// Path
/// </summary>
[CustomEditor(typeof(YScriptExe))]
public class E_YScriptExe : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();  //元のInspector部分を表示

        //変更のチェック
        EditorGUI.BeginChangeCheck();

        /*表示*/
        //オブジェクトの更新
        serializedObject.Update();
        var data = serializedObject.FindProperty("data");
        var tg = (YScriptExe)target;
        if (tg.data == null)
        {
            //ボタン
            if (GUILayout.Button("スクリプト生成"))
            {
                var obj = ScriptableObject.CreateInstance<DB_YScript>();
                var dir = "Assets/DB/Function";
                var path = dir + "/" + "ev.asset";
                var uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
                AssetDatabase.CreateAsset(obj, uniquePath);
                AssetDatabase.SaveAssets();
                Selection.activeObject = obj;
                tg.data = obj;
            }
        }
        else
        {
            tg.isOpen = EditorGUILayout.Foldout(tg.isOpen, "編集する");
            if (!tg.isOpen) return;
            var editor = CreateEditor(data.objectReferenceValue);
            editor.OnInspectorGUI();
        }

        //プロパティの変更を更新
        serializedObject.ApplyModifiedProperties();
        //変更された値を保存
        if (EditorGUI.EndChangeCheck())
        {
            //変更があった場合
            EditorUtility.SetDirty(target);
        }
    }
}
