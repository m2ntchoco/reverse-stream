/*using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using System;

public class Boss1_Skill : MonoBehaviour
{
    private List<BossSkill> skillList = new List<BossSkill>();

    public Boss1_FSM FSM;
    public Boss1_Coroutine coroutine;
    public Boss1_WheelWind wheelWind;
    public Boss1_CatchThrow catchThrow;
    public Boss1_BackDash backdash;
    public Boss1_Jump jump;
    private BossSkill lastUsedSkill = null;
    [SerializeField] private CircleCollider2D pillarAttack;
    public int PillarDamge = 1;
    public float skillCheckInterval = 8f;
    private float nextSkillCheckTime = 0f;
    public bool isCasting = false;
    

    private void Start()
    {
        // ������Ʈ ����
        FSM = GetComponent<Boss1_FSM>();
        coroutine = GetComponent<Boss1_Coroutine>();
        wheelWind = GetComponent<Boss1_WheelWind>();
        catchThrow = GetComponent<Boss1_CatchThrow>();
        backdash = GetComponent<Boss1_BackDash>();
        jump = GetComponent<Boss1_Jump>();

        // ��ų ���
        skillList.Add(new BossSkill
        {
            skillName = "WheelWind",
            skillAction = () =>
            {
                Debug.Log("��ų �׼� ����: WheelWind");
                StartCoroutine(coroutine.FinalWheel());
                Debug.Log("����");
            },
            cooldown = 8f,
            castTime = 10f,
            lastUsedTime = -999f
        });

        skillList.Add(new BossSkill
        {
            skillName = "CatchThrow",
            skillAction = () =>
            {                
                catchThrow.LaunchBoomerang();
                Debug.Log("����");
            },
            cooldown = 10f,
            castTime = 6f,
            lastUsedTime = -999f
        });
        skillList.Add(new BossSkill
        {
            skillName = "Jump",
            skillAction = () => 
            { 
                StartCoroutine(coroutine.JumpFinal());
                Debug.Log("����");
            },
            cooldown = 8f,
            castTime = 8f,
            lastUsedTime = -999f
        });


        StartCoroutine(SkillLoop());
    }

    public IEnumerator SkillLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillCheckInterval);

            if (isCasting) continue;
            

            var readySkills = skillList.FindAll(skill => skill.IsReady() && skill != lastUsedSkill);

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
    }
    IEnumerator CastSkill(BossSkill skill)
    {
        isCasting = true;
        Debug.Log($"{DateTime.Now:HH:mm:ss}, {skill.skillName} ĳ��Ʈ ����");
        skill.Use();
        yield return new WaitForSeconds(skill.castTime); // castTime �Ӽ� �ʿ�
        Debug.Log($"{DateTime.Now:HH:mm:ss}, {skill.skillName} ĳ��Ʈ ����");
        lastUsedSkill = skill;
        isCasting = false;
    }
}
*/