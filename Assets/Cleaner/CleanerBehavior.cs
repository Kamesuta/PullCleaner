using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerBehavior : MonoBehaviour
{
    private MaskBehavior mask;
    private CircleCollider2D circleCollider;
    public AudioSource hitAudio;
    public GameObject bombEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        mask = FindObjectOfType<GameManager>().mask;
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 自身の範囲を取得
        Vector2 worldPoint = transform.position;
        float radius = circleCollider.radius * transform.localScale.x;
        // マスクの範囲を取得し、当たり判定が乗っている部分のテクスチャを透明にする
        mask.WriteToTexture(worldPoint, radius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // マスクに当たったら音を鳴らす
        if (collision.gameObject.layer == gameObject.layer)
        {
            if (collision.gameObject.GetInstanceID() < gameObject.GetInstanceID())
            {
                // 当たった速度を計算
                hitAudio.volume = Mathf.Clamp01(collision.relativeVelocity.magnitude * 0.1f);
                hitAudio.Play();

                // 当たった場所に爆発エフェクトを生成
                Quaternion randomRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
                GameObject obj = Instantiate(bombEffectPrefab, collision.contacts[0].point, randomRotation);
                obj.transform.localScale = Vector3.one * collision.relativeVelocity.magnitude * 0.05f;
                Destroy(obj, 0.5f);
            }
        }
    }
}
