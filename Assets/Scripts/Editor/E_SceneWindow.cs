using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class E_SceneWindow : EditorWindow
{
    //シーンデータベース
    public DB_Scene dbScene;
    Vector2 scrollView;

    [MenuItem("Tools/SceneManager")]
    public static void Open()
    {
        GetWindow<E_SceneWindow>("SceneManager");
    }
    private void OnEnable()
    {
        //データベース読み込み
        if (dbScene != null) return;
        dbScene = Resources.Load("DB/DB_Scene") as DB_Scene;
    }
    private void OnGUI()
    {
        var gui = Editor.CreateEditor(dbScene);
        if (gui == null) return;
        scrollView = GUILayout.BeginScrollView(scrollView);
        gui.OnInspectorGUI();
        GUILayout.EndScrollView();
    }
}
