using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum WeaponGrade { Common, Rare, Epic, Unique, Legendary }
public enum WeaponType { GreatSword, OneHandSword, Spear, Hammer, Dagger, Chain, Gauntlet, Bow, Staff }
[System.Serializable]
public class WeaponPrefabData
{
    [Header("�⺻ ����")]
    public string weaponName;
    public WeaponGrade grade;     // ���
    public WeaponType type;       // ���� ����
    public int level;             // ���� ���� (10, 20, 30, 40, 50)
    public Sprite icon;           // ���� ������
    public string description;    // ���� ����

    [Header("���� ����")]
    public int price;
    public int sellprice;

    [Header("�䱸 ����")]
    public int requiredHp;
    public int requiredStr;
    public int requiredDex;
    public int requiredInt;
    public int requiredluk;

    [Header("���� �Ӽ�")]
    public float attackPower;     // ���� or ���� ���ݷ�
    public float attackSpeed;     // ���� �ӵ�
    public float critChance;      // ũ��Ƽ�� Ȯ�� (%)
    public float critDamage;      // ũ��Ƽ�� ��� (%)
    public float range;           // ��Ÿ�
}
