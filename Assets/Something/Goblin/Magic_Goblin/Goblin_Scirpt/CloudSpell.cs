using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CloudSpell : MonoBehaviour
{
    [Tooltip("���� �ִϸ��̼� �̺�Ʈ���� ����� ������ ��")]
    public int damageAmount = 2;

    [Header("�˹� ���� ����")]
    [SerializeField] private float minKnockbackForce = 1f;
    [SerializeField] private float maxKnockbackForce = 3f;
    [SerializeField] private float minKnockbackUpwardForce = 0.5f;
    [SerializeField] private float maxKnockbackUpwardForce = 2f;

    // Ʈ���ſ� ���� ������ ���� ����
    private readonly HashSet<IDamageable> _targetsInRange = new HashSet<IDamageable>();

    // Collider ����
    private Collider2D _col;

    private void Awake()
    {
        _col = GetComponent<Collider2D>();
        if (_col == null)
        {
            Debug.LogError("[CloudSpell] Collider2D�� �ʿ��մϴ�.", this);
            return;
        }
        _col.isTrigger = true;
        _col.enabled = false;           // ��ҿ� ����
        StartCoroutine(AutoDestroy());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_col.enabled) return;

        // IDamageable�� ������ ������Ʈ�� ������ ����
        if (other.TryGetComponent<IDamageable>(out var dmg))
            _targetsInRange.Add(dmg);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_col.enabled) return;
        if (other.TryGetComponent<IDamageable>(out var dmg))
            _targetsInRange.Remove(dmg);
    }

    /// <summary>
    /// �ִϸ��̼� �̺�Ʈ�� ȣ��: �ݶ��̴� Ȱ��ȭ
    /// </summary>
    public void EnableCollider()
    {
        _targetsInRange.Clear();
        _col.enabled = true;
    }

    /// <summary>
    /// �ִϸ��̼� �̺�Ʈ�� ȣ��: �ݶ��̴� ��Ȱ��ȭ
    /// </summary>
    public void DisableCollider()
    {
        _col.enabled = false;
    }

    /// <summary>
    /// �ִϸ��̼� �̺�Ʈ�� ȣ��: ���� �� ��� ��󿡰� �������� �ش�
    /// </summary>
    public void DealDamage()
    {
        if (!_col.enabled) return;

        float kbForce = Random.Range(minKnockbackForce, maxKnockbackForce);
        float kbUp = Random.Range(minKnockbackUpwardForce, maxKnockbackUpwardForce);

        foreach (var target in _targetsInRange)
        {
            target.TakeDamage(damageAmount, transform, kbForce, kbUp);
        }
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
