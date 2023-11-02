using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        None,
        Control, // 操作可能
        Physics, // 物理演算
        Brake, // 減速
    }

    // 時関
    public float dumpingStartSec = 5.0f;
    public float endSec = 10.0f;
    private float time;

    // クリーナー
    private CleanerController cleanerController;

    // ステート
    public GameState gameState = GameState.Control;
    private GameState lastGameState = GameState.None;

    // マスク
    public MaskBehavior mask;

    public bool boost = false;

    // Start is called before the first frame update
    void Start()
    {
        cleanerController = GetComponent<CleanerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 時間を加算
        time += Time.deltaTime;

        if (boost && Input.GetMouseButton(0))
        {
            var obj = cleanerController.Spawn();
            cleanerController.StartControl(obj.GetComponent<Rigidbody2D>());
        }

        // 更新処理
        switch (gameState)
        {
            case GameState.Control:
                break;
            case GameState.Physics:
                // 指定した秒数が経過したら減速する
                if (time > dumpingStartSec)
                {
                    gameState = GameState.Brake;
                }
                break;
            case GameState.Brake:
                if (time > endSec)
                {
                    gameState = GameState.Control;
                }
                break;
        }

        if (gameState != lastGameState)
        {
            // 開始処理
            switch (gameState)
            {
                case GameState.Control:
                    var obj = cleanerController.Spawn();
                    cleanerController.StartControl(obj.GetComponent<Rigidbody2D>());
                    break;
                case GameState.Physics:
                    break;
                case GameState.Brake:
                    cleanerController.StartBrake();
                    break;
            }

            // 終了処理
            switch (lastGameState)
            {
                case GameState.Control:
                    cleanerController.EndControl();
                    break;
                case GameState.Physics:
                    break;
                case GameState.Brake:
                    cleanerController.EndBrake();
                    time = 0;
                    break;
            }
        }

        lastGameState = gameState;
    }
}
