using System.Xml.Serialization;
using UnityEngine;
using System.Collections;


public class Player_move : MonoBehaviour
{

    [Header("이동 & 액션")]
    [SerializeField] public float speed = 3f;               // 걷기 속도
    [SerializeField] private float jumpForce = 7f;           // 점프 힘

    // ===== 상태 플래그 =====
    private bool isDash;

    [SerializeField] public int jumpCount = 0;
    private int facingDir = 1;
    public static float speedUP = 1;
    private const int maxJumps = 2;
    private float moveInput;
    public float dashDuration = 0.25f;
    public float dashSpeed = 3f;
    public float dashCooldown = 5.0f; // 쿨타임 설정
    private Vector2 dashInputDir;
    private float originalGravity;
    public bool isjump = false;
    private Coroutine _animationSlowRoutine;
    public DashSkill Dash => dash;

    // “스탯”이 주는 기본 이동속도 (예: 레벨, 스텟 포인트)
    [SerializeField] private float statMoveSpeed = 3f;

    // 장비/아이템이 주는 보너스 (예: 신발, 반지 등)
    private float equipmentSpeedBonus = 0f;

    // 버프(가속) 멀티플라이어
    private float buffSpeedMultiplier = 1f;

    // 디버프(감속) 멀티플라이어
    private float debuffSpeedMultiplier = 1f;

    public float finalSpeed = 0f;

    public float CurrentMoveSpeed => CalculateCurrentSpeed();

    // 디버프 복구 코루틴 레퍼런스
    private Coroutine _speedRecoverRoutine;

    // ===== 내부에서 사용할 참조 =====
    private PlayerAnimationSync sync;
    private Rigidbody2D rb;
    public DashSkill dash;
    private Ground ground;
    private PlayerHealth health;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sync = GetComponentInParent<PlayerAnimationSync>();
        dash = GetComponent<DashSkill>();
        ground = GetComponentInParent<Ground>();
        health = GetComponentInParent<PlayerHealth>();
    }
    private void Update()
    {
        //Debug.Log(rb.linearVelocity);
        HandleMovementAndJump();
    }

    private void FixedUpdate()
    {
        // 기본 이동 처리 (대쉬 / 넉백중 아닐 때)
        if (!dash.IsDashing && !health.isknockback) //대쉬중이 아니고 , 넉백 중이 아닐 때면 y값은 냅두고 x값에 움직임 속도 적용.
        {
            finalSpeed = CalculateCurrentSpeed();
            rb.linearVelocity = new Vector2(moveInput * finalSpeed, rb.linearVelocity.y);
        }
    }

    private void HandleMovementAndJump()
    {
        // 이동 방향 입력 (좌우 이동용)
        moveInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1f;

        // 대쉬 방향 입력 (8방향)
        dashInputDir = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow)) dashInputDir.y += 1f;
        if (Input.GetKey(KeyCode.DownArrow)) dashInputDir.y -= 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) dashInputDir.x -= 1f;
        if (Input.GetKey(KeyCode.RightArrow)) dashInputDir.x += 1f;

        dashInputDir = dashInputDir.normalized;

        if (Input.GetKeyDown(KeyCode.D))
        {
            //dash.TryDash(dashInputDir, rb, animController);
            dash.TryDash(dashInputDir, rb);
            StartCoroutine(IgnoreCollider(0.2f));
        }

        // 캐릭터 바라보는 방향 업데이트
        if (moveInput != 0f)
        {
            facingDir = (int)Mathf.Sign(moveInput);
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * facingDir;
            transform.localScale = scale;
        }

        // 애니메이션 처리
        sync.AirSpeedY(rb.linearVelocity.y);
        sync.IsWalking(moveInput != 0f);

        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Space) && ground.floatGround)
            {
                StartCoroutine(IgnoreCollider(0.5f));
            }
            else
            {
                Debug.Log("kkkkkk");
                sync.Jump();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                ground.isGroundedNow = false;
                isjump = true;
                jumpCount++;
                StartCoroutine(Jump());
                StartCoroutine(IgnoreCollider(0.4f));
            }

        }
    }
    public bool IsInvincible()
    {
        return dash.IsInvincible;
    }

    public void ResetJumpCount()
    {
        jumpCount = 0;
        //Debug.Log("Player_move: 점프 카운트 초기화");
    }
    private float CalculateCurrentSpeed()
    {
        float basePlusEquip = speed + statMoveSpeed + equipmentSpeedBonus;
        return basePlusEquip * buffSpeedMultiplier * debuffSpeedMultiplier * speedUP;
    }

    /// <summary>
    /// 현재 재생 중인 애니메이션이 끝날 때까지 속도를 느리게 유지합니다.
    /// </summary>
    public void AttackSpeedDownDuringAnimation(float slowRatio = 0.1f)
    {
        // 이미 실행 중인 코루틴이 있으면 중단
        if (_animationSlowRoutine != null)
            StopCoroutine(_animationSlowRoutine);

        // 속도 느려짐 적용
        debuffSpeedMultiplier = slowRatio;

        // 지금 재생 중인 애니메이션 State의 해시 저장
        AnimatorStateInfo stateInfo = sync.CurrentStateInfo;
        int currentStateHash = stateInfo.shortNameHash;

        // 해당 State가 끝날 때까지 복구 코루틴 실행
        _animationSlowRoutine = StartCoroutine(RecoverAfterAnimationEnd(currentStateHash));
    }
    private IEnumerator RecoverAfterAnimationEnd(int stateHash)
    {
        // 첫 프레임 건너뛰기
        yield return null;

        // 같은 State가 끝나지 않았으면 계속 대기
        while (sync.CurrentStateInfo.shortNameHash == stateHash
               && sync.CurrentStateInfo.normalizedTime < 1f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        // 애니메이션 종료 시점에 속도 복구
        debuffSpeedMultiplier = 1f;
        _animationSlowRoutine = null;
    }

    private IEnumerator Jump()
    {
        yield return null;
        isjump = false;
    }

    private IEnumerator IgnoreCollider(float ignoreTime)
    {
        CircleCollider2D Collider = GetComponent<CircleCollider2D>();
        Collider.enabled = false;
        yield return new WaitForSeconds(ignoreTime);
        Collider.enabled = true;
    }
}



