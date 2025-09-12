using UnityEngine;
using System.Collections;

public class SoulBuffAttack : MonoBehaviour
{
    private bool isBuffReady = false;
    private bool isBuffOnCooldown = false;
    public static bool isA5ButtonUnlocked = false;

    [SerializeField] private float buffMultiplier = 2f;
    [SerializeField] private float buffDuration = 3f;
    [SerializeField] private float buffCooldown = 10f;

    public bool IsBuffActive { get; private set; } = false;
    public float BuffMultiplier => buffMultiplier;

    public static void ButtonUnlockScan()
    {
        isA5ButtonUnlocked = true;
    }

    // [1] �ǰ� �� ���� �غ�
    public void TryActivateBuffAfterHit()
    {
        if (isA5ButtonUnlocked && !isBuffOnCooldown)
        {
            isBuffReady = true;
            Debug.Log("�ǰݵ� �� ���� ���� ���� �غ� �Ϸ�");
            
        }
    }

    // [2] ���� �� ���� �ߵ�
    public bool TryBuffAttack()
    {
        if (isBuffReady && !isBuffOnCooldown)
        {
            StartCoroutine(BuffRoutine());
            return true;
        }
        return false;
    }

    // [3] ���� Ȱ��ȭ �� ��Ÿ�� ó��
    private IEnumerator BuffRoutine()
    {
        IsBuffActive = true;
        isBuffReady = false;

        Debug.Log("���� ���� �� 3�� ����");

        yield return new WaitForSeconds(buffDuration);

        IsBuffActive = false;
        isBuffOnCooldown = true;

        Debug.Log("���� ���� �� 10�� ��Ÿ�� ����");

        yield return new WaitForSeconds(buffCooldown); // ���⼭ ��ü 10�� ����
        isBuffOnCooldown = false;

        Debug.Log("��Ÿ�� ���� �� �ٽ� �ǰ� �� �ߵ� ����");
    }
}
