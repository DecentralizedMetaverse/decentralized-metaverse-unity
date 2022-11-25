#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEditor;

/* 
 * 参考文献：!args 様 「【Unity】スクリプトからInputManagerを自動生産する」
 */

/// <summary>
/// 軸のタイプ
/// </summary>
public enum AxisType
{
    KeyOrMouseButton = 0,
    MouseMovement = 1,
    JoystickAxis = 2
};

/// <summary>
/// 入力の情報
/// </summary>
public class InputAxis
{
    public string name = "";
    public string descriptiveName = "";
    public string descriptiveNegativeName = "";
    public string positiveButton = "";
    public string negativeButton = "";
    public string altPositiveButton = "";
    public string altNegativeButton = "";

    public float gravity = 0;
    public float dead = 0;
    public float sensitivity = 0;

    public bool snap = false;
    public bool invert = false;

    public AxisType axisType = AxisType.KeyOrMouseButton;

    public int axis = 1;
    public int joyNum = 0; //0=すべて,1～=各対応の番号

    /// <summary>
    /// キーの設定データを作成
    /// </summary>
    /// <param name="name">キーの名前</param>
    /// <param name="positiveButton"></param>
    /// <param name="altPositiveButton"></param>
    /// <param name="joystickNum">ジョイスティックの接続番号</param>
    /// <returns></returns>
    public static InputAxis CreateButton(string name, string positiveButton, string altPositiveButton, int joystickNum)
    {
        var axis = new InputAxis();
        axis.name = name;
        axis.positiveButton = positiveButton;
        axis.altPositiveButton = altPositiveButton;
        axis.gravity = 1000;
        axis.dead = 0.001f;
        axis.sensitivity = 1000;
        axis.axisType = AxisType.KeyOrMouseButton;
        axis.joyNum = joystickNum;

        return axis;
    }

    /// <summary>
    /// ゲームパッド用の軸の設定データを作成
    /// </summary>
    /// <param name="name">名前</param>
    /// <param name="joystickNum">ジョイスティックの接続番号</param>
    /// <param name="axisNum">入力軸の番号</param>
    /// <param name="invert">入力軸を反転するかどうか</param>
    /// <returns></returns>
    public static InputAxis CreatePadAxis(string name, int joystickNum, int axisNum, bool invert)
    {
        var axis = new InputAxis();
        axis.name = name;
        axis.dead = 0.1f;
        axis.sensitivity = 1;
        axis.invert = invert;
        axis.axisType = AxisType.JoystickAxis;
        axis.axis = axisNum;
        axis.joyNum = joystickNum;

        return axis;
    }

    /// <summary>
    /// マウス用の軸の設定データを作成
    /// </summary>
    /// <param name="name">名前</param>
    /// <param name="joystickNum">ジョイスティックの接続番号</param>
    /// <param name="axisNum">入力軸の番号</param>
    /// <param name="invert">入力軸を反転するかどうか</param>
    /// <returns></returns>
    public static InputAxis CreateMouseAxis(string name, int joystickNum, int axisNum, bool invert)
    {
        var axis = new InputAxis();
        axis.name = name;
        axis.dead = 0;
        axis.sensitivity = 0.1f;
        axis.invert = invert;
        axis.axisType = AxisType.MouseMovement;
        axis.axis = axisNum;

        return axis;
    }

    /// <summary>
    /// キーボード用の軸の設定データを作成
    /// </summary>
    /// <param name="name">名前</param>
    /// <param name="positiveButton"></param>
    /// <param name="negativeButton"></param>
    /// <param name="altPositiveButton"></param>
    /// <param name="altNegativeButton"></param>
    /// <returns></returns>
    public static InputAxis CreateKeyAxis(string name, string positiveButton, string negativeButton, string altPositiveButton, string altNegativeButton)
    {
        var axis = new InputAxis();
        axis.name = name;
        axis.positiveButton = positiveButton;
        axis.negativeButton = negativeButton;
        axis.altPositiveButton = altPositiveButton;
        axis.altNegativeButton = altNegativeButton;
        axis.gravity = 3;
        axis.dead = 0.001f;
        axis.sensitivity = 3;
        axis.axisType = AxisType.KeyOrMouseButton;

        return axis;
    }
}

/// <summary>
/// InputManagerを設定
/// </summary>
public class SetupInputManager {

    SerializedObject serializedObject;
    SerializedProperty axesProperty;

    /// <summary>
    /// コンストラクタ
    /// InputManagerをシリアライズされたオブジェクトとして読み込む
    /// </summary>
    public SetupInputManager(){
        serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        axesProperty = serializedObject.FindProperty("m_Axes");
    }

    /// <summary>
    /// 軸を追加
    /// </summary>
    /// <param name="axis">軸</param>
    public void AddAxis(InputAxis axis)
    {
        if (axis.axis < 1)
        {
            Debug.LogError("Axisは1以上に設定してください。");
        }
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.arraySize++;
        serializedObject.ApplyModifiedProperties();

        SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

        GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
        GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
        GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
        GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
        GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
        GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
        GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
        GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
        GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
        GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
        GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
        GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
        GetChildProperty(axisProperty, "type").intValue = (int)axis.axisType;
        GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
        GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// 子要素のプロパティを取得
    /// </summary>
    /// <param name="parent">親要素</param>
    /// <param name="name">子要素の名前</param>
    /// <returns></returns>
    public SerializedProperty GetChildProperty(SerializedProperty parent, string name)
    {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do
        {
            if (child.name == name) return child;
        }
        while (child.Next(false));
        return null;
    }

    /// <summary>
    /// 設定をすべて消去
    /// </summary>
    public void Clear()
    {
        axesProperty.ClearArray();
        serializedObject.ApplyModifiedProperties();
    }
    
}

#endif