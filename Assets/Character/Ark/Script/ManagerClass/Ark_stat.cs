using System.Collections;
using UnityEngine;

/// <summary>
/// �÷��̾��� ������ �����ϰ� ������ �� �ִ� Ŭ����.
/// �� Ŭ������ ���� ������ ���·� ����� ���� [System.Serializable]�� ����Ѵ�.
/// </summary>
[System.Serializable]
public static class Ark_stat
{
    public static int health;
    public static int strength;
    public static int dexterity;
    public static int intelligence;
    public static int luck;
    public static int remainingStatPoints;
    //����
    public static float BaseAttackSpeed;
    private static float attackSpeedMultiplier = 1.0f;


    /// <summary>
    /// ����� �����͸� �޾Ƽ� ���� ����(Ark_stat) ��ü�� ��ȯ�ϴ� ������
    /// </summary>
    /// <param name="data">�ҷ��� statSaveData ��ü</param>

    public static void LoadFrom(statSaveData data)
    {

        health = data.health;
        strength = data.strength;
        dexterity = data.dexterity;
        intelligence = data.intelligence;
        remainingStatPoints = data.remainingStatPoints;

    }

    /// <summary>
    /// ���� ���� ������ ���� ������ ���·� ��ȯ
    /// </summary>
    /// <returns> statSaveData�� ��ȯ�� ���</returns>
    public static void ApplyTo(statSaveData data)
    {
        data.health = health;
        data.strength = strength;
        data.dexterity = dexterity;
        data.intelligence = intelligence;
        data.remainingStatPoints = remainingStatPoints;
    }

    public static void ResetStats()
    {
        // ��� ������ 0���� �ʱ�ȭ�ϰų� �⺻������ ����
        health = 0;
        strength = 0;
        dexterity = 0;
        intelligence = 0;
        remainingStatPoints = 20;

        Debug.Log("�÷��̾� ������ �ʱ�ȭ�Ǿ����ϴ�.");
    }

    public static bool MeetsRequirement(WeaponPrefabData data)
    {
        return health >= data.requiredHp &&
                strength >= data.requiredStr &&
                dexterity >= data.requiredDex &&
                intelligence >= data.requiredInt &&
                luck >= data.requiredluk;

    }

    /// <summary>
    /// Ư�� ������ 1��ŭ ������Ű��, ���� ���� ����Ʈ�� 1 ���ҽ�Ŵ
    /// ���� ���� ����Ʈ�� ���ٸ� false�� ��ȯ�ϰ� �ƹ� �۾��� ���� ����.
    /// </summary>
    /// <param name="stat">����(ref)�� ���޵� ���� ��</param>
    /// <returns>������ ���� �Ǿ����� ����</returns>
    public enum StatType { Health, Strength, Dexterity, Intelligence, Luck }

    public static bool IncreaseStat(StatType type)
    {
        if (remainingStatPoints <= 0) return false;

        switch (type)
        {
            case StatType.Health: health++; break;
            case StatType.Strength: strength++; break;
            case StatType.Intelligence: intelligence++; break;
            case StatType.Dexterity: dexterity++; break;
            case StatType.Luck: luck++; break;
        }

        remainingStatPoints--;

        SaveManager.Instance.SaveNow();
        return true;
    }
    public static float GetAttackSpeed()
    {
        return attackSpeedMultiplier;
    }
    public static void SetAttackSpeed(float multiplier)
    {
        attackSpeedMultiplier = multiplier;
    }
    public static void ResetAttackSpeed()
    {
        attackSpeedMultiplier = 1.0f;
    }
}
