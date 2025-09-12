using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class AttackSpeed : MonoBehaviour
{
    private static int _AttackCount;
    private const int maxAttackCount = 5;
    public static bool attack6Button = false;
    private static Coroutine resetCoroutine;
    private static MonoBehaviour coroutineRunner;  // �ܺο��� �־��� ��
    private const float resetDelay = 3f;

    private void Update()
    {
        Debug.Log($"���� �ӵ�: {Ark_stat.GetAttackSpeed()}");
    }
    public static int AttackCount
    {
        get => _AttackCount;
        set => _AttackCount = Mathf.Clamp(value, 0, maxAttackCount);
    }

    public static void Attack6Button()
    {
        attack6Button = true;
    }
    public static void RegisterRunner(MonoBehaviour runner)
    {
        coroutineRunner = runner;
    }
    public static void AttackSpeedUP()
    {
        if (attack6Button && (SteamPunk_Attack.AttackCountReady || Magic_Attack.AttackCountReady))
        {
            AttackCount++; // �ڵ����� Clamp �����
            Debug.Log($"[���� ����] ���� ��: {AttackCount}/{maxAttackCount}");
            SteamPunk_Attack.AttackCountReady = false;
            Magic_Attack.AttackCountReady = false;

            RestartResetTimer();
        }
    }

    private static void RestartResetTimer()
    {
        if (resetCoroutine != null && coroutineRunner != null)
        {
            coroutineRunner.StopCoroutine(resetCoroutine);
        }

        if (coroutineRunner != null)
        {
            resetCoroutine = coroutineRunner.StartCoroutine(ResetAfterDelay());
        }
    }

    private static IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        AttackCount = 0;
        Debug.Log("[���� ����] 3�� ���� ���� ��� ���� �ʱ�ȭ��");
    }

    public static void SpeedUPSize()
    {
        if(AttackCount == 0)
        {
            Ark_stat.SetAttackSpeed(1.1f);
            //Debug.Log($"���� ������");
        }
        else if(AttackCount == 1)
        {
            Ark_stat.SetAttackSpeed(1.2f);
            //Debug.Log($"���� ������");

        }
        else if (AttackCount == 2)
        {
            Ark_stat.SetAttackSpeed(1.3f);
            //Debug.Log($"���� ������");

        }
        else if (AttackCount == 3)
        {
            Ark_stat.SetAttackSpeed(1.4f);
            //Debug.Log($"���� ������");

        }
        else if (AttackCount == 4)
        {
            Ark_stat.SetAttackSpeed(1.5f);
            //Debug.Log($"���� ������");

        }
        else if (AttackCount == 5)
        {
            Ark_stat.SetAttackSpeed(1.6f);
            //Debug.Log($"���� ������");

        }
    }
}