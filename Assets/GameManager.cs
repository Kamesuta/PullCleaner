using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        None,
        Control, // ����\
        Physics, // �������Z
        Brake, // ����
    }

    // ����
    public float dumpingStartSec = 5.0f;
    public float endSec = 10.0f;
    private float time;

    // �N���[�i�[
    private CleanerController cleanerController;

    // �X�e�[�g
    public GameState gameState = GameState.Control;
    private GameState lastGameState = GameState.None;

    // �}�X�N
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
        // ���Ԃ����Z
        time += Time.deltaTime;

        if (boost && Input.GetMouseButton(0))
        {
            var obj = cleanerController.Spawn();
            cleanerController.StartControl(obj.GetComponent<Rigidbody2D>());
        }

        // �X�V����
        switch (gameState)
        {
            case GameState.Control:
                break;
            case GameState.Physics:
                // �w�肵���b�����o�߂����猸������
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
            // �J�n����
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

            // �I������
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
