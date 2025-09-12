/*using UnityEngine;
using System.Collections;
using TMPro;
using NUnit.Framework.Interfaces;
using UnityEngine.InputSystem.XR.Haptics;

public class Wolf : MonoBehaviour
{


    [Header("HP ����")]
    [SerializeField] private float currentHP;
    [SerializeField] private bool IsDeath = false;

    [Header("�ǰ� ��Ÿ��")]
    private float damageCooldown = 0.3f;
    private float lastDamageTime = -999f;

    [SerializeField] private EnemyAI enemy; // �ν����� ����� (�Ǵ� GetComponent�� �� ���� ����)
    /*[Header("UI")]
    [SerializeField] private TextMeshProUGUI gameOverText;

    private Animator animator;
    private Rigidbody2D rb;

    private bool isStunned = false;

    public bool isDie = false;

    void Start()
    {
        if (enemy == null)
        {
            Debug.LogWarning(" enemy�� �ν����Ϳ� ������� �ʾҽ��ϴ�. GetComponent �õ�");
            enemy = GetComponent<EnemyAI>();
        }

        if (enemy == null)
        {
            Debug.LogError(" EnemyAI ���� ���� - �ൿ �Ҵ� �����Դϴ�.");
            return;
        }

        animator = enemy.GetComponent<Animator>();
        if (animator == null) Debug.LogError(" Animator ���� ����");

        rb = enemy.GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError(" Rigidbody2D ���� ����");

        currentHP = enemy.maxHP;
        animator = enemy.GetComponent<Animator>();
        rb = enemy.GetComponent<Rigidbody2D>();

        if (enemy == null)
        {
            enemy = GetComponent<EnemyAI>();
            if (enemy == null)
                Debug.LogError("EnemyAI ������Ʈ�� ã�� �� �����ϴ�.");
        }


    }

    private void FixDeathAnimation()
    {
        animator.SetTrigger("IsDeath"); // ���� ���� ����
        Destroy(gameObject, 3.0f);
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.simulated = false;
        }

        // �浹 ����
        foreach (var col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }

        // ��� ���� ������Ʈ ��Ȱ��ȭ (��: AI, ����, ���� ��)
        if (TryGetComponent<EnemyAI>(out var ai)) ai.enabled = false;
        if (TryGetComponent<AttackableArea>(out var atk)) atk.enabled = false;
        if (TryGetComponent<DetectionArea>(out var det)) det.enabled = false;
        if (TryGetComponent<Wolf>(out var hp)) hp.enabled = false;
    }
    
    public void 
        (float damage)
    {
        
        if (canStun())
        {
            stun();
        }
        enemy.SetDetectionRadius(100f);
        Debug.Log($"������ ���� : ");
        if (IsDeath || isStunned) return;
        if (Time.time - lastDamageTime < damageCooldown) return;
        enemy.SetIsHit(true);
        animator.SetTrigger("IsAttacked");
        lastDamageTime = Time.time;
        currentHP -= damage;
        enemy.SetIsHit(false);
        Debug.Log($"wolf�� ���� ü�� : {currentHP}");
        //animator.SetTrigger("Hurt");

        if (currentHP <= 0)
        {
            IsDeath = true;
            enemy.IsDeath = true;
            Debug.Log("���� ����");
            enemy.Die();
            isDie = true;
            FixDeathAnimation();

        }
        
    }
    
    bool canStun()
    {
        return Time.time >= lastDamageTime + damageCooldown;
    }

    void stun()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void SetAttackrangeRadius(float radius)
    {
        Transform AttackableArea = transform.Find("AttackableArea");
        if (AttackableArea == null)
        {
            Debug.LogWarning("AttackableArea ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        CircleCollider2D collider = AttackableArea.GetComponent<CircleCollider2D>();
        if (collider == null)
        {
            Debug.LogWarning("AttackableArea�� CircleCollider2D�� �����ϴ�.");
            return;
        }

    }

}*/
