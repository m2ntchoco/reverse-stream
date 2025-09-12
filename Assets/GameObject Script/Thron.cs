using UnityEngine;

public class Thorn:MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer; // Inspector ���� Enemy ���̾� ����

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                Debug.Log("rktlrktl");
                player.TakeDamage(5, transform, 0f,0f);
            }
        }
        else if ((enemyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            // Ʈ���� �ݶ��̴��� �ڽĿ� �پ����� �� ������ �θ𿡼� ������Ʈ �˻�
            var enemy = other.GetComponentInParent<MonsterHP>();
            if (enemy != null)
            {
                // ��: 10 �����, �˹� 0, 0
                enemy.Getdamage(10);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                Debug.Log("rktlrktl");
                player.TakeDamage(5, transform, 0f, 0f);
            }
        }
        else if ((enemyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            // Ʈ���� �ݶ��̴��� �ڽĿ� �پ����� �� ������ �θ𿡼� ������Ʈ �˻�
            var enemy = other.GetComponentInParent<MonsterHP>();
            if (enemy != null)
            {
                // ��: 10 �����, �˹� 0, 0
                enemy.Getdamage(10);
            }
        }
    }


}
