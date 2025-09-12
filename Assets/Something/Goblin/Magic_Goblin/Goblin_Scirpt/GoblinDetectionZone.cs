using UnityEngine;

public class GoblinDetectionZone : MonoBehaviour
{
    private MagicGoblinAI goblinAI;

    void Awake()
    {
        // �θ� ������Ʈ���� AI ��ũ��Ʈ�� �ڵ����� ã�´�
        goblinAI = GetComponentInParent<MagicGoblinAI>();
        if (goblinAI == null)
            Debug.LogError("�θ� MagicGoblinAI�� �����ϴ�!", this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            goblinAI.OnPlayerDetected(other.transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            goblinAI.OnPlayerLost(other.transform);
    }
}
