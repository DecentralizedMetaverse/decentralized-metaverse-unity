using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TC;
using UnityEngine;

public class GameState : MonoBehaviour
{    
    async void Start()
    {
        //print(await GM.Msg<UniTask<string>>("cmd", $"dir {Application.dataPath}"));
        // print(await GM.Msg<UniTask<string>>("Exe", "ipfs", "--help"));
        // print(await GM.Msg<UniTask<bool>>("UploadContent", "yun.vrm"));
        
        
        //print(await GM.Msg<UniTask<bool>>("DownloadContent", "QmT3VUqAzLQSmYBUp83Ywbt57QkPs9NVhEmYVMPWXQJWtB", "0_0.yaml"));
        //print(await GM.Msg<UniTask<bool>>("DownloadContent", "QmezMyczWgBQuD3c7pZez6jE2jmofNNjbmYb6biU3nqzUc", "0_0.yaml"));
        //print(await GM.Msg<UniTask<bool>>("DownloadContent", "QmUNN63Brzn1qDxSRd1HDCsFJxboCetSckGR3FyP5sRTdQ", "yun.vrm"));
        // ���[���h�ǂݍ���
        //await GM.MsgAsync("LoadWorldByChunk", 0, 0);

        // �v���C���[����
        //await GM.MsgAsync("PlayerStart");
    }
}
