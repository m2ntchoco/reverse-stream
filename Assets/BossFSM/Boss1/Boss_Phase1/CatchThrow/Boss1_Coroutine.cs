using System.Collections;
using System.Threading;
using UnityEngine;
//using static Unity.Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;

public class Boss1_Coroutine : MonoBehaviour
{
    public Boss1_Animation ani;
    public Boss1_CatchThrow Boomerang;
    public Boss1_Jump Jump;
    public Boss1_FSM FSM;
    public Boss1_WheelWind Wheel;
    public Boss1_BackDash backdash;
    public Rigidbody2D rb;
    public Boss1_SkillManager skillManager;

    [SerializeField] private CircleCollider2D pillarAttack;
    [SerializeField] private Transform character;
    public void Init(Boss1_FSM boss)
    {
        FSM = boss;
        /*Wheel = boss.GetComponent<Boss1_WheelWind>();
        ani = boss.GetComponent<Boss1_Animation>();
        Boomerang = boss.GetComponent<Boss1_CatchThrow>();
        Jump = boss.GetComponent<Boss1_Jump>();
        backdash = boss.GetComponent<Boss1_BackDash>();
        rb = boss.GetComponent<Rigidbody2D>();
        skillManager = boss.GetComponent<Boss1_SkillManager>();*/
    }
    public void Awake()
    {
        Init(FSM);
        Wheel = FSM.GetComponent<Boss1_WheelWind>();
        ani = FSM.GetComponent<Boss1_Animation>();
        Boomerang = FSM.GetComponent<Boss1_CatchThrow>();
        Jump = FSM.GetComponent<Boss1_Jump>();
        backdash = FSM.GetComponent<Boss1_BackDash>();
        rb = FSM.GetComponent<Rigidbody2D>();
        skillManager = FSM.GetComponent<Boss1_SkillManager>();
        
        if (character == null)
        {
            var go = GameObject.FindWithTag("Player");
            if (go != null) character = go.transform;
        }
    }

    public void Update()
    {
      
    }

    public void Exit() 
    {

    }
    public IEnumerator Nattack()
    {
        Vector3 dir = FSM.target.position - transform.position;
        if (dir.x > 0)
            transform.localScale = new Vector3(-3, 3, 1);  // ������
        else
            transform.localScale = new Vector3(3, 3, 1); // ����
        Coroutine dmg = StartCoroutine(WheelWindDamageCoroutine());
        //skillManager.TrackSkillCoroutine(dmg);
        if (FSM.isGroggy) yield break;
        ani.Nattackprepare();
        yield return new WaitForSeconds(1f);
        if (FSM.isGroggy) yield break;
        ani.Nattack();
    }
    public IEnumerator JumpFinal()
    {
        Coroutine dmg = StartCoroutine(WheelWindDamageCoroutine());
        //skillManager.TrackSkillCoroutine(dmg);
        if (FSM.isGroggy) yield break;
        yield return StartCoroutine(PrepareJump());    //mono.StartCoroutine(JumpFinal());
        if (FSM.isGroggy) yield break;
        yield return StartCoroutine(Jumping());
        if (FSM.isGroggy) yield break;
        yield return new WaitForSeconds(2f);
    }

    public IEnumerator Jumping()
    {

        for (int i = 0; i < 3; i++)
        {
            if (FSM.isGroggy) yield break;
            ani.KeepGoing();
            Debug.Log("���� ����!");
            Jump.Jump(FSM.target, 5f);
            //Debug.Log("1�� ��� ��");
            yield return new WaitForSeconds(1f);
            FSM.DamagePillarsInRange(pillarAttack, 1);
        }
        ani.KeepEnd();
        //Debug.Log("���� ����");
    }
    public IEnumerator PrepareJump()
    {
        Debug.Log("�غ�");
        ani.PrepareJump();
        yield return new WaitForSeconds(1.4f);
    }
    
