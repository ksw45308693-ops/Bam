using UnityEngine;

public class Reposition : MonoBehaviour
{
    private GameObject player;
    private float tileSize = 20f; // 타일 하나의 가로/세로 길이 (유니티 단위)

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // 내 스프라이트의 실제 크기를 자동으로 가져옴 (BoxCollider2D가 있어야 함)
        if (GetComponent<BoxCollider2D>() != null)
        {
            tileSize = GetComponent<BoxCollider2D>().size.x;
        }
    }

    void Update()
    {
        if (player == null) return;

        // 플레이어와 나의 거리 차이 계산
        float diffX = player.transform.position.x - transform.position.x;
        float diffY = player.transform.position.y - transform.position.y;

        // X축 이동 (가로)
        // 거리가 타일 크기보다 멀어지면 -> 타일 크기 * 2 만큼 이동 (반대편으로 점프)
        float dirX = diffX < 0 ? -1 : 1;
        if (Mathf.Abs(diffX) > tileSize)
        {
            transform.Translate(Vector3.right * dirX * tileSize * 2);
        }

        // Y축 이동 (세로)
        float dirY = diffY < 0 ? -1 : 1;
        if (Mathf.Abs(diffY) > tileSize)
        {
            transform.Translate(Vector3.up * dirY * tileSize * 2);
        }
    }
}