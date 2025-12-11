using UnityEngine;

public class ExpGem : MonoBehaviour
{
    public int expAmount = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 무엇이든 닿으면 일단 소리쳐!
        Debug.Log("💎 보석에 무언가 닿았어요! 이름: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("--> 플레이어 확인됨! 경험치 지급 시도");
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.AddExp(expAmount);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("--> 오류: 플레이어에게 PlayerController 스크립트가 없어요!");
            }
        }
        else
        {
            Debug.Log($"--> 하지만 태그가 Player가 아니에요. (현재 태그: {other.tag})");
        }
    }
}