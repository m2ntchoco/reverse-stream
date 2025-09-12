using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class DetectionArea : MonoBehaviour
{
    private EnemyAI enemyAI;

    // �÷��̾��� ���� �ݶ��̴�(��/��/���� ��)�� ��� ����
    private readonly HashSet<Collider2D> _playerOverlaps = new HashSet<Collider2D>();

    [SerializeField] private string playerTag = "Player"; // �ʿ�� ����
    [SerializeField] private bool verboseLog = false;      // ���� �α� ������ ����ġ

    private void Awake()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
        if (enemyAI == null)
            Debug.LogError($"[DetectionArea] �θ� EnemyAI�� �����ϴ�. ({name})");

        // Ʈ���� ����
        var col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
            if (verboseLog) Debug.LogWarning("[DetectionArea] isTrigger�� ���� �־� �ڵ����� �׽��ϴ�.");
        }
    }

    private void OnEnable()
    {
        // ���� �ʱ�ȭ
        _playerOverlaps.Clear();
        SetInRange(false);
    }

    private void OnDisable()
    {
        // ���� ���� Ȯ���� false
        _playerOverlaps.Clear();
        SetInRange(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (_playerOverlaps.Add(other)) // ���� ���� �ݶ��̴��� �ݿ�
        {
            if (_playerOverlaps.Count == 1)
                SetInRange(true);
            if (verboseLog) Debug.Log("�� �÷��̾� ����: Enter");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (_playerOverlaps.Remove(other))
        {
            if (_playerOverlaps.Count == 0)
                SetInRange(false);
            if (verboseLog) Debug.Log("�� �÷��̾� ���� ��Ż: Exit");
        }
    }

    // OnTriggerStay2D�� ����/���� �����̶� ���� ���ʿ�. �ʿ��ϸ� �ּ�ȭ�ؼ� ���.
    // private void OnTriggerStay2D(Collider2D other) { ... }

    private void SetInRange(bool value)
    {
        if (enemyAI == null) return;
        enemyAI.playerInRange = value;
    }
}
