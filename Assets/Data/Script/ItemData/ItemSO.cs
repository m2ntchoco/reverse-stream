using UnityEngine;

public enum ItemType { Weapon, Armor, Accessory }

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    [Header("�⺻ ����")]
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    [TextArea]
    public string description;

    [Header("���� �ɷ�ġ")]
    public int attackPower;
    public float attackSpeed;
    public int defense;
    public int magicPower;
    public float criticalChance;
    public float cooldownReduction;

    [Header("��Ÿȿ��")]
    public int hpBonus;
    public int mpBonus;
}
