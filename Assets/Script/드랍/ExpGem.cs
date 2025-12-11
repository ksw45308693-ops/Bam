using UnityEngine;

public class ExpGem : MonoBehaviour
{
    public int expAmount = 10;

    // 자석 기능 변수들
    public float magnetRange = 4f; // 보석이 빨려오는 거리 (이 범위 안에 들면 날아옴)
    public float moveSpeed = 8f;   // 날아오는 속도

    private Transform playerTransform;
    private bool isAttracted = false; // 빨려가는 중인지 체크

    void Start()
    {
        // 게임 시작 시 플레이어 위치를 미리 찾아둠
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // 1. 거리 계산
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // 2. 자석 범위 안에 들어왔다면 "빨려감 모드" 발동
        if (distance < magnetRange)
        {
            isAttracted = true;
        }

        // 3. 빨려가는 움직임 처리
        if (isAttracted)
        {
            // 플레이어 쪽으로 이동 (시간이 지날수록 가속도가 붙게 할 수도 있음)
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            // 날아오는 속도를 점점 빠르게 (선택사항)
            moveSpeed += 5f * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.AddExp(expAmount);
                Destroy(gameObject);
            }
        }
    }
}