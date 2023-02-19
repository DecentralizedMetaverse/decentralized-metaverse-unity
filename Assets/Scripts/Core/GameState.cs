using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TC;
using UnityEngine;

public class GameState : MonoBehaviour
{    
    async void Start()
    {
        print(await GM.Msg<UniTask<string>>("ls"));
        // ���[���h�ǂݍ���
        await GM.MsgAsync("LoadWorldByChunk", 0, 0);

        // �v���C���[����
        await GM.MsgAsync("PlayerStart");
    }
}
