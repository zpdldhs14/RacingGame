using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // 싱글톤 인스턴스
    public GameObject carPrefab;         // 플레이어 프리팹
    public GameObject map;               // 맵 오브젝트
    private GameObject player;            // 현재 플레이어 인스턴스 레퍼런스
    public bool IsGameOver => isGameOver; // 외부에서 읽기 가능
    private bool isGameOver = false;     // 게임 오버 상태

    private void Awake()
    {
        if (Instance == null) Instance = this; // 싱글톤 초기화
    }

    // 상태 전환 메서드 추가
    public void SetGameOver(bool state)
    {
        isGameOver = state;
        if (state) // 만약 게임 오버 상태로 전환된 경우 추가 처리
        {
            UIManager.Instance.ShowGameOverPanel(); // 게임 오버 UI 호출
            Time.timeScale = 0f;                   // 게임 시간 정지
        }
    }

    // 게임 시작/재시작 메서드
    public void StartGame()
    {
        // 시간 흐름 재개, 게임 오버 상태 초기화
        isGameOver = false;
        Time.timeScale = 1f;

        // 기존 플레이어 제거 (재시작 시 필요)
        if (player != null)
        {
            Destroy(player);
        }

        // 플레이어 새로 생성
        player = Instantiate(carPrefab, new Vector3(0, 1f, -10.8f), Quaternion.identity);

        // UI 초기화
        UIManager.Instance.UpdateGasUI(100); // 가스 UI 초기화

        // 맵 초기화
        MapScroller mapScroller = FindObjectOfType<MapScroller>();
        if (mapScroller != null)
        {
            mapScroller.gameObject.SetActive(true);
            mapScroller.InitializePool();
        }
    }
}