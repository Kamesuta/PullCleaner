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
        // �}�X�N�̃e�N�X�`�����R�s�[
        maskTexture = new Texture2D(mask.sprite.texture.width, mask.sprite.texture.height, TextureFormat.RGBA32, false);
        maskTexture.SetPixels32(mask.sprite.texture.GetPixels32());
        maskTexture.Apply();
        // �}�X�N�̃e�N�X�`����ύX
        mask.sprite = Sprite.Create(maskTexture, mask.sprite.rect, Vector2.one * 0.5f);

        // �}�X�N�̃e�N�X�`���̃s�N�Z�������擾
        totalPixels = maskTexture.width * maskTexture.height;
    }

    // Update is called once per frame
    void Update()
    {
        // �f�o�b�O�p: �}�X�N�͈̔͂��擾���A�����蔻�肪����Ă��镔���̃e�N�X�`���𓧖��ɂ���
        /*
        if (Input.GetMouseButton(0))
        {
            // �}�E�X�̈ʒu���e�N�X�`����UV���W�ɕϊ�
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            WriteToTexture(worldPoint, 0.2f);
        }
        */
    }

    /// <summary>
    /// �}�X�N�̃N���A�����擾
    /// </summary>
    /// <returns>�N���A��</returns>
    public float GetClearRate()
    {
        return (float)writePixels / totalPixels;
    }

    /// <summary>
    /// �e�N�X�`���ɏ�������
    /// </summary>
    /// <param name="worldPoint">���[���h���W</param>
    /// <param name="radius">���a</param>
    public void WriteToTexture(Vector2 worldPoint, float radius)
    {
        // �S�Ẵs�N�Z�����������񂾂�G�t�F�N�g���Đ�
        if (GetClearRate() > 0.99f && !isClear)
        {
            // �}�X�N�̃e�N�X�`��������
            for (int i = 0; i < maskTexture.width; i++)
            {
                for (int j = 0; j < maskTexture.height; j++)
                {
                    maskTexture.SetPixel(i, j, Color.clear);
                }
            }

            // �������񂾃s�N�Z�������J�E���g
            writePixels = totalPixels;

            isClear = true;

            // �}�X�N�̃G�t�F�N�g���Đ�
            PlayMaskEffect();

            return;
        }

        // worldPoint(mask.bounds.min�`mask.bounds.max)��maskTexture��UV���W�ɕϊ�
        float x = (worldPoint.x - mask.bounds.min.x) / mask.bounds.size.x;
        float y = (worldPoint.y - mask.bounds.min.y) / mask.bounds.size.y;
        float r = radius / mask.bounds.size.x;
        // �e�N�X�`����UV���W���s�N�Z�����W�ɕϊ�
        int px = Mathf.FloorToInt(x * maskTexture.width);
        int py = Mathf.FloorToInt(y * maskTexture.height);
        int pr = Mathf.FloorToInt(r * maskTexture.width);
        // �e�N�X�`���̃s�N�Z�����W�𓧖��ɂ���
        for (int i = -pr; i <= pr; i++)
        {
            for (int j = -pr; j <= pr; j++)
            {
                // �~�̓����̃s�N�Z���̂ݓ����ɂ���
                if (i * i + j * j > pr * pr) continue;

                // �e�N�X�`���͈̔͊O�͖���
                if (px + i < 0 || px + i >= maskTexture.width) continue;
                if (py + j < 0 || py + j >= maskTexture.height) continue;

                // �s�N�Z�������ɓ����Ȃ疳��
                if (maskTexture.GetPixel(px + i, py + j) == Color.clear) continue;

                // �������񂾃s�N�Z�������J�E���g
                writePixels++;
                // �e�N�X�`���̃s�N�Z���𓧖��ɂ���
                maskTexture.SetPixel(px + i, py + j, Color.clear);
            }
        }
        // �e�N�X�`���̕ύX��K�p
        maskTexture.Apply();
    }

    /// <summary>
    /// �}�X�N�̃G�t�F�N�g���Đ�
    /// </summary>
    public void PlayMaskEffect()
    {
        maskEffect.SetTrigger("Effect");
        maskParticle.Play();
        maskAudio.Play();
    }
}
