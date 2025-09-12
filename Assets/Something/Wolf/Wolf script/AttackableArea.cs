using UnityEngine;

public class AttackableArea : MonoBehaviour
{
    private EnemyAI enemyAI;

    private void Awake()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
        if (enemyAI == null)
            Debug.LogError("Goblin���� EnemyAI�� ã�� �� �����ϴ�! ������Ʈ: " + gameObject.name);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enemyAI == null) return;

        // �÷��̾� ���̾� + �±� �� �� Ȯ��
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") &&
            other.CompareTag("Player"))
        {
            enemyAI.playerAttackable = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (enemyAI == null) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player") &&
            other.CompareTag("Player"))
        {
            enemyAI.playerAttackable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") &&
            other.CompareTag("Player"))
        {
            enemyAI.playerAttackable = false;
        }
    }
}
