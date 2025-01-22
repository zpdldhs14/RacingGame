using System.Collections;
using UnityEngine;

public class GasSpawner : MonoBehaviour
{
    public GameObject gasPrefab;                  // 가스 프리팹
    public Transform[] spawnPoints;              // 가스가 생성될 중앙, 왼쪽, 오른쪽 스폰 지점
    public float spawnProbability = 0.8f;

    private GameObject currentGas;  // 현재 Spawner가 관리하는 Gas 인스턴스
    
    public void SpawnGas()
    {
        // 가스가 이미 생성되었다면 추가로 스폰하지 않음
        if (currentGas != null) return;

        // 확률 계산 (0 ~ 1 사이 수에서 spawnProbability보다 높으면 스폰하지 않음)
        if (Random.value > spawnProbability) return;

        // 스폰 지점 중 하나를 랜덤으로 선택
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // 가스를 선택된 위치에 생성
        currentGas = Instantiate(gasPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);

        // 가스를 맵의 자식으로 설정
        currentGas.transform.parent = this.transform;
    }

    public void ResetSpawner()
    {
        // 이전에 생성된 Gas가 있다면 제거
        if (currentGas != null)
        {
            Destroy(currentGas);
            currentGas = null; // 참조 해제
        }
    }
}