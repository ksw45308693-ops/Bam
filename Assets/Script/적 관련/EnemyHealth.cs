using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30; // 적의 최대 체력
    private int currentHealth;

    public GameObject expGemPrefab; // ⭐ 추가: 여기에 보석 프리팹을 넣을 겁니다.

    void Start()
    {
        // 태어날 때 체력을 가득 채움
        currentHealth = maxHealth;
    }

    // 외부(총알)에서 이 함수를 호출해서 데미지를 줌
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // 체력 깎기

        // 체력이 0 이하가 되면 사망
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // ⭐ 추가: 보석 생성 (내 위치에)
        if (expGemPrefab != null)
        {
            Instantiate(expGemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}