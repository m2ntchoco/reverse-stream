using System;
using System.Collections.Generic;
using UnityEngine;

public class SoulBuffManager
{
    public static bool P_dead = false;
    public static bool IsButtonUnlocked(string buttonId)
    {
        if (PlayerExpManager.PlayerData == null)
        {
            Debug.LogWarning("[BuffManager] PlayerData�� null��");
            return false;
        }

        if (PlayerExpManager.PlayerData.unlockedButtons == null)
        {
            Debug.LogWarning("[BuffManager] unlockedButtons�� null��");
            return false;
        }

        return PlayerExpManager.PlayerData.unlockedButtons.Contains(buttonId);
    }

    private static readonly HashSet<string> statBuffButtons = new()//���Ȱ��� ������ �ϴ� ��ư�� ����
    {
        "attack0",
        "deffence0",
        "deffence4"
    };

    private static bool IsStatBuff(string buttonId)//���� ���� ������ �ϴ� ��ư�� ���� ����
    {
        return statBuffButtons.Contains(buttonId);
    }

    private static Dictionary<string, Action> buffActions = new Dictionary<string, Action>()
    {
        { "attack0", ApplyAttackStatBonus.ApplyAttakStatBonus},
        { "attack1", ApplyAttackStatBonus.MoveSpeedUp },
        { "attack2", ApplyAttackStatBonus.CritChanceUp },
        { "attack3", SoulBuffDamageUp.MaxHpDamageUpReady },
        { "attack4", ApplyAttackStatBonus.CritDamageUP },
        { "attack5", SoulBuffAttack.ButtonUnlockScan },
        { "attack6", AttackSpeed.Attack6Button },
        { "deffence0", DamageDiscount.enemyAttackDis },
        { "deffence1", ApplyDeffenceStatBonus.applyDeffenceStatBonus},
        { "deffence2", HpGenerate.HealCoroutine },
        { "deffence3", NonDamage.PreNonDamage },
        { "deffence4", DamageDiscount.enemyAttackDis},
        { "deffence5", SoulBuffShield.ShieldReadyOn },
        { "deffence6", SoulBuffInvincibility.DButton6Clicked }

    };

    public static void ApplyBuffByButtonId(string buttonId)
    {
        if (P_dead == true)
        {
            // �÷��̾ ���� ���, �رݵ� ��ü ��ư�� ������ ��� ������
            foreach (var id in PlayerExpManager.PlayerData.unlockedButtons)
            {
                if (buffActions.TryGetValue(id, out var act))
                {
                    act.Invoke();  // ���� ����
                    PlayerExpManager.PlayerData.appliedBuffs[id] = true; // ������ ǥ��
                    //Debug.Log($"[BuffManager] ��� �� ������: {id}");
                }
            }

            SaveManager.Instance.SaveNow();   // ����
            PlayerExpSave.SavedExp();         // ����ġ ���� ����

            P_dead = false;  // ������ �Ϸ� �� �ʱ�ȭ
            return;          // �ߺ� ó�� ������ �ٷ� ����
        }
        // �ߺ� ���� ������ üũ
        if (IsStatBuff(buttonId) &&
            PlayerExpManager.PlayerData.appliedBuffs.TryGetValue(buttonId, out bool alreadyApplied) && alreadyApplied)
        {
            Debug.Log($"[����] {buttonId} ȿ���� �̹� ����� (�ߺ� ���� ����)");
            return;
        }

        if (buffActions.TryGetValue(buttonId, out var action))
        {
            if (IsStatBuff(buttonId))
            {
                PlayerExpManager.PlayerData.appliedBuffs[buttonId] = true;
                Debug.Log($"[BuffManager] {buttonId} ȿ�� ���� �� ���� �Ϸ�");
                SaveManager.Instance.SaveNow();  // ����
                PlayerExpSave.SavedExp();        // ����
            }
            action.Invoke();
        }
        else
        {
            Debug.LogWarning($"��ư ID '{buttonId}'�� ����� ȿ���� �����ϴ�.");
        }
        SaveManager.Instance.SaveNow();
        PlayerExpSave.SavedExp();
    }

    public static void ApplyAllUnlockedBuffs()
    {
        foreach (var buttonId in PlayerExpManager.PlayerData.unlockedButtons)
        {
            ApplyBuffByButtonId(buttonId);
            Debug.Log($"[BuffManager] �رݵ� '{buttonId}' ȿ�� ������ �Ϸ�");
            var ui = UnityEngine.Object.FindFirstObjectByType<HealthBarUIController>();
            ui?.RefreshFromPlayerHealth();   // or SetHP(...)
        }
    }
}
