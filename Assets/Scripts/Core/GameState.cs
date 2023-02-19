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
        // ワールド読み込み
        await GM.MsgAsync("LoadWorldByChunk", 0, 0);

        // プレイヤー生成
        await GM.MsgAsync("PlayerStart");
    }
}
