using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TC;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

/// <summary>
/// QRコードをスキャンするプログラム
/// </summary>
public class QRCodeReader : MonoBehaviour
{
    [SerializeField] RawImage rawImage;
    WebCamTexture camTexture;

    void Start()
    {
        GM.Add<UniTask<string>>("ReadQRCode", ReadQRCode);
    }

    async UniTask<string> ReadQRCode()
    {
        camTexture = new WebCamTexture();
        rawImage.texture = camTexture;
        camTexture.Play();
        return await GetCodeContent();
    }

    async UniTask<string> GetCodeContent()
    {
        var result = "";

        while (true)
        {
            result = ReadCode(camTexture);

            if (result != "")
            {
                print(result);
                camTexture.Stop();
                return result;
            }

            await UniTask.Yield();
        }
    }

    string ReadCode(WebCamTexture texture)
    {
        var reader = new BarcodeReader();
        var rawRGB = texture.GetPixels32();
        var width = texture.width;
        var height = texture.height;
        var result = reader.Decode(rawRGB, width, height);
        return result != null ? result.Text : string.Empty;
    }
}
