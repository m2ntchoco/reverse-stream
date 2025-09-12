using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public static class NonDamage
{
    public static bool DefenceBClicked = false;
    public static bool negateNextDamage = false;
    public static bool isNegateReserved = false;
    public static void PreNonDamage()
    {
        DefenceBClicked = true;
    }
    public static IEnumerator InvokeEvery30Seconds()
    {
        while (DefenceBClicked == true)
        {
            if (!isNegateReserved) // ���� ���� �� �� ���¿�����
            {
                isNegateReserved = true; // ���� ������ ǥ��
                yield return new WaitForSeconds(30f);
                negateNextDamage = true;
                Debug.Log("���� �ǰ� ��ȿȭ �����");
            }
            else
            {
                yield return null; // �̹� ����� �� ���
            }
        }
    }
}
