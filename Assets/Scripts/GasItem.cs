using UnityEngine;

public class GasItem : MonoBehaviour
{
    public float gasAmount = 30f; // 증가시킬 가스량

    private void OnTriggerEnter(Collider other)
    {
        CarController carController = other.GetComponent<CarController>(); // CarController 찾기
        if (carController != null)
        {
            carController.IncreaseGas(gasAmount); // 가스 증가
            Destroy(gameObject); // 아이템 제거
        }
    }
}