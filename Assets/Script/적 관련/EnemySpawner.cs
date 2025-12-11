using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnInterval = 1f;
    public float spawnRadius = 10f; // 플레이어로부터 생성되는 거리

    private float timer;

    void Update()
    {
        if (player == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // 플레이어 주변 원형 랜덤 위치 계산
        Vector2 randomPos = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randomPos.x, randomPos.y, 0);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}