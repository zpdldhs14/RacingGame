using System.Collections.Generic;
using UnityEngine;

public class MapScroller : MonoBehaviour
{
    public Transform player;             // 플레이어 참조
    public GameObject mapPrefab;         // 맵 프리팹
    public float scrollSpeed = 5f;       // 스크롤 속도
    public int poolSize = 5;             // 풀에 저장할 맵 개수
    public float mapLength = 20f;        // 한 맵의 길이

    private Queue<GameObject> mapPool = new Queue<GameObject>(); // 오브젝트 풀

    // 오브젝트 풀 초기화 (게임 시작 시 호출됨)
    public void InitializePool()
    {
        mapPool.Clear(); // 기존에 풀에 남아 있는 데이터 초기화
        for (int i = 0; i < poolSize; i++)
        {
            GameObject map = Instantiate(mapPrefab);
            map.transform.position = new Vector3(0, 0, i * mapLength);
            map.SetActive(true); // 초기에 활성화 상태 유지
            mapPool.Enqueue(map);
        }
    }

    private void Update()
    {
        if (mapPool.Count == 0) return;             // 맵이 생성되지 않은 경우 Update 실행 중지
        if (GameManager.Instance.IsGameOver) return; // 게임 오버 상태에서는 동작 중지

        // 모든 활성화된 맵 이동
        foreach (var map in mapPool)
        {
            if (map.activeSelf)
            {
                map.transform.Translate(Vector3.back * (scrollSpeed * Time.deltaTime));
            }
        }

        // 맵 재활용 처리
        if (mapPool.TryPeek(out GameObject firstMap) &&
            firstMap.activeSelf &&
            firstMap.transform.position.z < player.position.z - mapLength)
        {
            RecycleMap();
        }
    }

    // 맵을 재활용 (비활성화 -> 재배치 -> 재활성화)
    private void RecycleMap()
    {
        GameObject oldMap = mapPool.Dequeue(); // 큐에서 맵 제거
        oldMap.SetActive(false);               // 맵 비활성화

        GameObject lastMap = mapPool.Peek();   // 마지막 맵 가져오기
        oldMap.transform.position = new Vector3(0, 0, lastMap.transform.position.z + mapLength);

        // 가스 스폰 가능하도록 초기화
        oldMap.GetComponent<GasSpawner>().ResetSpawner();

        oldMap.SetActive(true);                // 맵 재활성화

        // 새로운 가스를 스폰
        oldMap.GetComponent<GasSpawner>().SpawnGas();

        mapPool.Enqueue(oldMap);               // 큐에 다시 추가
    }
}