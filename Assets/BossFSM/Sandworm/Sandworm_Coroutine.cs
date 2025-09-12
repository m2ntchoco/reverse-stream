using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System;

public class Sandworm_Coroutine : MonoBehaviour
{
    public Sandworm_Animation ani;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public int dirx = 0;
    private bool ye;

    public Transform steampunktarget;    // �� ������
    public Transform magictarget;
    private Vector3 target;

    [SerializeField] private LayerMask playerLayer;

    [Header("Attack")]
    public Transform attack;
    public GameObject WarningNormal;

    [Header("Jump")]
    [SerializeField] public Transform jump;
    [SerializeField] private Vector2 landingPosition;
    [SerializeField] private List<Collider2D> jumpAttackAreas;
    [SerializeField] private GameObject WarningObjectJump;
    [SerializeField] private GameObject halfjumpOut;
    private bool End;
    [Header("JumpOut")]
    [SerializeField] public Transform jumpOut;
    [SerializeField] public GameObject isGround;
    [SerializeField] public float rayDistance = 0f;
    [SerializeField] public float rayOriginOffsetY = 0.5f;
    [SerializeField] public LayerMask groundMask;
    [SerializeField] public BoxCollider2D JumpDealArea;
    private int currentJumpCount;

    [Header("Spit")]
    [SerializeField] private GameObject[] WarningObjectSpit;
    public GameObject spit;
    public Transform spitPoint;

    private bool Steampunk = false;
    private bool Magic = false;
    private bool Select = false;

