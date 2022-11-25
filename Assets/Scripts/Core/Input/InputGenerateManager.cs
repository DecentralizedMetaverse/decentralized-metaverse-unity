#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class InputGenerateManager : MonoBehaviour {

	void Start () {
        //インプットマネージャの設定をリセット
        AutoSetInputManager.ResetInputManger();
	}
}

#endif