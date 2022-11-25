#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.IO;

/*
 * 参考文献：!args 様 「【Unity】スクリプトからInputManagerを自動生産する」
 */

[InitializeOnLoad]  //エディタ起動時のみ実行
/// <summary>
/// InputManagerの設定を自動で行うクラス
/// </summary>
public class AutoSetInputManager : MonoBehaviour {

    public int maxPlayerAmount = 4;

    static string[] deviceNames = 
        {
        "DS4",
        "DS3",
        "NSP",
        "JoyConR",
        "JoyConL",
        "XBOX360",
        "XBOXOne"
        };
    static int NumOfKey { get { return deviceNames.Length * 16; } }

    /// <summary>
    /// インプットマネージャを再設定する
    /// </summary>
	[MenuItem("Tools/Gamepad Setting/Reset InputManager")]
    public static void ResetInputManger()
    {
        bool isOk = EditorUtility.DisplayDialog("InputManagerの再設定を行います", "現在のInputManagerの設定は消去されます。\n残しておきたい場合はキャンセルを押して保存してからもう一度実行してください。", "OK", "キャンセル");

        if (!isOk)
        {
            return;
        }
        
        Debug.Log("インプットマネージャの設定を開始します。");
        SetupInputManager setupInputManager = new SetupInputManager();

        Debug.Log("設定をすべてクリアします。");
        setupInputManager.Clear();
        
        Debug.Log("プレイヤーごとの設定を追加します。");
        float progressRate;
        for (int j = 0; j < deviceNames.Length; j++) //コントローラ数で調整1のところをあとでdevicename.lengthにする
        {
            for (int i = 0; i < 16; i++)
            {
                progressRate = (j * 16 + i) / (float)NumOfKey;

                // プログレスバーを表示
                EditorUtility.DisplayProgressBar("Setting GamePad Key...", $"progress {j * 16 + i} / {NumOfKey} ({progressRate * 100:0.0}%)   {deviceNames[j]}の設定中...", progressRate);


                AddPlayerInputSettings(setupInputManager, i, deviceNames[j]);

            }
        }

        progressRate = 1;

        // プログレスバーを表示
        EditorUtility.DisplayProgressBar("Setting GamePad Key...", $"progress {NumOfKey} / {NumOfKey} ({progressRate * 100:0.0}%)   Keyboardの設定中...", progressRate);

        Debug.Log("グローバル設定を追加します。");
        AddGlobalInputSettings(setupInputManager);

        // プログレスバーを消す
        EditorUtility.ClearProgressBar();

        // 終了したことをポップアップで表示する
        EditorUtility.DisplayDialog("InputManager再設定完了", "インプットマネージャの設定が完了しました。\n保存する場合は「ProjectSettings」→「InputManager」タブより保存してください。", "OK");

    }

    private static void AddGlobalInputSettings(SetupInputManager setupInputManager)
    {
        //横方向
        {
            var name = "HorizontalL";
            setupInputManager.AddAxis(InputAxis.CreatePadAxis(name, 0, 1, false));
            setupInputManager.AddAxis(InputAxis.CreateKeyAxis(name, "d", "a", "", ""));
        }

        //縦方向
        {
            var name = "VerticalL";
            setupInputManager.AddAxis(InputAxis.CreatePadAxis(name, 0, 2, true));
            setupInputManager.AddAxis(InputAxis.CreateKeyAxis(name, "w", "s", "", ""));
        }

        //横方向
        {
            var name = "HorizontalR";
            setupInputManager.AddAxis(InputAxis.CreatePadAxis(name, 0, 6, false));
            setupInputManager.AddAxis(InputAxis.CreateKeyAxis(name, "", "", "right", "left"));
        }

        //縦方向
        {
            var name = "VerticalR";
            setupInputManager.AddAxis(InputAxis.CreatePadAxis(name, 0, 7, false));
            setupInputManager.AddAxis(InputAxis.CreateKeyAxis(name, "", "", "up", "down"));
        }

        //決定
        {
            var name = "Submit";
            setupInputManager.AddAxis(InputAxis.CreatePadAxis(name, 0, 1, false));
            setupInputManager.AddAxis(InputAxis.CreateKeyAxis(name, "return", "", "joystick button 0", ""));
        }
        //キャンセル
        {
            var name = "Cancel";
            setupInputManager.AddAxis(InputAxis.CreatePadAxis(name, 0, 1, false));
            setupInputManager.AddAxis(InputAxis.CreateKeyAxis(name, "escape", "", "joystick button 1", ""));
        }
        // マウス
        {
            var name = "Mouse X";
            setupInputManager.AddAxis(InputAxis.CreateMouseAxis(name, 0, 1, false));
        }
        {
            var name = "Mouse Y";
            setupInputManager.AddAxis(InputAxis.CreateMouseAxis(name, 0, 2, false));
        }
        {
            var name = "Mouse ScrollWheel";
            setupInputManager.AddAxis(InputAxis.CreateMouseAxis(name, 0, 3, false));
        }
    }

