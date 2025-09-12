using System;
using System.Collections.Generic;
using UnityEngine;

public class ApplyAttackStatBonus
{
    
    public static void ApplyAttakStatBonus()
    {
        if (PlayerExpManager.PlayerData.appliedBuffs.TryGetValue("attack0", out var done) && done) return;
        Debug.Log("[ApplyAttakStatBonus] �Լ� ȣ���");
        if (SoulBuffManager.IsButtonUnlocked("attack0"))
        {
            Debug.Log("attack0�� ������ �����");
            Ark_stat.strength += 1;
            Debug.Log($"Strength ������ �����Ͽ����ϴ� Strength{Ark_stat.strength}");
            SaveManager.Instance.SaveNow();
        }
    }

    public static void MoveSpeedUp()
    {
        Debug.Log("[MoveSpeedUp] �Լ� ȣ���");
        if (SoulBuffManager.IsButtonUnlocked("attack1"))
        {
            Debug.Log("attack1�� ������ �����");
            
            Player_move.speedUP += 0.1f;
        }
    }

    public static void CritChanceUp()
    {
        Debug.Log("[CritChanceUp] �Լ� ȣ���");
        if (SoulBuffManager.IsButtonUnlocked("attack2"))
        {
            Debug.Log("attack2�� ������ �����");

            Attack_Damage.SoulBuffCritChance = 20;
        }
        

    }

    public static void CritDamageUP()
    {
        Debug.Log("[CritDamageUp] �Լ� ȣ���");
        if (SoulBuffManager.IsButtonUnlocked("attack4"))
        {
            Debug.Log("attack4�� ������ �����");

            Attack_Damage.SoulBuffCriticalDamage = 0.2f;
        }
    }
}
