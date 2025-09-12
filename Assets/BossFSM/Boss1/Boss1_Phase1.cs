using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Boss1_Phase1 : IEnemyState 
{
    public Boss1_Jump Jump;
    public Boss1_WheelWind WheelWind;
    public Boss1_CatchThrow CatchThrow;
    public Boss1_FSM FSM;
    private MonoBehaviour mono;
    private Rigidbody2D rb;
    UnityEngine.Transform target;
    public Boss1_Animation ani;
    public Boss1_Coroutine Coroutine;
    public Boss1_WheelWind Wheel;
    public Boss1_BackDash BackDash;
    public Boss1_SkillManager skillManager;
    //public Boss1_Skill BossSkill;

    public Boss1_Phase1(Boss1_FSM boss)
    {
        FSM = boss.GetComponent<Boss1_FSM>();
        mono = boss.GetComponent<MonoBehaviour>();
        ani = boss.GetComponent<Boss1_Animation>();
        CatchThrow = boss.GetComponent<Boss1_CatchThrow>(); 
        Coroutine = boss.GetComponent<Boss1_Coroutine>();
        Wheel = boss.GetComponent <Boss1_WheelWind>();
        BackDash = boss.GetComponent<Boss1_BackDash>();
        //BossSkill = boss.GetComponent<Boss1_Skill>();
        skillManager = boss.GetComponent<Boss1_SkillManager>();
    }
    public void Awake()
    {

    }
    public void Enter()
    {

        rb = FSM.GetComponent<Rigidbody2D>();

        if (FSM.target == null)
        {
            Debug.LogError("���� target�� null�Դϴ�. ���� �Ұ�!");
            return;
        }

        Debug.Log("1������ ����");

        //------------------------------------------------- �����ڵ� ����.
        //mono.StartCoroutine(Coroutine.JumpFinal());/*����*/
        //CatchThrow.LaunchBoomerang();/*�θ޶�*/
        //Wheel.StartCoroutine(Coroutine.FinalWheel());/*������*/
        //BackDash.backDash();/*��뽬*/
        mono.StartCoroutine(skillManager.SkillLoop());/*��ų ��*/
        //--------------------------------------------------
    }

    public IEnumerator Catchprepare()
    {
        ani.CatchBoomerang();
        yield return new WaitForSeconds(1.4f);
    }
    public void Exit()
    {

    }

    public void Update()
    {
        
    }
    
    public IEnumerator Phase1Pattern()
    {
        yield return null;
    }

    public void Nattack()
    {

    }
}
