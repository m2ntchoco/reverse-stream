using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Boss1_EnJump : MonoBehaviour 
{
    public Rigidbody2D rb;
    public Boss1_FSM FSM;
    private bool isGrounded;

    private float previousY;
    public float increasedGravityScale = 10f;
    public float gravity = 1f;
    private LayerMask groundLayer;
    private bool isFallingToTarget = false;
    public Vector2 landingPosition;

    [SerializeField] private Transform detect;

    [SerializeField] private float rayOriginOffsetY = 0.1f;
    [SerializeField] private float rayDistance = 1.3f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float defaultGravityScale = 3f;
    [SerializeField] private CircleCollider2D[] jumpAttackAreas;
    [SerializeField] private LayerMask playerLayer;

    public Boss1_Animation ani;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Boss1_Animation>();
        previousY = transform.position.y;
        
    }
    private void Update()
    {
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - rayOriginOffsetY);
        
        // �ٴ��� ���� Ray �߻�
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, groundMask);

        // Scene �信�� ����� Ray ǥ�� (�ʷ�: �ٴ� ����, ����: ����)
        Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, hit.collider != null ? Color.green : Color.red);

        // ��Ҵ��� ���� �Ǵ�
        bool onGround = (hit.collider != null);

        // �߷� ����
        if (onGround)
        {
            rb.gravityScale = defaultGravityScale;
            //if(rb.gravityScale ==  defaultGravityScale)
            //Debug.Log("�⺻ �߷� (Ground�� ��� ����)");
            
        }
        else
        {
            rb.gravityScale = 100f;
            float distanceX = Mathf.Abs(landingPosition.x - transform.position.x);
            Vector3 pos = transform.position;
            float targetX = detect.position.x;
            float targetY = detect.position.y;
            float xDir = Mathf.Sign(landingPosition.x - transform.position.x);
            float airMoveSpeed = 35f;
            if (distanceX <= 1f)
            {
                airMoveSpeed = 5f;
            }
            else if (distanceX <= 5f)
            {
                airMoveSpeed = 20f;
            }
            else if (distanceX < 8f)
            {
                airMoveSpeed = 35f;
            }
            else
            {
                airMoveSpeed = 70f;
            }
            rb.linearVelocity = new Vector2(xDir * airMoveSpeed, rb.linearVelocity.y);
            //Debug.Log("�߷� ���� (Ground�� ���� ����)");
            ani.JumpAttack();

        }

    }

    public void Jump(Transform target, float jumpHeight)
    {
        float gravity = Physics2D.gravity.y * 3f;
        float vy = Mathf.Sqrt(6 * Mathf.Abs(gravity) * jumpHeight);
        int dir = 0;
        landingPosition = target.position;
        float timeToApex = vy / Mathf.Abs(gravity);
        float totalTime = timeToApex * 2;

        FSM.FindTarget();
        Debug.Log("Jump() ȣ���");
        //ani.Jump();
        Rigidbody2D rb = FSM.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(0, vy);

        if (FSM.target == null || rb == null)
        {
            //Debug.LogWarning("target �� null�̶� ���� �� ��");
            return;
        }

        Vector2 startPos = FSM.transform.position;
        Vector2 targetPos = FSM.target.position;
        float vx = (landingPosition.x - startPos.x) / totalTime;
        //Debug.Log($"vx : {vx}");
        float Ivx = (targetPos.x - startPos.x) / totalTime;
        if (Ivx <= 0)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }

        FSM.FaceDirection(dir);

        rb.linearVelocity = new Vector2(vx, vy);
        //Debug.Log("���� ����: " + rb.linearVelocity);
        DealJumpAttackDamage();
    }

    private void DealJumpAttackDamage()
    {
        HashSet<PlayerHealth> damaged = new HashSet<PlayerHealth>();

        foreach (var area in jumpAttackAreas)
        {
            Vector2 center = area.bounds.center;
            float radius = area.radius * area.transform.lossyScale.x;
            Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, playerLayer);

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player") && hit.TryGetComponent(out PlayerHealth player) && !damaged.Contains(player))
                {
                    player.TakeDamage(10, transform, 0f, 0f);
                    damaged.Add(player);
                }
            }
        }
    }
    //-----------------------------------------------------------



    //------------------------------------------------------------
    // ���� ����
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Ground") && !isGrounded)
        {
            Debug.Log("���� Ʈ���� ����: 1ȸ�� �����");
            isGrounded = true;

            // ���⿡ ������ ���� (��: velocity ���� ��)
            rb.linearVelocity = Vector2.zero;
        }

        if (other.CompareTag("Ground"))
        {
            isFallingToTarget = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            // ������ �������� �ٽ� ���� ���� �����ϵ��� �ʱ�ȭ
            isGrounded = false;

        }
    }

    private void OnDrawGizmosSelected() // �ð�ȭ
    {
        // ���信 ����ĳ��Ʈ ���� ǥ��
        Gizmos.color = Color.green;
        Vector2 gizmoOrigin = new Vector2(transform.position.x, transform.position.y - rayOriginOffsetY);
        Gizmos.DrawRay(gizmoOrigin, Vector2.down * rayDistance);
    }
}
