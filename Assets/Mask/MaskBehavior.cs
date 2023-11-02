using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskBehavior : MonoBehaviour
{
    private int totalPixels;
    private int writePixels;

    public SpriteMask mask;

    private Texture2D maskTexture;
    private bool isClear;

    public Animator maskEffect;
    public ParticleSystem maskParticle;
    public AudioSource maskAudio;

    // Start is called before the first frame update
    void Start()
    {
        // マスクのテクスチャをコピー
        maskTexture = new Texture2D(mask.sprite.texture.width, mask.sprite.texture.height, TextureFormat.RGBA32, false);
        maskTexture.SetPixels32(mask.sprite.texture.GetPixels32());
        maskTexture.Apply();
        // マスクのテクスチャを変更
        mask.sprite = Sprite.Create(maskTexture, mask.sprite.rect, Vector2.one * 0.5f);

        // マスクのテクスチャのピクセル数を取得
        totalPixels = maskTexture.width * maskTexture.height;
    }

    // Update is called once per frame
    void Update()
    {
        // デバッグ用: マスクの範囲を取得し、当たり判定が乗っている部分のテクスチャを透明にする
        /*
        if (Input.GetMouseButton(0))
        {
            // マウスの位置をテクスチャのUV座標に変換
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            WriteToTexture(worldPoint, 0.2f);
        }
        */
    }

    /// <summary>
    /// マスクのクリア率を取得
    /// </summary>
    /// <returns>クリア率</returns>
    public float GetClearRate()
    {
        return (float)writePixels / totalPixels;
    }

    /// <summary>
    /// テクスチャに書き込む
    /// </summary>
    /// <param name="worldPoint">ワールド座標</param>
    /// <param name="radius">半径</param>
    public void WriteToTexture(Vector2 worldPoint, float radius)
    {
        // 全てのピクセルを書き込んだらエフェクトを再生
        if (GetClearRate() > 0.99f && !isClear)
        {
            // マスクのテクスチャを消す
            for (int i = 0; i < maskTexture.width; i++)
            {
                for (int j = 0; j < maskTexture.height; j++)
                {
                    maskTexture.SetPixel(i, j, Color.clear);
                }
            }

            // 書き込んだピクセル数をカウント
            writePixels = totalPixels;

            isClear = true;

            // マスクのエフェクトを再生
            PlayMaskEffect();

            return;
        }

        // worldPoint(mask.bounds.min～mask.bounds.max)をmaskTextureのUV座標に変換
        float x = (worldPoint.x - mask.bounds.min.x) / mask.bounds.size.x;
        float y = (worldPoint.y - mask.bounds.min.y) / mask.bounds.size.y;
        float r = radius / mask.bounds.size.x;
        // テクスチャのUV座標をピクセル座標に変換
        int px = Mathf.FloorToInt(x * maskTexture.width);
        int py = Mathf.FloorToInt(y * maskTexture.height);
        int pr = Mathf.FloorToInt(r * maskTexture.width);
        // テクスチャのピクセル座標を透明にする
        for (int i = -pr; i <= pr; i++)
        {
            for (int j = -pr; j <= pr; j++)
            {
                // 円の内側のピクセルのみ透明にする
                if (i * i + j * j > pr * pr) continue;

                // テクスチャの範囲外は無視
                if (px + i < 0 || px + i >= maskTexture.width) continue;
                if (py + j < 0 || py + j >= maskTexture.height) continue;

                // ピクセルが既に透明なら無視
                if (maskTexture.GetPixel(px + i, py + j) == Color.clear) continue;

                // 書き込んだピクセル数をカウント
                writePixels++;
                // テクスチャのピクセルを透明にする
                maskTexture.SetPixel(px + i, py + j, Color.clear);
            }
        }
        // テクスチャの変更を適用
        maskTexture.Apply();
    }

    /// <summary>
    /// マスクのエフェクトを再生
    /// </summary>
    public void PlayMaskEffect()
    {
        maskEffect.SetTrigger("Effect");
        maskParticle.Play();
        maskAudio.Play();
    }
}
