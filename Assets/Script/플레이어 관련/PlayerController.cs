using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Experience")]
    public int level = 1;
    public int currentExp = 0;
    public int maxExp = 100;

    [Header("UI")]
    public GameObject levelUpPanel; // ⭐ 유니티에서 만든 패널을 여기에 넣을 겁니다.

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private WeaponController weapon; // 무기 성능도 올려야 하니까 가져옵니다.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponent<WeaponController>(); // 같은 오브젝트에 있는 무기 스크립트 찾기
    }

    void Update()
    {
        // 입력 받기
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        if (currentExp >= maxExp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentExp = currentExp - maxExp;
        level++;
        maxExp += 50;

        Debug.Log($"🎉 레벨 업! 현재 레벨: {level}");

        // ⭐ 핵심: 레벨업 패널을 켜고, 시간을 멈춤
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true); // 패널 켜기
            Time.timeScale = 0f; // 0이면 게임 시간이 멈춤 (일시정지)
        }
    }

    // --- 버튼을 눌렀을 때 실행될 함수들 ---

    // 1번 버튼: 이동 속도 증가
    public void UpgradeSpeed()
    {
        moveSpeed += 1f; // 속도 1 증가
        CloseLevelUpPanel(); // 창 닫기
    }

    // 2번 버튼: 공격 속도 증가
    public void UpgradeAttack()
    {
        if (weapon != null)
        {
            weapon.attackRate *= 0.9f; // 공격 쿨타임 10% 감소 (더 빨라짐)
        }
        CloseLevelUpPanel();
    }

    // 공통: 패널을 닫고 게임 재개
    void CloseLevelUpPanel()
    {
        levelUpPanel.SetActive(false); // 패널 끄기
        Time.timeScale = 1f; // 시간 다시 흐르게 하기 (1 = 정상 속도)
    }
}