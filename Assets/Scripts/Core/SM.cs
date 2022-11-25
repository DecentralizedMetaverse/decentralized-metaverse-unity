using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SM : MonoBehaviour, GM_Msg
{
    [SerializeField] DB_Scene _dBScene;
    public static DB_Scene dbScene;
    static int previous;
    static eScene.Scene loadScene;

    private void Awake()
    {
        dbScene = _dBScene;
        SceneManager.sceneUnloaded += SceneUnloaded;


        GM.Add("scene", this);
        GM.Add("Completed.LoadScreen", this);

        //ゲーム管理シーン読み込み index:0はロード済みなので飛ばす
        for (var i = 1; i < dbScene.data[0].scene.Length; i++)
        {
            if (dbScene.data[0].scene[i] == "")
                continue;

            SceneManager.LoadScene(/*"Scenes/" + */dbScene.data[0].scene[i], LoadSceneMode.Additive);
        }
        //初期シーン読み込み
        Load(dbScene.scene);
    }

    /// <summary>
    /// シーングループをロード
    /// </summary>
    /// <param name="scene"></param>
    public static void Load(eScene.Scene scene)
    {
        if (scene == 0)
        {
            Debug.LogError("ゲーム管理は読み込まないでください");
            return;
        }
        loadScene = scene;
        GM.Msg("Load", "true");     //ロード画面表示
        //GM.Msg("Save", "write");
    }
    void GM_Msg.Receive(string data1, params object[] data2)
    {
        if (data1 == "scene")
        {
            var dt2 = data2[0].ToString();
            var dt21 = data2[1].ToString();
            if (dt2 == "add")
            {
                SceneManager.LoadScene(dt21, LoadSceneMode.Additive);
            }
            else if (dt2 == "unload")
            {
                SceneManager.UnloadSceneAsync(dt21);
            }
            else if (dt2 == "load")
            {
                SceneManager.LoadScene(dt21);
            }
            return;
        }
        //ロードScene
        for (var i = 0; i < dbScene.data[(int)loadScene].scene.Length; i++)
        {
            var val = dbScene.data[(int)loadScene].scene[i];

            if (val == null || (i < dbScene.data[previous].scene.Length &&
                val == dbScene.data[previous].scene[i]))
                continue;

            Debug.Log("Load: " + val);
            SceneManager.LoadScene(val, LoadSceneMode.Additive);
        }

        //アンロードScene
        if (previous != 0)
        {
            for (var i = 0; i < dbScene.data[(int)previous].scene.Length; i++)
            {
                var val = dbScene.data[(int)previous].scene[i];

                if (val == null || (i < dbScene.data[(int)loadScene].scene.Length &&
                    val == dbScene.data[(int)loadScene].scene[i]))
                    continue;

                Debug.Log("UnLoad: " + val);
                SceneManager.UnloadSceneAsync(val);
            }
        }
        previous = (int)loadScene;
        GM.Msg("Load", "false");    //ロード画面非表示
    }

    void SceneUnloaded(UnityEngine.SceneManagement.Scene thisScene)
    {
        GM.Msg("unloaded", thisScene.name);
    }
}
