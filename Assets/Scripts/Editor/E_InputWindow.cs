using UnityEngine;
using UnityEditor;

public class E_InputWindow : EditorWindow
{
    DB_Input dbInput;
    Vector2 scrollView;

    [MenuItem("Tools/InputManager")]
    public static void Open()
    {
        GetWindow<E_InputWindow>("InputManager");
    }
    private void OnEnable()
    {
        //データベース読み込み
        if (dbInput != null) return;
        dbInput = Resources.Load("DB/DB_Input") as DB_Input;
    }
    private void OnGUI()
    {
        var gui = Editor.CreateEditor(dbInput);
        if (gui == null) return;
        scrollView = GUILayout.BeginScrollView(scrollView);
        gui.OnInspectorGUI();
        GUILayout.EndScrollView();
    }
}
