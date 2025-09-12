using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Boss1_WheelWind : MonoBehaviour
{
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public float wheelDamageInterval = 1f; // �� ����
    [SerializeField] public float wheelDuration = 3f;         // ������ ���� �ð�
    [SerializeField] public int wheelDamage = 20;
    [SerializeField] public float wheelRange = 1.5f;
    [SerializeField] public BoxCollider2D wheelAttackArea;

    private Vector3 lockedDir;
    private float lockTimer = 0f;
    private const float lockDuration = 1f;


    public HashSet<PlayerHealth> recentlyDamaged = new HashSet<PlayerHealth>();
    public Boss1_Phase1 Phase1;
    public Boss1_Animation ani;
    public Boss1_Coroutine coroutine;
    public Boss1_FSM fsm;
    
    public float moveSpeed = 15f;
    public float searchRadius = 30f;

    private UnityEngine.Transform player;

    public void Init()
    {
        ani = GetComponent<Boss1_Animation>();
        coroutine = GetComponent<Boss1_Coroutine>();
        fsm = GetComponent<Boss1_FSM>();
    }

    public void FindPlayerByLayer(LayerMask playerLayer)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 50f, playerLayer);
        if (hits.Length > 0)
        {
            player = hits[0].transform;
            Debug.Log(hits[0].transform);
        }
    }

    private void Awake()
    {
        Init();
    }


    private Vector3 GetDirectionToPlayer()
    {
        if (player == null)
        {
            // ��� "Player" �±� ������Ʈ Ž��
            GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject obj in playerObjs)
            {
                player = obj.transform;

            }

            if (player == null)
            {
                Debug.LogWarning("Player �±��̰� �̸��� 'Ark'�� ������Ʈ�� ã�� �� �����ϴ�.");
                return Vector3.zero;
            }
        }

        //Debug.Log(player.position);
        //Debug.Log(transform.position);
        // ���� ���
        return (player.position - transform.position).normalized;
    }


    public void Enter()
    {
    }

    public void Update()
    {
    }
    public void Exit() 
    {
    
    }

    public void WheelMove()
    {
        /*int xdir;
        Vector3 dir = GetDirectionToPlayer();
        dir.y = 0;
        transform.position += dir * moveSpeed * Time.deltaTime;
        if (dir.x <= 0)
        {
            xdir = -1;
        }
        else
        {
            xdir = 1;
        }
        if (fsm == null)
        {
            Debug.LogError("FSM�� null�Դϴ�! �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }
        fsm.FaceDirection(xdir);
        //Debug.Log(transform.position);*/

        if (lockTimer <= 0f)
        {
            // �÷��̾� ��ġ ������� ���� ���
            Vector3 dir = GetDirectionToPlayer();
            dir.y = 0;
            lockedDir = dir.normalized;        // ������ ���� ����
            lockTimer = lockDuration;          // Ÿ�̸� ����
        }
        if (player == null)
        {
            Debug.LogWarning("�÷��̾ ã�� �������Ƿ� �̵����� �ʽ��ϴ�.");
        }
        // 2) ������ �������� �̵�
        transform.position += lockedDir * moveSpeed * Time.deltaTime;
        //Debug.Log(transform.position);
        // 3) Ÿ�̸� ����
        lockTimer -= Time.deltaTime;

        // 4) �ٶ󺸴� ���� ���߱�
        int xdir = lockedDir.x < 0 ? -1 : 1;
        fsm.FaceDirection(xdir);

    }
    
}


