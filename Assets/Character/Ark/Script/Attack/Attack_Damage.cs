using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using System.Security.Cryptography.X509Certificates;

public class Attack_Damage : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private Transform attackPoint;          // �⺻ ���� ���� ��ġ
    [SerializeField] private float attackRange = 0.5f;       // �⺻ ���� �ݰ�
    [SerializeField] private LayerMask enemyLayer;           // �� ���̾� ����
    [SerializeField] public int BasicattackDamage = 30;          // �⺻ ���� ������
    [SerializeField] public int BasicstrongAttackDamage = 100;    // ������(��¡) ������
    [SerializeField] public int BasicstrongUPAttackDamage = 60;    // ������(��¡) ������
    [SerializeField] public int OverHeatBounsDamage = 2;    // ������(��¡) ������
    public static float CriticalDamage = 0;
    public static float CritChance = 0;
    public static float MaxHPDamage = 0;
    public static float SoulBuffCriticalDamage = 0;
    public static float SoulBuffCritChance = 0;
    public static int Critrand;
    private float weaponcirdmg = 0f;
    private float luckstat;

    // ===== ���ο��� ����� ���� =====
    public WeaponStatPackage currentWeaponData;
    private SteamPressureSystem steampunk;

    private void Awake()
    {
        steampunk = GetComponent<SteamPressureSystem>();
        if (gameObject.name == "Ark/mana")
        {
            OverHeatBounsDamage = 1;
        }
    }

    private void Update()
    {
        /*BasicattackDamage += + StrStat;
        BasicstrongAttackDamage += StrStat;
        BasicstrongUPAttackDamage += StrStat;
        Debug.Log($"�ҿ� ������ ���� ũ��Ƽ�� ������ : {SoulBuffCriticalDamage}, �ҿ� ������ ���� ũ��Ƽ�� Ȯ�� : {SoulBuffCritChance}");*/
        luckstat = Ark_stat.luck;
        weaponcirdmg = (currentWeaponData.critDamage / 100) - 1; 
        CritChance = (luckstat * 0.1f) + currentWeaponData.critChance + SoulBuffCritChance; 
        Debug.Log($"���� �������� ���� ������ {currentWeaponData.attackPower}");
    }

    // ���� �޼���
    private void AttackWithModifiers(float baseAttackDamage)
    {
        // 1) ���̽� + ���� + MaxHP ������
        float damage = baseAttackDamage
                     + MaxHPDamage
                     + currentWeaponData.attackPower;

        // 2) ���� ���
        damage *= GetBuffMultiplier();

        // 3) ũ��Ƽ�� ���
        Critrand = UnityEngine.Random.Range(0, 100); //ũ��Ƽ�� Ȯ�� ����� ���� ���� �� �ޱ�
        if(CritChance >= Critrand)
        {
            damage *= 1 + (weaponcirdmg + SoulBuffCriticalDamage);
            Debug.Log("ũ���߻�");
        }

        // 4) ������Ʈ ��� (������Ʈ ������ ����)
        if (gameObject.name == "Ark/SteamPunk")
        {
            if (steampunk.isOverheated)
                damage *= OverHeatBounsDamage;
        }

        // 5) ���� ����
        DoAttack(attackPoint.position, attackRange, damage);
    }
    public void NormalAttack()
    {
        AttackWithModifiers(BasicattackDamage);
    }
    public void DownCommand()
    {
        AttackWithModifiers(BasicstrongAttackDamage);
    }
    public void SideCommand()
    {
        AttackWithModifiers(BasicstrongUPAttackDamage);
    }

    private float GetBuffMultiplier()
    {
        if (TryGetComponent<SoulBuffAttack>(out var buff) && buff.IsBuffActive)
        {
            return buff.BuffMultiplier;
        }
        //Debug.Log("[���� Ȯ��] ���� ������ ���� ��Ȱ��ȭ �� �⺻ ��� 1.0");
        return 1f;
    }
    private void DoAttack(Vector2 point, float range, float damage)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(point, range, enemyLayer);
        foreach (var col in hitEnemies)
        {
            if (col.TryGetComponent<MonsterHP>(out MonsterHP monster))
            {
                //Debug.Log($"�� ������: {damage} ü�� ��� ������ ���� : {MaxHPDamage}");
                Debug.Log("�� ����");
                monster.Getdamage(damage);
                //Debug.Log($"[PerformAttack] ���� ������: {damage}, ���: {GetBuffMultiplier()}");
                Debug.Log(damage);
            }
            else if (col.TryGetComponent<MagicGoblinAI>(out MagicGoblinAI goblin))
            {
                //Debug.Log($"�� ������: {damage} ü�� ��� ������ ���� : {MaxHPDamage}");
                goblin.TakeDamage(damage);
                Debug.Log($"[PerformAttack] ���� ������: {damage}, ���: {GetBuffMultiplier()}");
                Debug.Log(damage);
            }
            else if (col.TryGetComponent<SandWorm_FSM>(out SandWorm_FSM Sandworm))
            {
                Debug.Log("����� ����");
                Sandworm.Getdamage(damage);
                
                Debug.Log(damage);
            }
            else if (col.TryGetComponent<Boss1_FSM>(out Boss1_FSM boss1))
            {
                Debug.Log("����� ����");
                boss1.Getdamage(damage);
                Debug.Log(damage);
            }

        }
    }

    public void SetWeaponStats(WeaponStatPackage stats)
    {
        currentWeaponData = stats;
    }

}
