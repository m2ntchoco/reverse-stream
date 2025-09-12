using UnityEngine;

public class BossSpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bossObject; // Ȱ��ȭ�� ���� ������Ʈ

    private bool bossSpawned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!bossSpawned && other.CompareTag("Player"))
        {
            bossObject.SetActive(true);  // ���� ����
            bossSpawned = true;          // �ߺ� ���� ����
            Debug.Log("������ �����Ǿ����ϴ�!");
        }
    }
}
