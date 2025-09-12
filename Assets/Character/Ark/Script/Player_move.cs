using System.Xml.Serialization;
using UnityEngine;
using System.Collections;


public class Player_move : MonoBehaviour
{

    [Header("�̵� & �׼�")]
    [SerializeField] public float speed = 3f;               // �ȱ� �ӵ�
    [SerializeField] private float jumpForce = 7f;           // ���� ��

    // ===== ���� �÷��� =====
    private bool isDash;

    [SerializeField] public int jumpCount = 0;
    private int facingDir = 1;
    public static float speedUP = 1;
    private const int maxJumps = 2;
    private float moveInput;
    public float dashDuration = 0.25f;
    public float dashSpeed = 3f;
    public float dashCooldown = 5.0f; // ��Ÿ�� ����
    private Vector2 dashInputDir;
    private float originalGravity;
    public bool isjump = false;
    private Coroutine _animationSlowRoutine;
    public DashSkill Dash => dash;

    // �����ȡ��� �ִ� �⺻ �̵��ӵ� (��: ����, ���� ����Ʈ)
    [SerializeField] private float statMoveSpeed = 3f;

    // ���/�������� �ִ� ���ʽ� (��: �Ź�, ���� ��)
    private float equipmentSpeedBonus = 0f;

    // ����(����) ��Ƽ�ö��̾�
    private float buffSpeedMultiplier = 1f;

    // �����(����) ��Ƽ�ö��̾�
    private float debuffSpeedMultiplier = 1f;

    public float finalSpeed = 0f;

    public float CurrentMoveSpeed => CalculateCurrentSpeed();

    // ����� ���� �ڷ�ƾ ���۷���
    private Coroutine _speedRecoverRoutine;

    // ===== ���ο��� ����� ���� =====
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
        // �⺻ �̵� ó�� (�뽬 / �˹��� �ƴ� ��)
        if (!dash.IsDashing && !health.isknockback) //�뽬���� �ƴϰ� , �˹� ���� �ƴ� ���� y���� ���ΰ� x���� ������ �ӵ� ����.
        {
            finalSpeed = CalculateCurrentSpeed();
            rb.linearVelocity = new Vector2(moveInput * finalSpeed, rb.linearVelocity.y);
        }
    }

    private void HandleMovementAndJump()
    {
        // �̵� ���� �Է� (�¿� �̵���)
        moveInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1f;

        // �뽬 ���� �Է� (8����)
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

        // ĳ���� �ٶ󺸴� ���� ������Ʈ
        if (moveInput != 0f)
        {
            facingDir = (int)Mathf.Sign(moveInput);
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * facingDir;
            transform.localScale = scale;
        }

        // �ִϸ��̼� ó��
        sync.AirSpeedY(rb.linearVelocity.y);
        sync.IsWalking(moveInput != 0f);

        // ����
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
        //Debug.Log("Player_move: ���� ī��Ʈ �ʱ�ȭ");
    }
    private float CalculateCurrentSpeed()
    {
        float basePlusEquip = speed + statMoveSpeed + equipmentSpeedBonus;
        return basePlusEquip * buffSpeedMultiplier * debuffSpeedMultiplier * speedUP;
    }

    /// <summary>
    /// ���� ��� ���� �ִϸ��̼��� ���� ������ �ӵ��� ������ �����մϴ�.
    /// </summary>
    public void AttackSpeedDownDuringAnimation(float slowRatio = 0.1f)
    {
        // �̹� ���� ���� �ڷ�ƾ�� ������ �ߴ�
        if (_animationSlowRoutine != null)
            StopCoroutine(_animationSlowRoutine);

        // �ӵ� ������ ����
        debuffSpeedMultiplier = slowRatio;

        // ���� ��� ���� �ִϸ��̼� State�� �ؽ� ����
        AnimatorStateInfo stateInfo = sync.CurrentStateInfo;
        int currentStateHash = stateInfo.shortNameHash;

        // �ش� State�� ���� ������ ���� �ڷ�ƾ ����
        _animationSlowRoutine = StartCoroutine(RecoverAfterAnimationEnd(currentStateHash));
    }
    private IEnumerator RecoverAfterAnimationEnd(int stateHash)
    {
        // ù ������ �ǳʶٱ�
        yield return null;

        // ���� State�� ������ �ʾ����� ��� ���
        while (sync.CurrentStateInfo.shortNameHash == stateHash
               && sync.CurrentStateInfo.normalizedTime < 1f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        // �ִϸ��̼� ���� ������ �ӵ� ����
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



