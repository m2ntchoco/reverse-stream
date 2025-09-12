/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class Boss1_SkillMana : MonoBehaviour
{                                                                   // ĳ��Ʈ Ÿ�Ӱ� ��Ÿ���� ������ �ð��� �۵��ϰ� ĳ��Ʈ Ÿ���� ������ ������ �ɸ��� �ð�. ��Ÿ���� �� �ð��� ������ ���� ������ ���� ����.
    private List<BossSkill> phase1Skills = new List<BossSkill>();   // �ǵ��� ĳ��Ʈ Ÿ�Ӱ� ��Ÿ���� �����ϰ� �� ��.
    private List<BossSkill> phase2Skills = new List<BossSkill>();
    private List<BossSkill> currentSkillList;
    private BossSkill lastUsedSkill = null;
    private Coroutine skillLoopCoroutine;

    public Boss1_Coroutine coroutine;
    public Boss1_WheelWind wheelWind;
    public Boss1_CatchThrow catchThrow;
    public Boss1_BackDash backdash;
    public Boss1_Jump jump;

    public Transform player;
    public float skillCheckInterval = 8f;
    public bool isCasting = false;
    //public float dashDistance = 10f;
    public bool CantDash = false;

    private void Start()
    {
        // ������Ʈ ����
        coroutine = GetComponent<Boss1_Coroutine>();
        wheelWind = GetComponent<Boss1_WheelWind>();
        catchThrow = GetComponent<Boss1_CatchThrow>();
        backdash = GetComponent<Boss1_BackDash>();
        jump = GetComponent<Boss1_Jump>();

        InitSkills();
        SetPhase(1); // �ʱ� ������ ����
    }

    public void InitSkills()
    {
        // ������ 1 ��ų
        phase1Skills.Add(new BossSkill
        {
            skillName = "WheelWind",
            skillAction = () =>
            {
                Debug.Log("��ų �׼� ����: WheelWind");
                StartCoroutine(coroutine.FinalWheel());
                Debug.Log("����");
            },
            cooldown = 9f,
            castTime = 9f,
            lastUsedTime = -999f
        });
        phase1Skills.Add(new BossSkill
        {
            skillName = "CatchThrow",
            skillAction = () =>
            {
                catchThrow.LaunchBoomerang();
                Debug.Log("����");
            },
            cooldown = 9f,
            castTime = 6f,
            lastUsedTime = -999f
        });

        phase1Skills.Add(new BossSkill
        {
            skillName = "Jump",
            skillAction = () =>
            {
                StartCoroutine(coroutine.JumpFinal());
                Debug.Log("����");
            },
            cooldown = 9f,
            castTime = 9f,
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
            cooldown = 12f,
            castTime = 12f,
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
            cooldown = 20f,
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
            cooldown = 8f,
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
        Debug.Log($"[��ų ������ ��ȯ] ������ {phase}");
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
   /* public IEnumerator SkillLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillCheckInterval);

            if (isCasting) continue;

            // ������ ��ų ��� �� �غ�� �͸� ��
            var readySkills = currentSkillList.FindAll(skill =>
            {
                // Dash��� �Ÿ� üũ
                if (skill.skillName == "Dash")
                {
                    float dist = Vector3.Distance(transform.position, player.position);
                    return skill.IsReady() && skill != lastUsedSkill && dist > dashDistance && !CantDash;
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


    private IEnumerator CastSkill(BossSkill skill)
    {
        isCasting = true;
        Debug.Log($"{DateTime.Now:HH:mm:ss}, {skill.skillName} ĳ��Ʈ ����");
        skill.Use();
        yield return new WaitForSeconds(skill.castTime);
        Debug.Log($"{DateTime.Now:HH:mm:ss}, {skill.skillName} ĳ��Ʈ ����");
        lastUsedSkill = skill;
        isCasting = false;
    }
}*/
