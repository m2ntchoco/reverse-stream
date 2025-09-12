using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ScanMaxHP : MonoBehaviour 
{
    public static float detectedMaxHP;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        // ���� ������Ʈ�� MonsterHP ������Ʈ�� �ִ��� Ȯ��
        if (other.TryGetComponent<MonsterHP>(out MonsterHP monster))
        {
            detectedMaxHP = monster.maxHP;
            Debug.Log($"�� ������: {other.name}, �ִ� ü��: {detectedMaxHP}");

        }
        SoulBuffDamageUp.MaxHpDamageUp();


    }
}
