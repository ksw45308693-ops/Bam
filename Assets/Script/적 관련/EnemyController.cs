using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f; // 적 이동 속도

    private Transform target; // 추적할 대상(플레이어)
    private Rigidbody2D rb;
    private SpriteRenderer spr; // 적이 바라보는 방향을 뒤집기 위해 필요

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();

        // 게임 시작 시 "Player"라는 태그를 가진 오브젝트를 찾아서 타겟으로 설정
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        // 타겟(플레이어)이 없으면 아무것도 안 함 (플레이어가 죽었을 때 등)
        if (target == null) return;

        // 1. 방향 구하기 (플레이어 위치 - 내 위치)
        Vector2 direction = (target.position - transform.position).normalized;

        // 2. 이동하기 (물리 엔진 사용)
        Vector2 nextPos = rb.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);

        // 3. 시선 처리 (오른쪽/왼쪽 바라보기)
        // 적의 x 좌표가 플레이어보다 크면(오른쪽에 있으면) 왼쪽을 봐야 함 -> flipX = true
        spr.flipX = target.position.x < transform.position.x;
    }
}