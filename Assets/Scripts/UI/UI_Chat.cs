using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class UI_Chat : MonoBehaviour, GM_Msg
{
    [SerializeField] DB_User dbUser;
    [SerializeField] UI_AnimBase ui;
    [SerializeField] InputField input;
    //チャットログ
    [SerializeField] Transform groupTopScreen;
    [SerializeField] Transform groupLogScreen;
    [SerializeField] GameObject prefabL;
    [SerializeField] GameObject prefabR;

    bool cancel;
    string pathChat = "";
    string pathCommand = "";
    DB_Chat_E chatData;
    List<string> commandLog = new List<string>();
    int select = 0;

    void Awake()
    {
        //チャット履歴とコマンド履歴を読み込み
        pathChat = Application.dataPath + "/chat.dat";
        pathCommand = Application.dataPath + "/command.dat";
        ReadChat();
        ReadCommandLog();
        GM.Add("chat", this);
    }

    void GM_Msg.Receive(string data1, params object[] data2)
    {
        //データの受け取り
        var id = uint.Parse((string)data2[0]);
        var content = (string)data2[1];

        //メッセージ受信
        if (data2[1].ToString().IndexOf("/") == -1)
        {
            //コマンド以外であれば、ログに追加する
            chatData.uid.Add(id);
            chatData.content.Add(content);
            //ログに追加
            AddChatLog(id, content);
        }

        //画面にチャットを表示       
        ShowChatMessage(id, content);
    }

    private void OnDisable()
    {
        WriteChatLog();
        WriteCommandLog();
    }

    void Update()
    {
        if (GM.pause != ePause.mode.GameStop &&
            InputF.GetButtonDown(eInputMap.data.Chat) &&
            !ui.active)
        {
            //チャット画面開始
            OpenChatWindow();
        }

        if (!ui.active) return;
        //表示中の場合

        if (InputF.GetButtonDown(eInputMap.data.Cancel))
        {
            //チャット画面終了
            CloseChatWindow();
            return;
        }
        
        //コマンド履歴
        ShowPreviousCommand();
    }

    /// <summary>
    /// 上下押下時、コマンド履歴を表示する
    /// </summary>
    void ShowPreviousCommand()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //履歴を遡る
            if (commandLog.Count == 0) return;
            select++;
            if (select > commandLog.Count) select = commandLog.Count;
            input.text = commandLog[commandLog.Count - select];
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //履歴を遡る
            select--;
            if (select < 0) select = 0;
            if (select == 0) input.text = "";
            else input.text = commandLog[commandLog.Count - select];
        }
    }

    /// <summary>
    /// チャット画面表示
    /// </summary>
    void OpenChatWindow()
    {
        //チャット開始
        ui.active = true;
        GM.pause = ePause.mode.GameStop;
        StartCoroutine(SelectInputField());//選択
    }

    /// <summary>
    /// チャット画面終了
    /// </summary>
    void CloseChatWindow()
    {
        cancel = true;
        select = 0;
        ui.active = false;
        GM.pause = ePause.mode.none;
    }

    /// <summary>
    /// メッセージ送信
    /// </summary>
    public void Send()
    {
        if (cancel)
        {
            cancel = false;
            return;
        }

        if (input.text == "") return;

        if (!IsCommandDuplicate())
        {
            //コマンドが前回と重複していない場合
            commandLog.Add(input.text);
        }

        select = 0;
        if (input.text[0] == '/')
        {
            //ローカルでコマンド実行
            var command = input.text.Substring(1);
            GM.Msg("message", command);
            CloseChatWindow();
            GM.Msg("Script", command);
        }
        else if (input.text[0] == '+')
        {
            //全クライアントでコマンド実行
            var command = input.text.Substring(1);
            GM.Msg("message", command);
            CloseChatWindow();
            GM.Msg("send", command); //全クライアントに送信
        }
        else GM.Msg("send", "chat", GM.id, input.text); //全クライアントに送信
        input.text = "";
    }

    /// <summary>
    /// 画面上部にメッセージを表示する
    /// </summary>
    /// <param name="id"></param>
    /// <param name="content"></param>
    void ShowChatMessage(uint id, string content)
    {
        GameObject obj1 = Instantiate(prefabR, groupTopScreen);
        var nameText = obj1.transform.GetChild(1).GetChild(0).GetComponent<Text>();//名前
        var contentText = obj1.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>();//内容
        var profile = obj1.transform.GetChild(0).GetChild(0).GetComponent<Image>();//アイコン

        var data = dbUser.GetData(id);
        if (data == null) return;

        nameText.text = data.name;
        contentText.text = content;
        profile.sprite = data.thumbnail;

        //表示時間の設定
        obj1.GetComponent<UI_Destroy>().DestroyByTime(3f);
    }

    /// <summary>
    /// チャット画面に追加
    /// </summary>
    /// <param name="id"></param>
    /// <param name="content"></param>
    void AddChatLog(uint id, string content)
    {
        if (!dbUser.index.ContainsKey(id)) return;

        GameObject obj;
        if (id == GM.id)
        {
            //自分
            obj = Instantiate(prefabR, groupLogScreen);
        }
        else
        {
            //他の人
            obj = Instantiate(prefabL, groupLogScreen);
        }

        var nameText = obj.transform.GetChild(1).GetChild(0).GetComponent<Text>();//名前
        var contentText = obj.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>();//内容
        var profile = obj.transform.GetChild(0).GetChild(0).GetComponent<Image>();//アイコン

        var data = dbUser.GetData(id);

        nameText.text = data.name;
        contentText.text = content;
        profile.sprite = data.thumbnail;
    }

    //UIを選択状態にする
    IEnumerator SelectInputField()
    {
        yield return null;
        input.ActivateInputField();
    }

    /// <summary>
    /// 前のコマンドと重複していないか
    /// </summary>
    /// <returns></returns>
    bool IsCommandDuplicate()
    {
        return commandLog.Count != 0 && commandLog[commandLog.Count - 1] == input.text;
    }

    //-----------------------------------------------

    void ReadChat()
    {
        if (!File.Exists(pathChat))
        {
            chatData = new DB_Chat_E();
            return;
        }

        var data = File.ReadAllText(pathChat, System.Text.Encoding.UTF8);

        //ログに追加
        chatData = JsonUtility.FromJson<DB_Chat_E>(data);
        for (int i = 0; i < chatData.uid.Count; i++)
        {
            AddChatLog(chatData.uid[i], chatData.content[i]);
        }
    }

    void WriteChatLog()
    {
        var text = JsonUtility.ToJson(chatData);
        File.WriteAllText(pathChat, text, System.Text.Encoding.UTF8);
    }

    void ReadCommandLog()
    {
        if (!File.Exists(pathCommand)) return;

        var data = File.ReadAllText(pathCommand, System.Text.Encoding.UTF8);
        commandLog = new List<string>(data.Split('\n'));
    }

    void WriteCommandLog()
    {
        var data = string.Join("\n", commandLog.ToArray());
        File.WriteAllText(pathCommand, data, System.Text.Encoding.UTF8);
    }
    //-----------------------------------------------
}

[System.Serializable]
public class DB_Chat_E
{
    public List<uint> uid = new List<uint>();
    public List<string> content = new List<string>();
}