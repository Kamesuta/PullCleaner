using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerController : MonoBehaviour
{
    // ����n
    public Rigidbody2D rb;
    private bool isControl = false;

    // �}�E�X�A���n
    private Vector2 mouseLastPos;
    public GameObject arrowObject;
    public float arrowScale = 1.0f;

    // �͂̋����A�����̋���
    public float power = 1.0f;
    public float drag = 1.0f;

    // �N���[�i�[
    public GameObject cleanerPrefab;
    public GameObject cleanerParent;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isControl)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // �}�E�X�̈ʒu���擾
            mouseLastPos = Input.mousePosition;
            // ���̈ʒu���}�E�X�̈ʒu�ɐݒ�
            arrowObject.transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // ����\������
            arrowObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // �}�E�X�̈ړ��ʂɉ����ė͂�������
            rb.AddForce(-((Vector2)Input.mousePosition - mouseLastPos).normalized * power);
            // �X�e�[�g���X�V
            FindObjectOfType<GameManager>().gameState = GameManager.GameState.Physics;
            // �����\���ɂ���
            arrowObject.SetActive(false);
        }
        
        if (Input.GetMouseButton(0))
        {
            // ���̊p�x���}�E�X�̈ʒu�ɍ��킹�Đݒ�
            arrowObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Input.mousePosition.y - mouseLastPos.y, Input.mousePosition.x - mouseLastPos.x) * Mathf.Rad2Deg);
            // ���̒������}�E�X�̈ʒu�ɍ��킹�Đݒ�
            arrowObject.transform.localScale = new Vector3(Vector2.Distance(Input.mousePosition, mouseLastPos) * arrowScale, 1, 1);
        }
    }

    public GameObject Spawn()
    {
        return Instantiate(cleanerPrefab, cleanerParent.transform);
    }

    public void StartControl(Rigidbody2D rb2d)
    {
        rb = rb2d;
        isControl = true;
    }

    public void EndControl()
    {
        arrowObject.SetActive(false);
        isControl = false;
    }

    public void StartBrake()
    {
        foreach (var rb in cleanerParent.GetComponentsInChildren<Rigidbody2D>())
        {
            rb.drag = drag;
        }
    }

    public void EndBrake()
    {
        foreach (var rb in cleanerParent.GetComponentsInChildren<Rigidbody2D>())
        {
            rb.velocity = Vector2.zero;
            rb.drag = 0;
        }
    }
}
