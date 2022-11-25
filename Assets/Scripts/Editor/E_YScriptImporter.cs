using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System.IO;

[ScriptedImporter(1, "ys")]
public class E_YScriptImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        TextAsset data = new TextAsset(File.ReadAllText(ctx.assetPath));

        ctx.AddObjectToAsset("yscript", data);
        ctx.SetMainObject(data);
    }
    [MenuItem("Assets/Create/Yscript", priority = 71)]
    static void Create()
    {
        var path = EditorUtility.SaveFilePanel("Yscript", "Assets/Resources/DB/functions", "", "ys");
        if (path == "") return;
        File.WriteAllText(path, "");
        //File.Open(path, FileMode.CreateNew);
        //読み込み
        AssetDatabase.Refresh();
    }
}