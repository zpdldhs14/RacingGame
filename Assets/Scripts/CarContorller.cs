using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    public float speed = 10f;         // 이동 속도
    public float laneDistance = 3f;  // 이동할 레인 거리
    private int currentLine = 1;      // 현재 레인 위치

    public float gas = 100f;               // 현재 가스(체력)
    public float gasDecreaseRate = 10f;    // 초당 감소 속도
    private bool isGameOver = false;       // 게임 종료 상태

    private void Start()
    {
        StartCoroutine(ConsumeGas()); // 가스 소비 시작
        UIManager.Instance.UpdateGasUI(gas); // UI 초기화
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;
        // 플레이어 이동
        if (Input.GetKeyDown(KeyCode.A) && currentLine > 0)
        {
            currentLine--;
            MoveLine();
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentLine < 2)
        {
            currentLine++;
            MoveLine();
        }
    }

    private void MoveLine()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = (currentLine - 1) * laneDistance;
        transform.position = newPosition;
    }

    IEnumerator ConsumeGas()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(1f);

            // 가스 감소
            gas -= gasDecreaseRate;
            UIManager.Instance.UpdateGasUI(gas); // UI 업데이트

            if (gas <= 0)
            {
                EndGame();
                break;
            }
        }
    }

    public void IncreaseGas(float amount)
    {
        gas += amount; // 가스 증가
        gas = Mathf.Clamp(gas, 0, 100); // 가스를 0~100 사이로 고정
        UIManager.Instance.UpdateGasUI(gas); // UI 업데이트
    }

    public void EndGame()
    {
        isGameOver = true;
        // GameManager.Instance를 통해 상태 변경
        GameManager.Instance.SetGameOver(true);
    }
}