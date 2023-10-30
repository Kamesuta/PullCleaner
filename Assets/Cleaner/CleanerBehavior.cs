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
        // ���g�͈̔͂��擾
        Vector2 worldPoint = transform.position;
        float radius = circleCollider.radius * transform.localScale.x;
        // �}�X�N�͈̔͂��擾���A�����蔻�肪����Ă��镔���̃e�N�X�`���𓧖��ɂ���
        mask.WriteToTexture(worldPoint, radius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �}�X�N�ɓ��������特��炷
        if (collision.gameObject.layer == gameObject.layer)
        {
            if (collision.gameObject.GetInstanceID() < gameObject.GetInstanceID())
            {
                // �����������x���v�Z
                hitAudio.volume = Mathf.Clamp01(collision.relativeVelocity.magnitude * 0.1f);
                hitAudio.Play();

                // ���������ꏊ�ɔ����G�t�F�N�g�𐶐�
                Quaternion randomRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
                GameObject obj = Instantiate(bombEffectPrefab, collision.contacts[0].point, randomRotation);
                obj.transform.localScale = Vector3.one * collision.relativeVelocity.magnitude * 0.05f;
                Destroy(obj, 0.5f);
            }
        }
    }
}