    private bool bossOnOff = false;
    private Coroutine jumpBoxDamageCoroutine;
    //private HashSet<PlayerHealth> damagedInBox = new HashSet<PlayerHealth>();
    [SerializeField] private PlayerHealth playerhealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        ani = GetComponent<Sandworm_Animation>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();


    }
    void Start()
    {

    }
    // Update is called once per frame
    private void OnEnable()
    {
        StartCoroutine(FindPlayerTargets());
    }

    private IEnumerator FindPlayerTargets()
    {
        GameObject player = null;

        // �÷��̾ ������ ������ ���
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            yield return null; // �� ������ ���
        }

        // �ڽ� ������Ʈ �ٷ� ã�� (�̸� ���)
        foreach (Transform child in player.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "Ark/SteamPunk") steampunktarget = child;
            if (child.name == "Ark/Mana") magictarget = child;
        }

        Debug.Log($"Ÿ�� ã�� �Ϸ�: {steampunktarget?.name}, {magictarget?.name}");
    }

    void Update()
     {
        if (steampunktarget.gameObject.activeInHierarchy)
        {
            SetSteamPunktype();
        }
        else
        {
            SetMagictype();
        }
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - rayOriginOffsetY);

        // �ٴ��� ���� Ray �߻�
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, groundMask);

        // Scene �信�� ����� Ray ǥ�� (�ʷ�: �ٴ� ����, ����: ����)
        Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, hit.collider != null ? Color.green : Color.red);
        if (ye)
        {
            int dir = 0;
            Vector2 startPos = transform.position;
            Vector2 targetPos = target;

            float Ivx = (targetPos.x - startPos.x);
            if (Ivx <= 0)
            {
                dir = -1;
            }
            else
            {
                dir = 1;
            }

            dirx = dir;
            float dx = targetPos.x - transform.position.x;
            if (Mathf.Abs(dx) > 2.5f) // ���� �Ÿ� �̻��� ���� ���� ����
            {
                dirx = dx > 0 ? 1 : -1;

                // �¿� ����
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * -dirx;
                transform.localScale = scale;
            }
        }

        // ��Ҵ��� ���� �Ǵ�
        bool onGround = (hit.collider != null);
        if (onGround)
        {
        }
        else
        {
            ani.JumpRoll();
        }

        if (End)
        {
            halfjumpOut.transform.position = target;
        }
     }

    //---------------------------------------------------------------------
    //          % �ֿ� ��ų %
    //---------------------------------------------------------------------
    public IEnumerator Jump(Transform player, float jumpHeight)
    {
        ye = false;
        GetComponent<SpriteRenderer>().enabled = true;
        GameObject obj = WarningObjectJump;


        if (obj != null) obj.SetActive(false);

        Debug.Log($"Jump �÷��̾� ������{player.position}");
        //ani.HalfJump();
        float vy = Mathf.Sqrt(6 * jumpHeight);
        int dir = 0;
        landingPosition = player.position;
        float timeToApex = vy;
        float totalTime = timeToApex * 2;
        Vector2 startPos = transform.position;
        Vector2 targetPos = target;
        float Ivx = targetPos.x - startPos.x;
        if (Ivx <= 0)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
        dirx = dir;
        float vx = Mathf.Abs(landingPosition.x - startPos.x) / 4 * dir;
        Debug.Log($"vx : {vx}");
        Debug.Log(player.transform.position);
        float dx = targetPos.x - transform.position.x;
        if (Mathf.Abs(dx) > 1.1f) // ���� �Ÿ� �̻��� ���� ���� ����
        {
            dirx = dx > 0 ? 1 : -1;

            // �¿� ����
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * -dirx;
            transform.localScale = scale;
        }
        //Debug.Log(vx);
        //Debug.Log(vy);
        if (dir == 0) yield return null;  // ���� ���¿����� ���� ����
        if (obj != null) obj.SetActive(true);
        ani.HalfJump();
        yield return new WaitForSeconds(2f);
        obj.SetActive(false);

        //Debug.Log("���� ����: " + rb.linearVelocity);
        yield return new WaitForSeconds(0.1f);
        halfjumpOut.SetActive(true);
        Debug.Log("�̰� ��");
        End = true;
        yield return new WaitForSeconds(1.3f);
        End = false;
        transform.position = target;
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + 2f,
            transform.position.z
        );
        yield return new WaitForSeconds(0.1f);
        halfjumpOut.SetActive(false);
        ani.Jumpoo();
        GetComponent<SpriteRenderer>().enabled = true;
        ye = true;
    }

    public IEnumerator XJump(Transform player, float jumpHeight)
    {
        ye = false;
        Debug.Log("������");
        GameObject obj = WarningObjectJump;

        if (obj != null) obj.SetActive(false);

        Debug.Log($"Jump �÷��̾� ������{target}");
        //ani.HalfJump2();
        float vy = Mathf.Sqrt(6 * jumpHeight);
        int dir = 0;
        landingPosition = target;
        float timeToApex = vy;
        float totalTime = timeToApex * 2;
        Vector2 startPos = transform.position;
        Vector2 targetPos = target;
        float Ivx = (targetPos.x - startPos.x) / totalTime;
        if (Ivx <= 0)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }

        dirx = dir;
        Debug.Log(dir);
        float dx = targetPos.x - transform.position.x;
        if (Mathf.Abs(dx) > 0.4f) // ���� �Ÿ� �̻��� ���� ���� ����
        {
            dirx = dx > 0 ? 1 : -1;

            // �¿� ����
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * -dirx;
            transform.localScale = scale;
        }
        if (obj != null) obj.SetActive(true);
        ani.HalfJump2();
        yield return new WaitForSeconds(2f);
        obj.SetActive(false);
        //Debug.Log(vx);
        //Debug.Log(vy);
        if (dir == 0) yield return null;  // ���� ���¿����� ���� ����
        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().enabled = true;
        ye = true;
    }
    public IEnumerator fuckthatshit()
    {
        //Debug.Log($"������ ��ġ : {transform.position}");
        //Debug.Log($"dirx�� : {dirx}");
        transform.position = new Vector3((5f * dirx) + transform.position.x, transform.position.y, transform.position.z);
        yield return null;
    }
    public IEnumerator NAttack()
    {
        ye = true;
        WarningNormal.SetActive(true);
        int dir;
        Vector2 startPos = transform.position;
        Vector2 targetPos = target;
        float Ivx = targetPos.x - startPos.x;
        if (Ivx <= 0)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
        dirx = dir;
        Debug.Log(dir);
        if (dir == 0) yield return null;  // ���� ���¿����� ���� ����
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;  // dir�� -1�̸� ����, 1�̸� ������

        ani.AttackPrepare();
        WarningNormal.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        ye = false;
        yield return new WaitForSeconds(1.3f);
        ani.Attack();
        yield return new WaitForSeconds(0.01f);
    }
    public IEnumerator JumpOut(Transform player, float jumpHeight)
    {
        ye = true;
        //Debug.Log("�Լ� ����.");
        StartCoroutine(XJump(player, 0f));
        yield return new WaitForSeconds(1.4f);
        StartCoroutine(fuckthatshit());
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(XJump(player, 0f));
        yield return new WaitForSeconds(1.4f);
        StartCoroutine(fuckthatshit());
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Jump(player, 0f));
        //Debug.Log("�Լ� ������ ����");
        ye = true;
    }
    public IEnumerator Spit()
    {
        ye = true;
        int dir;
        Vector2 startPos = transform.position;
        Vector2 targetPos = target;
        float Ivx = targetPos.x - startPos.x;
        if (Ivx <= 0)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
        dirx = dir;
        Debug.Log(dir);
        if (dir == 0) yield return null;  // ���� ���¿����� ���� ����
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -dir;  // dir�� -1�̸� ����, 1�̸� ������
        ye = false;
        StartCoroutine(WarningSpit());
        yield return new WaitForSeconds(1.8f);
        transform.localScale = scale;
        ani.Spit();
        yield return new WaitForSeconds(0.8f);
        FireBullet();
        yield return new WaitForSeconds(0.4f);
        FireBullet2();
        yield return new WaitForSeconds(0.4f);
        FireBullet3();
        yield return new WaitForSeconds(1f);
        ani.SpitEnd();
    }

    //---------------------------------------------------------------------
    //          % ��ų ���� �Լ� %
    //---------------------------------------------------------------------
    public IEnumerator WarningSpit()
    {
        float groundY = 0f;
        Vector2 startPos = spitPoint.position;
        Vector2 force = new Vector2(1f * dirx, 10f);
        float gravity = Physics2D.gravity.y;

        // 1. �ٴ� ��ġ ���� ����
        RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, 100f, groundMask);
        if (hit.collider != null)
        {
            groundY = hit.point.y;
        }

        // 2. �꼺ħ �߻�
        if (WarningObjectSpit != null && WarningObjectSpit.Length > 0 && spitPoint != null)
        {
            GameObject bullet = Instantiate(WarningObjectSpit[0], spitPoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.gravityScale = 1f; // �����ϰ� ����
                rb.AddForce(force, ForceMode2D.Impulse); // �߻�
            }
            else
            {
                Debug.LogWarning("�߻� ������Ʈ�� Rigidbody2D�� �����ϴ�.");
            }
        }
        yield return new WaitForSeconds(0.4f);
        force = new Vector2(4f * dirx, 7f);
        if (WarningObjectSpit != null && WarningObjectSpit.Length > 0 && spitPoint != null)
        {
            GameObject bullet = Instantiate(WarningObjectSpit[0], spitPoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.gravityScale = 1f; // �����ϰ� ����
                rb.AddForce(force, ForceMode2D.Impulse); // �߻�
            }
            else
            {
                Debug.LogWarning("�߻� ������Ʈ�� Rigidbody2D�� �����ϴ�.");
            }
        }
        yield return new WaitForSeconds(0.4f);
        force = new Vector2(12f * dirx, 0f);
        if (WarningObjectSpit != null && WarningObjectSpit.Length > 0 && spitPoint != null)
        {
            GameObject bullet = Instantiate(WarningObjectSpit[0], spitPoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.gravityScale = 1f; // �����ϰ� ����
                rb.AddForce(force, ForceMode2D.Impulse); // �߻�
            }
            else
            {
                Debug.LogWarning("�߻� ������Ʈ�� Rigidbody2D�� �����ϴ�.");
            }
        }

    }

    public void FireBullet()
    {
        float groundY = 0f;
        Vector2 startPos = spitPoint.position;
        Vector2 force = new Vector2(1f * dirx, 10f);
        float mass = spit.GetComponent<Rigidbody2D>().mass;
        float gravity = Physics2D.gravity.y;

        // 1. �ٴ� ��ġ ���� ����
        RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, 100f, groundMask);
        if (hit.collider != null)
        {
            groundY = hit.point.y;
        }

        // 4. �꼺ħ �߻�
        if (spit != null && spitPoint != null)
        {
            GameObject bullet = Instantiate(spit, spitPoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1f;
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

    public void FireBullet2() //enemyAI�� movetoplayer �޼ҵ带 ����ؼ� ����ź�� �ǰ� �����.
    {
        float groundY = 0f;
        Vector2 startPos = spitPoint.position;
        Vector2 force = new Vector2(4f * dirx, 7f);
        float mass = spit.GetComponent<Rigidbody2D>().mass;
        float gravity = Physics2D.gravity.y;

        // 1. �ٴ� ��ġ ���� ����
        RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, 100f, groundMask);
        if (hit.collider != null)
        {
            groundY = hit.point.y;
        }

        // 4. �꼺ħ �߻�
        if (spit != null && spitPoint != null)
        {
            GameObject bullet = Instantiate(spit, spitPoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1f;
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

    public void FireBullet3() //enemyAI�� movetoplayer �޼ҵ带 ����ؼ� ����ź�� �ǰ� �����.
    {
        float groundY = 0f;
        Vector2 startPos = spitPoint.position;
        Vector2 force = new Vector2(12f * dirx, 0f);
        float mass = spit.GetComponent<Rigidbody2D>().mass;
        float gravity = Physics2D.gravity.y;

        // 1. �ٴ� ��ġ ���� ����
        RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, 100f, groundMask);
        if (hit.collider != null)
        {
            groundY = hit.point.y;
        }


        // 4. �꼺ħ �߻�
        if (spit != null && spitPoint != null)
        {
            GameObject bullet = Instantiate(spit, spitPoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1f;
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

    public void ObjectOnOFF()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SetSteamPunktype()
    {
        Steampunk = true;
        target = steampunktarget.position;
        Select = true;
        Debug.Log("������ũ ����");
    }

    public void SetMagictype()
    {
        Magic = true;
        target = magictarget.position;
        Select = true;
        Debug.Log("�ù�");
    }


    //--------------------------------------------------------------------------------------------------------
    //              ������ ��Ʈ
    //--------------------------------------------------------------------------------------------------------
    public void StartJumpAreaDamageWindow()// ���� ������ ���ÿ�. �ִϸ��̼� �̺�Ʈ �Լ�
    {
        if (jumpBoxDamageCoroutine != null)
            StopCoroutine(jumpBoxDamageCoroutine);

        jumpBoxDamageCoroutine = StartCoroutine(JumpAreaDamageRoutine());
    }

    private IEnumerator JumpAreaDamageRoutine()
    {
        float timer = 0f;

        while (timer < 0.5f)
        {
            HashSet<PlayerHealth> damaged = new HashSet<PlayerHealth>();

            foreach (var area in jumpAttackAreas)
            {
                List<Collider2D> allHits = new List<Collider2D>();

                // Circle
                var circle = area.GetComponent<CircleCollider2D>();
                if (circle != null)
                {
                    Vector2 center = circle.bounds.center;
                    float radius = circle.radius * area.transform.lossyScale.x;
                    var circleHits = Physics2D.OverlapCircleAll(center, radius, playerLayer);
                    allHits.AddRange(circleHits);
                }

                // Box
                var box = area.GetComponent<BoxCollider2D>();
                if (box != null)
                {
                    Vector2 center = box.bounds.center;
                    Vector2 size = box.bounds.size;
                    float angle = box.transform.eulerAngles.z;
                    var boxHits = Physics2D.OverlapBoxAll(center, size, angle, playerLayer);
                    allHits.AddRange(boxHits);
                }

                // Damage
                foreach (var hit in allHits)
                {
                    if (hit.TryGetComponent(out PlayerHealth player) && !damaged.Contains(player))
                    {
                        player.TakeDamage(5, transform, 0.5f, 0.5f);
                        Debug.Log("JumpAttackArea ������ ����");
                        damaged.Add(player);
                    }
                }
            }

            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }

        jumpBoxDamageCoroutine = null;
    }

    public void StartJumpBoxDamageWindow() // ���� ������ ���ÿ�. �ִϸ��̼� �̺�Ʈ �Լ�
    {
        if (jumpBoxDamageCoroutine != null)
        {
            StopCoroutine(jumpBoxDamageCoroutine);
        }

        jumpBoxDamageCoroutine = StartCoroutine(JumpBoxDamageRoutine());
    }

    private IEnumerator JumpBoxDamageRoutine()
    {
        //damagedInBox.Clear();
        float timer = 0f;
        Debug.Log("�Լ�����");
        while (timer < 0.5f)
        {
            Vector2 boxCenter = JumpDealArea.bounds.center;
            Vector2 boxSize = JumpDealArea.bounds.size;
            float boxAngle = JumpDealArea.transform.eulerAngles.z;
            Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, boxAngle, playerLayer);

            foreach (var hit in hits)
            {
                //Debug.Log("���� ������");
                if (/*hit.CompareTag("Player") && */hit.TryGetComponent(out PlayerHealth player)/* && !damagedInBox.Contains(player)*/)
                {
                    player.TakeDamage(10, transform, 0.5f, 0.5f);
                    Debug.Log("Box ������ ����.");
                    //damagedInBox.Add(player);
                }
            }

            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        jumpBoxDamageCoroutine = null;
    }

    public void StartJumpCircleDamageWindow() // ���� ������ ���ÿ�. �ִϸ��̼� �̺�Ʈ �Լ�
    {
        if (jumpBoxDamageCoroutine != null)
        {
            StopCoroutine(jumpBoxDamageCoroutine);
        }

        jumpBoxDamageCoroutine = StartCoroutine(JumpCircleDamageRoutine());
    }

    private IEnumerator JumpCircleDamageRoutine()
    {
        //damagedInBox.Clear();
        float timer = 0f;
        Debug.Log("�Լ�����");
        while (timer < 0.5f)
        {
            CircleCollider2D circle = attack.GetComponent<CircleCollider2D>();
            Vector2 center = circle.bounds.center;
            float radius = circle.radius * attack.lossyScale.x;
            Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, playerLayer);

            foreach (var hit in hits)
            {
                //Debug.Log("���� ������");
                if (/*hit.CompareTag("Player") && */hit.TryGetComponent(out PlayerHealth player)/* && !damagedInBox.Contains(player)*/)
                {
                    player.TakeDamage(10, transform, 0.5f, 0.5f);
                    Debug.Log("Box ������ ����.");
                    //damagedInBox.Add(player);
                }
            }

            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        jumpBoxDamageCoroutine = null;
    }

}
