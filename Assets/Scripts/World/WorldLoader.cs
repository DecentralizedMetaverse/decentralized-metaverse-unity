using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TC;
using UnityEngine;

/// <summary>
/// ChunkからWorldのLoad・Unloadを管理するProgram
/// </summary>
public class WorldLoader : MonoBehaviour
{
    const int loadMapSize = 3;
    const int loadMapMax = 12;

    Dictionary<(int, int), GameObject> map = new (loadMapMax);

    void Awake()
    {
        GM.Add<int, int, UniTask>("LoadWorldByChunk", LoadWorldByChunk);
    }

    /// <summary>
    /// 対象ChunkのWorldを生成する
    /// </summary>
    /// <param name="chunkX"></param>
    /// <param name="chunkY"></param>
    /// <returns></returns>
    async UniTask LoadWorldByChunk(int chunkX, int chunkY)
    {
        var loadTargetMap = CreateLoadList(chunkX, chunkY);
        
        await LoadMapAsync(loadTargetMap);

        UnloadMap(loadTargetMap).Forget();
    }

    async UniTask LoadMapAsync(HashSet<(int, int)> loadTargetMap)
    {
        foreach (var (loadX, loadY) in loadTargetMap)
        {
            if (map.ContainsKey((loadX, loadY))) continue;

            var obj = GM.Msg<GameObject>("GenerateWorld", loadX, loadY);
            map.Add((loadX, loadY), obj);

            await UniTask.Yield();
        }
    }

    /// <summary>
    /// 生成するワールドのリストを作成する
    /// </summary>
    /// <param name="chunkX"></param>
    /// <param name="chunkY"></param>
    /// <returns></returns>
    HashSet<(int, int)> CreateLoadList(int chunkX, int chunkY)
    {
        HashSet<(int, int)> loadTargetMap = new(loadMapSize * loadMapSize);

        for (int j = 0; j < loadMapSize; j++)
        {
            for (int i = 0; i < loadMapSize; i++)
            {
                var x = chunkX + i - 1;
                var y = chunkY + j - 1;

                loadTargetMap.Add((x, y));
                print($"{x}, {y}");
            }
        }

        return loadTargetMap;
    }

    async UniTask UnloadMap(HashSet<(int, int)> loadTargetMap)
    {
        var unloadMapList = new HashSet<(int, int)>(loadMapMax);

        // Terrainの削除
        foreach (var mapKey in map.Keys)
        {
            if (loadTargetMap.Contains(mapKey)) continue;

            Destroy(map[mapKey]);
            unloadMapList.Add(mapKey);

            await UniTask.Yield();
        }

        // リストの更新
        foreach (var mapKey in unloadMapList)
        {
            map.Remove(mapKey);
        }
    }
}
