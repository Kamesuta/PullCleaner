using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerController : MonoBehaviour
{
    // 操作系
    public Rigidbody2D rb;
    private bool isControl = false;

    // マウス、矢印系
    private Vector2 mouseLastPos;
    public GameObject arrowObject;
    public float arrowScale = 1.0f;

    // 力の強さ、減速の強さ
    public float power = 1.0f;
    public float drag = 1.0f;

    // クリーナー
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
            // マウスの位置を取得
            mouseLastPos = Input.mousePosition;
            // 矢印の位置をマウスの位置に設定
            arrowObject.transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 矢印を表示する
            arrowObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // マウスの移動量に応じて力を加える
            rb.AddForce(-((Vector2)Input.mousePosition - mouseLastPos).normalized * power);
            // ステートを更新
            FindObjectOfType<GameManager>().gameState = GameManager.GameState.Physics;
            // 矢印を非表示にする
            arrowObject.SetActive(false);
        }
        
        if (Input.GetMouseButton(0))
        {
            // 矢印の角度をマウスの位置に合わせて設定
            arrowObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Input.mousePosition.y - mouseLastPos.y, Input.mousePosition.x - mouseLastPos.x) * Mathf.Rad2Deg);
            // 矢印の長さをマウスの位置に合わせて設定
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
