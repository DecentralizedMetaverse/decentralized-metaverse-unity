using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TC;
using UnityEngine;

/// <summary>
/// �v���C���[�̃`�����N���펞���ׂ�
/// �ω����̓��[���h�̓ǂݍ��݂��s��
/// TODO: UniTask��Cancel�������s��
/// </summary>
public class PlayerChunk : MonoBehaviour
{
    [SerializeField] DB_Player dbPlayer;
    float divideChunkSize;

    void Start()
    {
        dbPlayer.transform = transform;
        divideChunkSize = 1.0f / GM.db.chunkSize;
        CheckChunk().Forget();
    }

    async UniTask CheckChunk()
    {
        while (true)
        {
            var (x, y) = fg.GetChunk2(transform.position, divideChunkSize);

            if (IsChangedChunk(x, y))
            {
                dbPlayer.chunkX = x;
                dbPlayer.chunkY = y;
                await GM.MsgAsync("LoadWorldByChunk", x, y);
            }

            await UniTask.Yield();
        }
    }

    bool IsChangedChunk(int x, int y)
    {
        return !(dbPlayer.chunkX == x && dbPlayer.chunkY == y);
    }
}
