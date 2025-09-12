using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class SoulBuffShield
{
    public static bool shieldReady = false;
    public static bool shieldCanUse = false;
    public static bool DefenceSClicked = false;
    
    public static void ShieldReadyOn()
    {
        DefenceSClicked = true;
    }
    public static IEnumerator ShieldEvery30Seconds()
    {
        while (DefenceSClicked == true)
        {
            if (!shieldReady) // ���� ���� �� �� ���¿�����
            {
                shieldReady = true; // ���� ������ ǥ��
                yield return new WaitForSeconds(30f);
                shieldCanUse = true;
                Debug.Log("���� �ߵ���");
            }
            else
            {
                yield return null; // �̹� ����� �� ���
            }
        }
    }
}
