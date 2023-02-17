using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace TC
{
    /// <summary>
    /// Lua�t�@�C����ǂݍ��߂�悤�ɂ���
    /// </summary>
    [ScriptedImporter(1, "lua")]
    public class LuaImporter : ScriptedImporter
    {
        /// <summary>
        /// Lua�t�@�C����ۑ�����ꏊ
        /// </summary>
        const string dirName = "Assets/LuaScripts";
        public override void OnImportAsset(AssetImportContext ctx)
        {
            TextAsset data = new TextAsset(File.ReadAllText(ctx.assetPath));

            ctx.AddObjectToAsset("LuaScript", data);
            ctx.SetMainObject(data);
        }

        /// <summary>
        /// LuaScript�t�@�C�����쐬���郁�j���[
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