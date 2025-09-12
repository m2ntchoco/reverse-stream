using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;

public class Boss1_SkillManager : MonoBehaviour
{                                                                   // ĳ��Ʈ Ÿ�Ӱ� ��Ÿ���� ������ �ð��� �۵��ϰ� ĳ��Ʈ Ÿ���� ������ ������ �ɸ��� �ð�. ��Ÿ���� �� �ð��� ������ ���� ������ ���� ����.
    private List<BossSkill> phase1Skills = new List<BossSkill>();   // �ǵ��� ĳ��Ʈ Ÿ�Ӱ� ��Ÿ���� �����ϰ� �� ��.
    private List<BossSkill> phase2Skills = new List<BossSkill>();
    private List<BossSkill> groggySkills = new List<BossSkill>();
    private List<BossSkill> currentActiveSkillList;
    private List<BossSkill> currentSkillList;
    private BossSkill lastUsedSkill = null;
    private Coroutine skillLoopCoroutine;
    private Coroutine castingCoroutine;
    private Coroutine currentSkillCoroutine;
    private List<Coroutine> runningSkillCoroutines = new List<Coroutine>();

    public Boss1_Coroutine coroutine;
    public Boss1_WheelWind wheelWind;
    public Boss1_CatchThrow catchThrow;
    public Boss1_BackDash backdash;
    public Boss1_Jump jump;
    public Boss1_Animation ani;
    public Transform player;
    public Boss1_FSM fsm;
    public float skillCheckInterval = 8f;
    public bool isCasting = false;
    public float dashDistance = 4f;
    public bool CantDash = false;

    //public bool isGroggy = false;
    private float damageAccumulator = 0f;
    [SerializeField] private float groggyThreshold = 10f;
    [SerializeField] private float groggyDuration = 5f;
    private Coroutine groggyCoroutine;

    private void Start()
    {
        // ������Ʈ ����
        coroutine = GetComponent<Boss1_Coroutine>();
        wheelWind = GetComponent<Boss1_WheelWind>();
        catchThrow = GetComponent<Boss1_CatchThrow>();
        backdash = GetComponent<Boss1_BackDash>();
        jump = GetComponent<Boss1_Jump>();
        ani = GetComponent<Boss1_Animation>();
        fsm = GetComponent<Boss1_FSM>();
        InitSkills();
        SetPhase(1); // �ʱ� ������ ����

        if (player == null)
        {
            var go = GameObject.FindWithTag("Player");
            if (go != null) player = go.transform;
        }
    }

    private void InitSkills()
    {
        groggySkills.Add(new BossSkill
        {
            skillName = "GroggyStun",
            skillAction = () =>
            {
                StartCoroutine(HandleGroggyStun());
            },
            cooldown = 0f,
            castTime = groggyDuration,
            lastUsedTime = -999f
        });
        // ������ 1 ��ų
        phase1Skills.Add(new BossSkill
        {
            skillName = "Nattack",
            skillAction = () =>
            {
                Debug.Log("��ų �׼� ����: Nattack");
                StartCoroutine(coroutine.Nattack());
                Debug.Log("����");
            },
            cooldown = 12f,
            castTime = 4f,
            lastUsedTime = -999f
        });
        phase1Skills.Add(new BossSkill
        {
            skillName = "WheelWind",
            skillAction = () =>
            {
                Debug.Log("��ų �׼� ����: WheelWind");
                StartCoroutine(coroutine.FinalWheel());
                Debug.Log("����");
            },
            cooldown = 10f,
            castTime = 8f,
            lastUsedTime = -999f
        });
        /*phase1Skills.Add(new BossSkill
        {
            skillName = "CatchThrow",
            skillAction = () =>
            {
                catchThrow.LaunchBoomerang();
                Debug.Log("����");
            },
            cooldown = 8f,
            castTime = 5f,
            lastUsedTime = -999f
        });*/

        phase1Skills.Add(new BossSkill
        {
            skillName = "Jump",
            skillAction = () =>
            {
                StartCoroutine(coroutine.JumpFinal());
                Debug.Log("����");
            },
            cooldown = 10f,
            castTime = 8f,
            lastUsedTime = -999f
        });

        // ������ 2 ��ų
        phase2Skills.Add(new BossSkill
        {
            skillName = "EnWheelWind",
            skillAction = () =>
            {
                Debug.Log("��ų �׼� ����: EnWheelWind");
                StartCoroutine(coroutine.Phase2Wheelwind()); // ������ �ڷ�ƾ ���
                Debug.Log("����2");
            },
            cooldown = 11f,
            castTime = 11f,
            lastUsedTime = -999f
        });

        phase2Skills.Add(new BossSkill
        {
            skillName = "Dash",
            skillAction = () =>
            {
                Debug.Log("��ų �׼� ����: Dash");
                StartCoroutine(coroutine.dashCoroutine()); // ������ �ڷ�ƾ ���
                Debug.Log("����2");
            },
            cooldown = 10f,
            castTime = 5f,
            lastUsedTime = -999f
        });

        phase2Skills.Add(new BossSkill
        {
            skillName = "EnJump",
            skillAction = () =>
            {
                StartCoroutine(coroutine.JumpDashFinal());
                Debug.Log("����2");
            },
            cooldown = 7f,
            castTime = 8f,
            lastUsedTime = -999f
        });
    }
    public void SetPhase(int phase)
    {
        // ���� �ڷ�ƾ ����
        if (skillLoopCoroutine != null)
            StopCoroutine(skillLoopCoroutine);

        // ����� �´� ��ų ����Ʈ�� ����
        if (phase == 1)
        {
            currentSkillList = phase1Skills;
        }
        else if (phase == 2)
        {
            currentSkillList = phase2Skills;
        }

        // �� �ڷ�ƾ ����
        skillLoopCoroutine = StartCoroutine(SkillLoop());
        //Debug.Log($"[��ų ������ ��ȯ] ������ {phase}");
    }

