using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    private Vector2 moveDirection;

    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // 화면 밖으로 나가면 삭제 (최적화 위해 3초 뒤 삭제)
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 1. 부딪힌 적(other)에게서 EnemyHealth 스크립트를 가져옵니다.
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();

            // 2. 만약 적에게 체력 스크립트가 있다면 데미지를 줍니다.
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Bullet의 damage 변수만큼 깎음
            }

            // 3. 총알은 적을 맞췄으니 사라집니다.
            Destroy(gameObject);
        }
    }
}