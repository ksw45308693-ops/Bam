using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("AI Auto Dodge")]
    public bool useAutoMode = true; // 이동 자동화
    public float detectionRadius = 5f;
    public LayerMask enemyLayer;

    [Header("UI Controls")]
    public VirtualJoystick virtualJoystick;

    [Header("Stats")]
    public int maxHealth = 100;
    public int currentHealth;
    public float damageCooldown = 0.5f;
    private float lastDamageTime;

    [Header("Experience")]
    public int level = 1;
    public int currentExp = 0;
    public int maxExp = 100;

    // ⭐ 오토 레벨업 변수 추가
    public bool isAutoLevelUp = false;

    [Header("UI")]
    public GameObject levelUpPanel;
    public GameObject gameOverPanel;
    public Slider expSlider;
    public Slider hpSlider;
    public Text levelText;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private WeaponController weapon;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponent<WeaponController>();

        currentHealth = maxHealth;
        UpdateExpUI();
        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead) return;

        Vector2 manualInput = Vector2.zero;
        if (virtualJoystick != null && virtualJoystick.InputVector != Vector2.zero)
            manualInput = virtualJoystick.InputVector;
        else
        {
            manualInput.x = Input.GetAxisRaw("Horizontal");
            manualInput.y = Input.GetAxisRaw("Vertical");
            manualInput = manualInput.normalized;
        }

        if (manualInput != Vector2.zero) moveInput = manualInput;
        else if (useAutoMode) moveInput = GetAutoDodgeVector();
        else moveInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (isDead) return;
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isDead) return;
        if (other.CompareTag("Enemy"))
        {
            if (Time.time > lastDamageTime + damageCooldown)
            {
                TakeDamage(10);
                lastDamageTime = Time.time;
            }
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ⭐ 오토 레벨업 토글 함수 (UI 토글과 연결)
    public void SetAutoLevelUp(bool isOn)
    {
        isAutoLevelUp = isOn;
        Debug.Log("오토 레벨업 모드: " + isOn);
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        if (currentExp >= maxExp) LevelUp();
        UpdateExpUI();
    }

    void LevelUp()
    {
        currentExp -= maxExp;
        level++;
        maxExp += 50;

        // ⭐ 오토 레벨업 로직
        if (isAutoLevelUp)
        {
            // 패널을 띄우지 않고 랜덤하게 하나 선택해서 바로 적용
            int randomChoice = Random.Range(0, 2); // 0 또는 1 랜덤

            if (randomChoice == 0)
            {
                UpgradeSpeed();
                Debug.Log("🤖 오토: 이동 속도 강화 선택됨!");
            }
            else
            {
                UpgradeAttack();
                Debug.Log("🤖 오토: 공격 속도 강화 선택됨!");
            }

            // 주의: Upgrade 함수들이 CloseLevelUpPanel을 부르지만,
            // 이미 Time.timeScale이 1인 상태라 문제없음.
        }
        else
        {
            // 수동 모드일 때만 패널 띄우고 멈춤
            if (levelUpPanel != null)
            {
                levelUpPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        UpdateExpUI();
    }

    public void UpgradeSpeed() { moveSpeed += 1f; CloseLevelUpPanel(); }
    public void UpgradeAttack() { if (weapon != null) weapon.attackRate *= 0.9f; CloseLevelUpPanel(); }

    void CloseLevelUpPanel()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // --- (이하 동일) ---
    void UpdateHealthUI() { if (hpSlider != null) hpSlider.value = (float)currentHealth / maxHealth; }
    void UpdateExpUI() { if (expSlider != null) expSlider.value = (float)currentExp / maxExp; }

    Vector2 GetAutoDodgeVector()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        if (enemies.Length == 0) return Vector2.zero;
        Vector2 fleeDirection = Vector2.zero;
        foreach (Collider2D enemy in enemies)
        {
            Vector2 directionToMe = (Vector2)transform.position - (Vector2)enemy.transform.position;
            fleeDirection += directionToMe.normalized / (directionToMe.magnitude + 0.1f);
        }
        return fleeDirection.normalized;
    }
}