using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float attackRate = 1f; // 공격 속도
    public float range = 10f; // 사거리

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > attackRate)
        {
            AttackNearestEnemy();
            timer = 0;
        }
    }

    void AttackNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        // 가장 가까운 적 찾기
        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, currentPos);
            if (dist < minDistance && dist <= range)
            {
                nearestEnemy = enemy;
                minDistance = dist;
            }
        }

        // 적이 있으면 발사
        if (nearestEnemy != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;
            bullet.GetComponent<Bullet>().SetDirection(direction);
        }
    }
}