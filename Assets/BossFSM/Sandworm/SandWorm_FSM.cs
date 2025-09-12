using System.Collections;
using UnityEngine;

public class SandWorm_FSM : MonoBehaviour
{
    public Sandworm_Coroutine coroutine;
    public SandWorm skillManager;
    public Sandworm_Animation ani;
    [Header("")]
    [SerializeField] GameObject steampunk;
    [SerializeField] GameObject mana;
    public Transform target;
    public GameObject obj;
    public GameObject clearTileMap;
    public GameObject ClearCameraRoom;
    public GameObject OffCameraRoom;
    public float searchRadius;

    private bool Steampunk = false;
    private bool Magic = false;
    private bool Select = false;

    [SerializeField] private float damageCooldown = 1.0f;
    private float lastDamageTime = -999f;

    [Header ("health")]
    public static float maxHP;
    [SerializeField]public static float currentHP;
    private bool isStunned = false; 
    [SerializeField] private bool IsDead = false;
    [SerializeField] private bool isGroggy = false;
    private float damageAccumulator = 0;


    [Header("ü�� ��")]
    [SerializeField] private BossUI bossUI;
    [SerializeField] private Canvas healthBarCanvas;
    public void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, searchRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                target = hit.transform;
                //Debug.Log($"[Ÿ�� ȹ��] {target.name} ��ġ: {target.position}");
                return;
            }
        }

        Debug.Log("Talen Ark�� ã�� �� �����ϴ�.");
    }
    void Awake()
    {
        bossUI = GetComponent<BossUI>();
        maxHP = hp.SandWorm;
        currentHP = hp.SandWorm;
        skillManager = GetComponent<SandWorm>();
        FindTarget();
        skillManager.InitSkills();
        Debug.Log("����� ����");
        StartCoroutine(skillManager.SkillLoop());
    }

    // Update is called once per frame
    void Update()
    {
        if (steampunk.gameObject.activeInHierarchy)
        {
            SetSteamPunktype();
        }
        else
        {
            SetMagictype();
        }
    }

    public void Getdamage(float damage) //���� �Ա�.
    {
        if (IsDead || isStunned) return;
        if (Time.time - lastDamageTime < damageCooldown) return;
        lastDamageTime = Time.time;

        float finaldamage = damage;
        currentHP -= damage;
        bossUI?.UpdateBossUI();
        if (isGroggy)
        {
            finaldamage += 5;
        }
        else
        {
            finaldamage -= 5;
        }

        currentHP -= finaldamage;
        damageAccumulator += finaldamage;
        bossUI?.UpdateBossUI();
        if (!isGroggy && damageAccumulator >= 500f)
        {
            StartCoroutine(Groggy());
            return;
        }

        if (currentHP <= 0)
        {
            IsDead = true;
            Destroy(gameObject);
            //DropManager.Instance?.SpawnDrop(transform.position);
            clearTileMap.SetActive(false);
            OffCameraRoom.SetActive(false);
            ClearCameraRoom.SetActive(true);
        }
        Debug.Log(currentHP);
        Debug.Log(damageAccumulator);
        bossUI?.UpdateBossUI();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // �߽��� �� ������Ʈ�� ��ġ, �������� searchRadius
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
    public IEnumerator Groggy()
    {
        
        isGroggy = true;
        
        Debug.Log("[����] ����� �׷α� ����");

        // ��ų ��� ����: skillManager ��Ȱ��ȭ
        skillManager.isCasting = true;
        yield return new WaitForSeconds(5f);
        isGroggy = false;
        damageAccumulator = 0;
        skillManager.isCasting = false;
    }

    public void SetSteamPunktype()
    {
        Steampunk = true;
        target = steampunk.transform;
        obj = steampunk;
        Select = true;
        Debug.Log("������ũ ����");
    }

    public void SetMagictype()
    {
        Magic = true;
        target = mana.transform;
        obj = mana;
        Select = true;
        Debug.Log("�ù�");
    }
}