    /// <summary>
    /// プレイヤーごとの入力設定を追加する
    /// </summary>
    /// <param name="setupInputManager"></param>
    /// <param name="playerIndex"></param>
    private static void AddPlayerInputSettings(SetupInputManager setupInputManager, int playerIndex, string deviceName)
    {

        if (playerIndex < 0 || playerIndex > 16-1)
        {
            Debug.Log("プレイヤーインデックスの値が不正です。");
        }

        int lStickH, lStickV, rStickH, rStickV, ArrowKeyH, ArrowKeyV;
        string aButton, bButton, xButton, yButton,
        lButton, rButton, ltButton, rtButton, startButton, selectButton;
        bool invertH, invertV;

        //デバイスに対応したキーを取得
        GetDeviceKey(out lStickH, out lStickV, out rStickH, out rStickV,
            out ArrowKeyH, out ArrowKeyV,
            out aButton, out bButton, out xButton, out yButton,
            out lButton, out rButton, out ltButton, out rtButton,
            out startButton, out selectButton, out invertH, out invertV, deviceName, playerIndex+1);

        //ジョイスティック番号を0～3→1～4に変更
        int joystickNum = playerIndex + 1;

        // Lスティック左右
        {
            string name = deviceName + playerIndex + "LStickH";
            setupInputManager.AddAxis(InputAxis.CreatePadAxis(name, joystickNum, lStickH, invertH));
        }
        // Lスティック上下
        {
            string name = deviceName + playerIndex + "LStickV";
            setupInputManager.AddAxis(InputAxis.CreatePadAxis(name, joystickNum, lStickV, invertV));
        }
        // Lスティック押し込み
        {

        }

        // Rスティック左右
        {
            string name = deviceName + playerIndex + "RStickH";
            setupInputManager.AddAxis(InputAxis.CreatePadAxis(name, joystickNum, rStickH, invertH));
        }
        // Rスティック上下
        {
            string name = deviceName + playerIndex + "RStickV";
            setupInputManager.AddAxis(InputAxis.CreatePadAxis(name, joystickNum, rStickV, invertV));
        }
        // Rスティック押し込み
        {

        }

        // 十字キー左右
        {
            string name = deviceName + playerIndex + "CrossKeyH";
            setupInputManager.AddAxis(InputAxis.CreatePadAxis(name, joystickNum, ArrowKeyH, false));
        }
        // 十字キー上下
        {
            string name = deviceName + playerIndex + "CrossKeyV";
            setupInputManager.AddAxis(InputAxis.CreatePadAxis(name, joystickNum, ArrowKeyV, false));
        }

        // Aボタン
        {
            string name = deviceName + playerIndex + "A";
            setupInputManager.AddAxis(InputAxis.CreateButton(name, aButton, "", joystickNum));
        }
        // Bボタン
        {
            string name = deviceName + playerIndex + "B";
            setupInputManager.AddAxis(InputAxis.CreateButton(name, bButton, "", joystickNum));
        }
        // Xボタン
        {
            string name = deviceName + playerIndex + "X";
            setupInputManager.AddAxis(InputAxis.CreateButton(name, xButton, "", joystickNum));
        }
        // Yボタン
        {
            string name = deviceName + playerIndex + "Y";
            setupInputManager.AddAxis(InputAxis.CreateButton(name, yButton, "", joystickNum));
        }

        // Lボタン
        {
            string name = deviceName + playerIndex + "L";
            setupInputManager.AddAxis(InputAxis.CreateButton(name, lButton, "", joystickNum));
        }
        // Rボタン
        {
            string name = deviceName + playerIndex + "R";
            setupInputManager.AddAxis(InputAxis.CreateButton(name, rButton, "", joystickNum));
        }

        // Lトリガー
        {
            string name = deviceName + playerIndex + "LT";
            setupInputManager.AddAxis(InputAxis.CreateButton(name, ltButton, "", joystickNum));
        }
        // Rトリガー
        {
            string name = deviceName + playerIndex + "RT";
            setupInputManager.AddAxis(InputAxis.CreateButton(name, rtButton, "", joystickNum));
        }

        // STARTボタン
        {
            string name = deviceName + playerIndex + "Start";
            setupInputManager.AddAxis(InputAxis.CreateButton(name, startButton, "", joystickNum));
        }
        // SELECTボタン
        {
            string name = deviceName + playerIndex + "Select";
            setupInputManager.AddAxis(InputAxis.CreateButton(name, selectButton, "", joystickNum));
        }

        // Homeボタン
        {

        }
    }