    public IEnumerator FinalWheel()
    {
        ani.ani.ResetTrigger("WheelEnd");
        Coroutine dmg = StartCoroutine(WheelWindDamageCoroutine());
        //skillManager.TrackSkillCoroutine(dmg);
        ani.WheelPrepare();
        yield return new WaitForSeconds(3f);
        ani.WheelStart();
        StartCoroutine(WheelWindDamageCoroutine());
        float duration = 5f;
        float timer = 0f;
        while (timer < duration)
        {
            if (FSM.isGroggy) yield break;
            Wheel.WheelMove();
            timer += Time.deltaTime;
            yield return null;
        }
        //yield return new WaitForSeconds(3f);
        ani.WheelEnd();
        yield return Vector3.zero;
        ani.ani.ResetTrigger("WheelEnd");
    }

    public IEnumerator WheelWindDamageCoroutine()
    {
        float timer = 0f;
        while (timer < Wheel.wheelDuration)
        {
            Vector2 center = Wheel.wheelAttackArea.bounds.center;
            Vector2 size = new Vector2(
                Wheel.wheelAttackArea.size.x * Wheel.wheelAttackArea.transform.lossyScale.x,
                Wheel.wheelAttackArea.size.y * Wheel.wheelAttackArea.transform.lossyScale.y
            );
            Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, Wheel.playerLayer);
            //Debug.Log($"������ ���� ��� ��: {hits.Length}");
            foreach (var hit in hits)
            {
                if (FSM.isGroggy) yield break;

                if (hit.CompareTag("Player") && hit.TryGetComponent(out PlayerHealth player))
                {
                    if (!Wheel.recentlyDamaged.Contains(player))
                    {
                        if (!Boss1_FSM.cphase2)
                        { 
                            player.TakeDamage(Wheel.wheelDamage, transform, 0f, 0f); 
                        }
                        else
                        {
                            player.TakeDamage(Wheel.wheelDamage, transform,   4f, 4f); 
                        }
                            Wheel.recentlyDamaged.Add(player);
                        if (Boss1_FSM.cphase2)
                        {
                            //StartCoroutine(FSM.KnockBack(FSM.target, 10f, 10f));
                            Debug.Log("���ǹ� ����");
                        }
                    }
                }
            }

            yield return new WaitForSeconds(Wheel.wheelDamageInterval);
            FSM.DamagePillarsInRange(pillarAttack, 1);
            Wheel.recentlyDamaged.Clear(); // ���� ���ݿ� �ٽ� ������ ����
            timer += Wheel.wheelDamageInterval;
        }
    }
    //--------------------------------------------------------------------------------------------------------------------
    // ������ 2
    //--------------------------------------------------------------------------------------------------------------------
    public IEnumerator BackdashCoroutine()
    {
        Coroutine dmg = StartCoroutine(WheelWindDamageCoroutine());
        //skillManager.TrackSkillCoroutine(dmg);
        Vector3 dir = FSM.target.position - transform.position;
        if (dir.x > 0)
            transform.localScale = new Vector3(3, 3, 1);  // ������
        else
            transform.localScale = new Vector3(-3, 3, 1); // ����
        Debug.Log("�ڷ�ƾ ����");
        Debug.Log("animation");
        // 3. ���� ��� (�÷��̾��� �ݴ� ��������)
        Vector3 dirToPlayer = (backdash.player.position - transform.position).normalized;
        float directionSign = Mathf.Sign(-dirToPlayer.x);
        Vector3 dashTarget = transform.position + new Vector3(directionSign * 3f, 0f, 0f);
        Debug.Log("heyjey");
        // 4. ������ �̵�
        while (Mathf.Abs(transform.position.x - dashTarget.x) > 0.1f)
        {
            if (FSM.isGroggy) yield break;
            Vector3 xdir = (dashTarget - transform.position).normalized;
            rb.linearVelocity = xdir * backdash.dashSpeed / 4;
            yield return null;
        }

        Debug.Log("�߾߾�");        // 5. ��Ȯ�� ����
        rb.linearVelocity = Vector3.zero;
        transform.position = new Vector3(dashTarget.x, transform.position.y, 0);
        Debug.Log("���� �Ϸ� �� ����");
    }

    public IEnumerator dashCoroutine()
    {
        Coroutine dmg = StartCoroutine(WheelWindDamageCoroutine());
        //skillManager.TrackSkillCoroutine(dmg);
        Vector3 dir = FSM.target.position - transform.position;
        if (dir.x > 0) {
            transform.localScale = new Vector3(-3, 3, 1);  // ������
            Debug.Log("������");
        }
        else {transform.localScale = new Vector3(3, 3, 1); // ����
            Debug.Log("����");
        }
            
        Debug.Log("�ڷ�ƾ ����");
        ani.Dash();
        if (FSM.isGroggy) yield break;
        yield return new WaitForSeconds(1f);
        if (FSM.isGroggy) yield break;
        // 3. ���� ��� (�÷��̾� ��������)
        Vector3 dirToPlayer = (backdash.player.position - transform.position).normalized;
        float directionSign = Mathf.Sign(dirToPlayer.x);
        Vector3 dashTarget = transform.position + new Vector3(directionSign * 8f, 0f, 0f);
        Debug.Log("heyjey");
        if (FSM.isGroggy) yield break;
        // 4. ������ �̵�
        while (Mathf.Abs(transform.position.x - dashTarget.x) > 0.1f)
        {
            Vector3 xdir = (dashTarget - transform.position).normalized;
            rb.linearVelocity = xdir * backdash.dashSpeed / 6;

            yield return null;
        }
        if (FSM.isGroggy) yield break;

        // 5. ��Ȯ�� ����
        rb.linearVelocity = Vector3.zero;
        transform.position = new Vector3(dashTarget.x, transform.position.y, 0);

    }
    public IEnumerator JumpDashFinal()
    {
        Coroutine dmg = StartCoroutine(WheelWindDamageCoroutine());
        //skillManager.TrackSkillCoroutine(dmg);
        yield return StartCoroutine(BackDashPrepareJump()); //mono.StartCoroutine(JumpFinal());
        if (FSM.isGroggy) yield break;
        yield return StartCoroutine(Jumping());
        if (FSM.isGroggy) yield break;
        yield return new WaitForSeconds(3f);
    }

    public IEnumerator BackDashPrepareJump()
    {
        Debug.Log("�غ�");
        ani.PrepareJump();
        yield return StartCoroutine(BackdashCoroutine());
        yield return new WaitForSeconds(1.4f);
    }

    
    public IEnumerator PhaseChange()
    {
        FSM.hit = true;
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>(true);

        foreach (var sr in renderers)
        {
            sr.enabled = false;
        }
        foreach (Transform child in transform)
        {
            if (child.name != "isground")
            {
                child.gameObject.SetActive(false);
            }
        }
        ani.PhaseChange();
        yield return new WaitForSeconds(1f);
        foreach (var sr in renderers)
        {
            sr.enabled = true;
        }
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        transform.position = new Vector2(character.position.x, character.position.y+10);
    }
    public IEnumerator Phase2Wheelwind()
    {
        Coroutine dmg = StartCoroutine(WheelWindDamageCoroutine());
        //skillManager.TrackSkillCoroutine(dmg);
        ani.ani.ResetTrigger("WheelEnd");
        ani.WheelPrepare();
        if (FSM.isGroggy) yield break;
        yield return new WaitForSeconds(3f);
        if (FSM.isGroggy) yield break;
        ani.WheelStart();
        Debug.Log($"[{Time.time:F2}] Phase2Wheelwind �� Reset WheelEnd");
        StartCoroutine(WheelWindDamageCoroutine());
        if (FSM.isGroggy) yield break;
        float duration = 5f;
        float timer = 0f;
        while (timer < duration)
        {
            if (FSM.isGroggy) yield break;
            Wheel.WheelMove();
            //PlayerHealth.isknockback = true;
            
            timer += Time.deltaTime;
            yield return null;
        }
        if (FSM.isGroggy) yield break;
        //yield return new WaitForSeconds(2f);
        ani.WheelEnd();
        if (FSM.isGroggy) yield break;
        yield return new WaitForSeconds(0.2f);
        Boomerang.LaunchBoomerang();
        if (FSM.isGroggy) yield break;
        //PlayerHealth.isknockback = true;
        yield return Vector3.zero;
    }

}
