using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SandWorm : MonoBehaviour
{
    public Transform Boss;
    public Transform player;

    public Sandworm_Coroutine coroutine;
    public Sandworm_Animation ani;

    private List<BossSkill> SandwormSkills = new List<BossSkill>();

    private BossSkill lastUsedSkill = null;
    public float skillCheckInterval = 4f;
    public bool isCasting = false;
    public float dashDistance = 4f;
    public bool CantDash = false;

    public void Awake()
    {
        coroutine = GetComponent<Sandworm_Coroutine>();
        ani = GetComponent<Sandworm_Animation>();
    }
    public void InitSkills()
    {
        //SandwormSkills.Add(new BossSkill
        //{
        //    skillName = "Nattack",
        //    skillAction = () =>
        //    {
        //        Debug.Log("��ų �׼� ����: Nattack");
        //        StartCoroutine(coroutine.NAttack());
        //        Debug.Log("����");
        //    },
        //    cooldown = 8f,
        //    castTime = 8f,
        //    lastUsedTime = -999f
        //});
        //SandwormSkills.Add(new BossSkill
        //{
        //    skillName = "jumpAttack",
        //    skillAction = () =>
        //    {
        //        StartCoroutine(coroutine.Jump(player, 1f));
        //        Debug.Log("��ų �׼� ���� : JumpAttack");
        //    },
        //    cooldown = 8f,
        //    castTime = 8f,
        //    lastUsedTime = -999f
        //});
        //SandwormSkills.Add(new BossSkill
        //{
        //    skillName = "spit",
        //    skillAction = () =>
        //    {
        //        StartCoroutine(coroutine.Spit());
        //        Debug.Log("��ų �׼� ���� : spit");
        //    },
        //    cooldown = 8f,
        //    castTime = 8f,
        //    lastUsedTime = -999f
        //});
        //SandwormSkills.Add(new BossSkill
        //{
        //    skillName = "Jump Out",
        //    skillAction = () =>
        //    {
        //        Debug.Log("��ų �׼� ���� : Jump Out");
        //        StartCoroutine(coroutine.JumpOut(player, 1f));
        //    },
        //    cooldown = 15f,
        //    castTime = 15f,
        //    lastUsedTime = -999f
        //});


    }

    public IEnumerator SkillLoop()
    {
        while (true)
        {
            Debug.Log("��ų���� ����.");
            yield return new WaitForSeconds(skillCheckInterval);

            Debug.Log("��ų ��� �Ⱓ");
            if (isCasting) continue;

            // ������ ��ų ��� �� �غ�� �͸� ��
            var readySkills = SandwormSkills.FindAll(skill =>
            {
                // Dash��� �Ÿ� üũ
                if (skill.skillName == "NAttack")
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
        if (isCasting)
        {
            Debug.Log($"[{skill.skillName}] �õ������� �̹� �ٸ� ��ų ���� ���Դϴ�."); //����ڵ� �ߺ�����
            yield break;
        }

        isCasting = true;
        Debug.Log($"{DateTime.Now:HH:mm:ss}, {skill.skillName} ĳ��Ʈ ����");
        //skill.Use();
        //yield return new WaitForSeconds(skill.castTime);
        Debug.Log($"{DateTime.Now:HH:mm:ss}, {skill.skillName} ĳ��Ʈ ����");
        lastUsedSkill = skill;
        isCasting = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
