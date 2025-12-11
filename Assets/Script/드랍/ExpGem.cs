using UnityEngine;

public class ExpGem : MonoBehaviour
{
    public int expAmount = 10; // 보석 하나당 경험치 양

    void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 부딪혔을 때만 작동
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.AddExp(expAmount); // 플레이어에게 경험치 전달
                Destroy(gameObject); // 보석은 사라짐
            }
        }
    }
}