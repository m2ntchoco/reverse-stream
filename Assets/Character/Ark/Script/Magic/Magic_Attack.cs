using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
// using UnityEditor;   // ��Ÿ�� ��ũ��Ʈ���� ���ʿ�. (���� ���� ����)
using Unity.VisualScripting;

public class Magic_Attack : MonoBehaviour
{
    public float requiredHoldTime = 0.35f; // ��¡ ������ ��¡ �ð�

    // Attack
    public float m_timeSinceAttack = 0.0f;
    public int m_currentAttack = 0;
    public float BonusDamage = 1f;

    // speed
    public float SpeedMulti = 1.2f;
    public static bool AttackCountReady = false; // ������ �ߵ��ߴ��� Ȯ���ϴ� bool��

    [Header("��ų")]
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private Transform slashSpawnPoint;
    [SerializeField] private float slashSpeed = 15f;
    [SerializeField] private bool destroySlashAtEnd = false; // �ڷ�ƾ ���� �� ������ ���� �ɼ�

    // ����
    private ManaSystem manaSystem;
    private PlayerSkill skill;
    private PlayerAnimationSync sync;
    private Player_move move;

    void Awake()
    {
        manaSystem = GetComponent<ManaSystem>();
        sync = GetComponentInParent<PlayerAnimationSync>();
        move = GetComponentInParent<Player_move>();
        skill = GetComponentInParent<PlayerSkill>();
        AttackSpeed.RegisterRunner(this);  // ���� ��ü ���
    }

    private void Update()
    {
        // ���� [�ִϸ��̼� ���] ���� ���� ���(Attack1/Attack2)�� ������ ������ ��� �Է� ����
        var state = sync.CurrentStateInfo;
        bool inAttackAnim = (state.IsName("Attack 1") || state.IsName("Attack 2")
            || state.IsName("Down_Command") || state.IsName("Side_Command"));
        if (inAttackAnim && state.normalizedTime < 1f)
            return;

        // 1) �⺻ Ÿ�̸� ������Ʈ
        m_timeSinceAttack += Time.deltaTime;

        // 3) �Ϲ� �޺� �Է� ó�� (Ŀ�ǵ� ������ �ƴ� ���� A Ű�� ����)
        if (Input.GetKeyDown(KeyCode.A) && m_timeSinceAttack > 0.1f)
        {
            AttackCountReady = true;
            AttackSpeed.AttackSpeedUP();
            AttackSpeed.SpeedUPSize();

            if (TryGetComponent<SoulBuffAttack>(out var buff))
            {
                buff.TryBuffAttack(); // ���� ���� ����
            }

            m_currentAttack++;
            BonusDamage = 1f; // �� ������ ������Ʈ �����ϱ� ��� 1��

            // �޺� ���� �ð� �ʰ� �� �ʱ�ȭ
            if (m_timeSinceAttack > 2.0f)
                m_currentAttack = 1;

            // 3�ܰ� �Ѿ�� 1�ܰ�� ��ȯ
            if (m_currentAttack > 2)
                m_currentAttack = 1;

            // �ִϸ����� Ʈ���� �ߵ�
            move.AttackSpeedDownDuringAnimation(0.3f);
            sync.ApplyAttackSpeed();
            sync.NomalAttack(m_currentAttack);
            m_timeSinceAttack = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            manaSystem.Q_Skill();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            manaSystem.W_Skill();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            manaSystem.E_Skill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("��������");
            Ark_stat.SetAttackSpeed(SpeedMulti); // ���� 20% ����
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("���� �ʱ�ȭ");
            Ark_stat.ResetAttackSpeed();
        }
    }

    public void UseSlashSkill()
    {
        if (slashPrefab == null || slashSpawnPoint == null)
        {
            Debug.LogWarning("[UseSlashSkill] slashPrefab �Ǵ� slashSpawnPoint�� ������ϴ�.");
            return;
        }

        // 1) Player_move ������Ʈ�� ���� �÷��̾� ��Ʈ�� ������ X�� �����ɴϴ�.
        float playerScaleX = move != null ? move.transform.localScale.x : transform.localScale.x;
        int facingDir = playerScaleX > 0f ? 1 : -1;

        // 2) �˱� �ν��Ͻ�ȭ
        GameObject slash = Instantiate(slashPrefab, slashSpawnPoint.position, Quaternion.identity);

        // 3) SpriteRenderer�� ������ flipX�� �¿� ����
        var sr = slash.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.flipX = facingDir < 0;

        // 5) �̵� ���� ����
        Vector2 dir = Vector2.right * facingDir;

        // 6) �ڷ�ƾ���� �̵� (�ν��Ͻ����� duration �б�)
        float dur = 0.3f; // �⺻��
        var fx = slash.GetComponent<SlashEffect>();
        if (fx != null) dur = fx.duration;

        StartCoroutine(MoveSlashManual(slash.transform, dir, dur, destroySlashAtEnd));
    }

    private IEnumerator MoveSlashManual(Transform t, Vector2 dir, float duration, bool destroyWhenDone)
    {
        // ���� �������� �̹� �ı�/������ ��� ����
        if (t == null) yield break;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // �� ������ �ı�/���� üũ �� MissingReferenceException ����
            if (t == null) yield break;

            t.position += (Vector3)(dir * slashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // ���� �� ������ ���� �ɼ�
        if (destroyWhenDone && t != null)
        {
            var go = t.gameObject;
            if (go != null)
                Destroy(go);
        }
    }

    private void OnDisable()
    {
        // �� ��ũ��Ʈ�� ��Ȱ��ȭ�� �� ���� ����
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        // ������Ʈ �ı� �� �ڷ�ƾ ���� ����
        StopAllCoroutines();
    }
}