    /// <summary>
    /// コントローラに応じたキーを設定
    /// (キー配置名はNintendoSwitchProcontroller準拠です)
    /// </summary>
    /// <param name="lStickH"></param>
    /// <param name="lStickV"></param>
    /// <param name="rStickH"></param>
    /// <param name="rStickV"></param>
    /// <param name="arrowKeyH"></param>
    /// <param name="arrowKeyV"></param>
    /// <param name="aButton"></param>
    /// <param name="bButton"></param>
    /// <param name="xButton"></param>
    /// <param name="yButton"></param>
    /// <param name="lButton"></param>
    /// <param name="rButton"></param>
    /// <param name="ltButton"></param>
    /// <param name="rtButton"></param>
    /// <param name="startButton"></param>
    /// <param name="selectButton"></param>
    /// <param name="invertH"></param>
    /// <param name="invertV"></param>
    /// <param name="deviceName"></param>
    private static void GetDeviceKey(out int lStickH, out int lStickV, out int rStickH, out int rStickV,
        out int arrowKeyH, out int arrowKeyV,
        out string aButton, out string bButton, out string xButton, out string yButton,
        out string lButton, out string rButton, out string ltButton, out string rtButton,
        out string startButton, out string selectButton, out bool invertH, out bool invertV, string deviceName, int playerIndex)
    {
        

        //コントローラに対応したキーを配置
        switch (deviceName)
        {
            case "DS4": //PS4(DUALSHOCK4)
                //各ボタンの初期化
                lStickH = 1;
                lStickV = 2;
                rStickH = 3;
                rStickV = 6;
                arrowKeyH = 7;
                arrowKeyV = 8;
                aButton = "joystick "+ playerIndex.ToString() + " button 2";
                bButton = "joystick " + playerIndex.ToString() + " button 1";
                xButton = "joystick " + playerIndex.ToString() + " button 3";
                yButton = "joystick " + playerIndex.ToString() + " button 0";
                lButton = "joystick " + playerIndex.ToString() + " button 4";
                rButton = "joystick " + playerIndex.ToString() + " button 5";
                ltButton = "joystick " + playerIndex.ToString() + " button 6";
                rtButton = "joystick " + playerIndex.ToString() + " button 7";
                startButton = "joystick " + playerIndex.ToString() + " button 9";
                selectButton = "joystick " + playerIndex.ToString() + " button 8";
                invertH = false;
                invertV = true;
                break;
            case "DS3": //PS3(DUALSHOCK3)
                //各ボタンの初期化
                lStickH = 1;
                lStickV = 2;
                rStickH = 3;
                rStickV = 5;
                arrowKeyH = 6;
                arrowKeyV = 7;
                aButton = "joystick " + playerIndex.ToString() + " button 13";
                bButton = "joystick " + playerIndex.ToString() + " button 14";
                xButton = "joystick " + playerIndex.ToString() + " button 12";
                yButton = "joystick " + playerIndex.ToString() + " button 15";
                lButton = "joystick " + playerIndex.ToString() + " button 10";
                rButton = "joystick " + playerIndex.ToString() + " button 11";
                ltButton = "joystick " + playerIndex.ToString() + " button 8";
                rtButton = "joystick " + playerIndex.ToString() + " button 9";
                startButton = "joystick " + playerIndex.ToString() + " button 3";
                selectButton = "joystick " + playerIndex.ToString() + " button 0";
                invertH = false;
                invertV = true;
                break;
            case "NSP": //NintendoSwitchProController
                //各ボタンの初期化
                lStickH = 2;
                lStickV = 4;
                rStickH = 7;
                rStickV = 8;
                arrowKeyH = 9;
                arrowKeyV = 10;
                aButton = "joystick " + playerIndex.ToString() + " button 1";
                bButton = "joystick " + playerIndex.ToString() + " button 0";
                xButton = "joystick " + playerIndex.ToString() + " button 3";
                yButton = "joystick " + playerIndex.ToString() + " button 2";
                lButton = "joystick " + playerIndex.ToString() + " button 4";
                rButton = "joystick " + playerIndex.ToString() + " button 5";
                ltButton = "joystick " + playerIndex.ToString() + " button 7";
                rtButton = "joystick " + playerIndex.ToString() + " button 6";
                startButton = "joystick " + playerIndex.ToString() + " button 9";
                selectButton = "joystick " + playerIndex.ToString() + " button 8";
                invertH = false;
                invertV = true;
                break;
            case "JoyConR":  // JoyConR(横持ち)
                lStickH = 9;
                lStickV = 10;
                rStickH = 7;
                rStickV = 8;
                arrowKeyH = 9;
                arrowKeyV = 10;
                aButton = "joystick " + playerIndex.ToString() + " button 1";   // Xボタン
                bButton = "joystick " + playerIndex.ToString() + " button 0";   // Aボタン
                xButton = "joystick " + playerIndex.ToString() + " button 3";   // Yボタン
                yButton = "joystick " + playerIndex.ToString() + " button 2";   // Bボタン
                lButton = "joystick " + playerIndex.ToString() + " button 4";   // SLボタン
                rButton = "joystick " + playerIndex.ToString() + " button 5";   // SRボタン
                ltButton = "joystick " + playerIndex.ToString() + " button 14";   // Rボタン
                rtButton = "joystick " + playerIndex.ToString() + " button 15";   // ZRボタン
                startButton = "joystick " + playerIndex.ToString() + " button 9";    // +ボタン
                selectButton = "joystick " + playerIndex.ToString() + " button 11";  // Rスティック押し込み
                invertH = false;
                invertV = false;
                break;
            case "JoyConL":  // JoyConL(横持ち)
                lStickH = 9;
                lStickV = 10;
                rStickH = 7;
                rStickV = 8;
                arrowKeyH = 9;
                arrowKeyV = 10;
                aButton = "joystick " + playerIndex.ToString() + " button 1";   // ↓ボタン
                bButton = "joystick " + playerIndex.ToString() + " button 0";   // ←ボタン
                xButton = "joystick " + playerIndex.ToString() + " button 3";   // →ボタン
                yButton = "joystick " + playerIndex.ToString() + " button 2";   // ↑ボタン
                lButton = "joystick " + playerIndex.ToString() + " button 4";   // SLボタン
                rButton = "joystick " + playerIndex.ToString() + " button 5";   // SRボタン
                ltButton = "joystick " + playerIndex.ToString() + " button 14";   // Lボタン
                rtButton = "joystick " + playerIndex.ToString() + " button 15";   // ZLボタン
                startButton = "joystick " + playerIndex.ToString() + " button 8";    // -ボタン
                selectButton = "joystick " + playerIndex.ToString() + " button 10";  // Lスティック押し込み
                invertH = true;
                invertV = true;
                break;
            case "XBOX360": //XBOX360
                //各ボタンの初期化
                lStickH = 1;
                lStickV = 2;
                rStickH = 4;
                rStickV = 5;
                arrowKeyH = 6;
                arrowKeyV = 7;
                aButton = "joystick " + playerIndex.ToString() + " button 1";
                bButton = "joystick " + playerIndex.ToString() + " button 0";
                xButton = "joystick " + playerIndex.ToString() + " button 3";
                yButton = "joystick " + playerIndex.ToString() + " button 2";
                lButton = "joystick " + playerIndex.ToString() + " button 4";
                rButton = "joystick " + playerIndex.ToString() + " button 5";
                ltButton = "joystick " + playerIndex.ToString() + " button 4";
                rtButton = "joystick " + playerIndex.ToString() + " button 5";
                startButton = "joystick " + playerIndex.ToString() + " button 7";
                selectButton = "joystick " + playerIndex.ToString() + " button 6";
                invertH = false;
                invertV = true;
                break;
            case "XBOXOne": //XBOXOne
                //各ボタンの初期化
                lStickH = 1;
                lStickV = 2;
                rStickH = 4;
                rStickV = 5;
                arrowKeyH = 6;
                arrowKeyV = 7;
                aButton = "joystick " + playerIndex.ToString() + " button 1";
                bButton = "joystick " + playerIndex.ToString() + " button 0";
                xButton = "joystick " + playerIndex.ToString() + " button 3";
                yButton = "joystick " + playerIndex.ToString() + " button 2";
                lButton = "joystick " + playerIndex.ToString() + " button 4";
                rButton = "joystick " + playerIndex.ToString() + " button 5";
                ltButton = "joystick " + playerIndex.ToString() + " button 9";
                rtButton = "joystick " + playerIndex.ToString() + " button 10";
                startButton = "joystick " + playerIndex.ToString() + " button 7";
                selectButton = "joystick " + playerIndex.ToString() + " button 6";
                invertH = false;
                invertV = true;
                break;
            default: //その他
                //各ボタンの初期化
                lStickH = 0;
                lStickV = 1;
                rStickH = 2;
                rStickV = 3;
                arrowKeyH = 4;
                arrowKeyV = 5;
                aButton = "";
                bButton = "";
                xButton = "";
                yButton = "";
                lButton = "";
                rButton = "";
                ltButton = "";
                rtButton = "";
                startButton = "";
                selectButton = "";
                invertH = false;
                invertV = false;
                break;
        }
    }
}

#endif