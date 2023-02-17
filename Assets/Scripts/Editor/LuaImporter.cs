using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace TC
{
    /// <summary>
    /// Luaファイルを読み込めるようにする
    /// </summary>
    [ScriptedImporter(1, "lua")]
    public class LuaImporter : ScriptedImporter
    {
        /// <summary>
        /// Luaファイルを保存する場所
        /// </summary>
        const string dirName = "Assets/LuaScripts";
        public override void OnImportAsset(AssetImportContext ctx)
        {
            TextAsset data = new TextAsset(File.ReadAllText(ctx.assetPath));

            ctx.AddObjectToAsset("LuaScript", data);
            ctx.SetMainObject(data);
        }

        /// <summary>
        /// LuaScriptファイルを作成するメニュー
        /// </summary>
        [MenuItem("Assets/Create/Lua Script", priority = 71)]
        static void Create()
        {
            var path = EditorUtility.SaveFilePanel("Lua", dirName, "", "lua");
            if (path == "") return;
            File.WriteAllText(path, "");
            AssetDatabase.Refresh();
        }
    }
}