    /*public IEnumerator SkillLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillCheckInterval);

            if (isCasting) continue;

            var readySkills = currentSkillList.FindAll(skill => skill.IsReady() && skill != lastUsedSkill);
            if (readySkills.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, readySkills.Count);
                var selected = readySkills[index];
                StartCoroutine(CastSkill(selected));
                Debug.Log($"[��ų ���] {selected.skillName}");
            }
            else
            {
                Debug.Log("��� ��ų ��Ÿ�� ��");
            }
        }
    }*/
    public IEnumerator SkillLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillCheckInterval);

            if (isCasting) continue;

            currentActiveSkillList = fsm.isGroggy ? groggySkills : currentSkillList;
            // ������ ��ų ��� �� �غ�� �͸� ��
            var readySkills = currentActiveSkillList.FindAll(skill =>
            {
                float dist = Vector3.Distance(transform.position, player.position);
                // Dash��� �Ÿ� üũ
                if (skill.skillName == "Dash")
                {
                    return skill.IsReady() && skill != lastUsedSkill && dist > dashDistance && !CantDash;
                }
                if (skill.skillName == "Nattack")
                {
                    return skill.IsReady() && skill != lastUsedSkill && dist <= 3f;
                }

                // �� �� �Ϲ� ��ų
                return skill.IsReady() && skill != lastUsedSkill;
            });

            if (readySkills.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, readySkills.Count);
                var selected = readySkills[index];
                StartCoroutine(CastSkill(selected));
                Debug.Log($"[��ų ���] {selected.skillName}");
            }
            else
            {
                Debug.Log("��� ��ų ��Ÿ�� �� �Ǵ� �Ÿ� ���� ������");
            }
        }
    }
    /*public void ReceiveDamage(float amount)
    {
        float finalDamage = isGroggy ? amount + 5f : Mathf.Max(1f, amount - 5f);
        damageAccumulator += finalDamage;

        Debug.Log($"[���� �ǰ�] ������: {finalDamage}, ����: {damageAccumulator}");

        if (!isGroggy && damageAccumulator >= groggyThreshold)
        {
            StartGroggy();
        }
    }*/

    public void StartGroggy()
    {
        if (currentSkillCoroutine != null)
        {
            Debug.Log("[��ų �ߴ�] ���� ��ų �ڷ�ƾ ���� ����");
            StopCoroutine(currentSkillCoroutine);
            currentSkillCoroutine = null;
        }

        if (fsm.isGroggy) return;

        fsm.isGroggy = true;

        if (castingCoroutine != null)
        {

            StopCoroutine(castingCoroutine);
            castingCoroutine = null;
        }

        isCasting = false;
        damageAccumulator = 0f;

        ani.Groggy();
        Debug.Log("[���� ����] �׷α� ����");

        var groggySkill = groggySkills.Find(s => s.skillName == "GroggyStun");
        if (groggySkill != null)
        {
            groggyCoroutine = StartCoroutine(CastSkill(groggySkill));
        }
    }

    private IEnumerator HandleGroggyStun()
    {
        isCasting = true;
        Debug.Log("[�׷α� ��ų] ���� �ִϸ��̼� �� ����");

        yield return new WaitForSeconds(groggyDuration);

        Debug.Log("[�׷α� ��ų] ����");
        fsm.isGroggy = false;
        isCasting = false;
        ani.GroggyEnd();
    }

    private IEnumerator CastSkill(BossSkill skill)
    {
        if (isCasting)
        {
            Debug.Log($"[{skill.skillName}] �õ������� �̹� �ٸ� ��ų ���� ���Դϴ�.");
            yield break;
        }

        if (castingCoroutine != null)
        {
            StopCoroutine(castingCoroutine);
            castingCoroutine = null;
        }

        isCasting = true;
        Debug.Log($"{DateTime.Now:HH:mm:ss}, {skill.skillName} ĳ��Ʈ ����");

        skill.Use();  // �ٷ� ����

        castingCoroutine = StartCoroutine(SkillCastTimer(skill));
        yield return castingCoroutine;
    }
    private IEnumerator SkillCastTimer(BossSkill skill)
    {
        yield return new WaitForSeconds(skill.castTime);

        if (fsm.isGroggy)
        {
            Debug.Log($"[{skill.skillName}] ���� �׷α� �������� ���� �����");
            castingCoroutine = null;
            yield break;
        }

        Debug.Log($"{DateTime.Now:HH:mm:ss}, {skill.skillName} ĳ��Ʈ ����");
        lastUsedSkill = skill;
        isCasting = false;
        castingCoroutine = null;
    }
    public void TrackSkillCoroutine(Coroutine c)
    {
        if (c != null)
            runningSkillCoroutines.Add(c);
    }
    public void StopAllSkillCoroutines()
    {
        foreach (var c in runningSkillCoroutines)
        {
            if (c != null)
                StopCoroutine(c);
        }
        runningSkillCoroutines.Clear();
    }
}