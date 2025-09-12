using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어의 스탯을 저장하고 조작할 수 있는 클래스.
/// 이 클래스는 저장 가능한 형태로 만들기 위해 [System.Serializable]을 사용한다.
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
    //공속
    public static float BaseAttackSpeed;
    private static float attackSpeedMultiplier = 1.0f;


    /// <summary>
    /// 저장된 데이터를 받아서 현재 스탯(Ark_stat) 객체로 변환하는 생성자
    /// </summary>
    /// <param name="data">불러온 statSaveData 객체</param>

    public static void LoadFrom(statSaveData data)
    {

        health = data.health;
        strength = data.strength;
        dexterity = data.dexterity;
        intelligence = data.intelligence;
        remainingStatPoints = data.remainingStatPoints;

    }

    /// <summary>
    /// 현재 스탯 정보를 저장 가능한 형태로 변환
    /// </summary>
    /// <returns> statSaveData로 변환된 결과</returns>
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
        // 모든 스탯을 0으로 초기화하거나 기본값으로 설정
        health = 0;
        strength = 0;
        dexterity = 0;
        intelligence = 0;
        remainingStatPoints = 20;

        Debug.Log("플레이어 스탯이 초기화되었습니다.");
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
    /// 특정 스탯을 1만큼 증가시키고, 남은 스탯 포인트를 1 감소시킴
    /// 만약 남은 포인트가 없다면 false를 반환하고 아무 작없도 하지 않음.
    /// </summary>
    /// <param name="stat">참조(ref)로 전달된 스탯 값</param>
    /// <returns>스탯이 증가 되었는지 여부</returns>
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
