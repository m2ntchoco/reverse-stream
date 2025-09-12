using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[DefaultExecutionOrder(50)]
public class DashStackUI : MonoBehaviour
{
    [Header("���� ����")]
    public float cooldownTime = 5f;

    [Header("���ù� ������")]
    public GameObject dashStackBarFrame2;
    public GameObject dashStackBarFrame3;

    [Header("���� �̹���")]
    public Image[] stackBars2;
    public Image[] stackBars3;

    private Image[] currentStackBars;
    private int currentStacks;
    private bool[] isCoolingDown;

    // (���� ���� ��ġ, ��ٿ��� ǥ���� �ð��� ��ġ)
    Queue<int> cooldownQueue = new(); // �ð� �ε����� ���߿� ����
    int visualCooldownIndex = 0;

    private int nextVisualIndex = 0;
    private bool isCooldownRunning = false;

    [SerializeField] private ChooseOne chooseone;

    [SerializeField] private DashSkill steampunkdashSkill;
    [SerializeField] private DashSkill magicdashSkill;

    public void SteamPunkDash()
    {
        GameObject steampunkobj = GameObject.Find("Ark/SteamPunk");
        steampunkdashSkill = steampunkobj.GetComponent<DashSkill>();
        currentStacks = steampunkdashSkill.maxStacks;
        SetupUI();
        isCoolingDown = new bool[currentStackBars.Length];
        for (int i = 0; i < currentStackBars.Length; i++)
        {
            currentStackBars[i].fillAmount = 1f;
        }
    }

    public void MagicDash()
    {
        Debug.Log("�鸶��");
        GameObject magicobj = GameObject.Find("Ark/Mana");
        magicdashSkill = magicobj.GetComponent<DashSkill>();
        currentStacks = magicdashSkill.maxStacks;
        SetupUI();
        isCoolingDown = new bool[currentStackBars.Length];
        for (int i = 0; i < currentStackBars.Length; i++)
        {
            currentStackBars[i].fillAmount = 1f;
        }
    }

    void SetupUI()
    {
        if (chooseone.SystemSteamPunk)
        {
            dashStackBarFrame2.SetActive(steampunkdashSkill.maxStacks == 2);
            dashStackBarFrame3.SetActive(steampunkdashSkill.maxStacks == 3);
            currentStackBars = (steampunkdashSkill.maxStacks == 2) ? stackBars2 : stackBars3;
        }
        else
        {
            dashStackBarFrame2.SetActive(magicdashSkill.maxStacks == 2);
            dashStackBarFrame3.SetActive(magicdashSkill.maxStacks == 3);
            currentStackBars = (magicdashSkill.maxStacks == 2) ? stackBars2 : stackBars3;
        }
    }

    public bool HasAvailableStack()
    {
        return currentStacks > 0;
    }

    public void UseDash()
    {
        if (currentStacks <= 0)
            return;

        // �����ʺ��� ��� ������ ���� ã��
        for (int i = currentStackBars.Length - 1; i >= 0; i--)
        {
            if (!isCoolingDown[i])
            {
                isCoolingDown[i] = true;
                cooldownQueue.Enqueue(i); // usedIndex�� ����
                Debug.Log($"[DASH] ���� ����: ���� �ε���={i}, �ð��� �ε���={nextVisualIndex}");

                nextVisualIndex++;
                currentStacks--;
                UpdateStackUI();
                TryStartNextCooldown();
                return;
            }
        }
    }

    private void TryStartNextCooldown()
    {
        if (isCooldownRunning || cooldownQueue.Count == 0)
            return;

        int usedIndex = cooldownQueue.Dequeue();
        int visualIndex = visualCooldownIndex++;

        Debug.Log($"[COOLDOWN ����] ���� �ε���={usedIndex}, �ð��� �ε���={visualIndex}");

        StartCoroutine(CooldownRoutine(usedIndex, visualIndex));
    }

    private IEnumerator CooldownRoutine(int usedIndex, int visualIndex)
    {
        isCooldownRunning = true;

        float timer = cooldownTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float ratio = 1f - Mathf.Clamp01(timer / cooldownTime);
            currentStackBars[visualIndex].fillAmount = ratio;
            yield return null;
        }

        currentStackBars[visualIndex].fillAmount = 1f;
        isCoolingDown[usedIndex] = false;
        currentStacks++;
        Debug.Log($"[COOLDOWN �Ϸ�] ���� ���� ����: {usedIndex}, �ð� �� �ʱ�ȭ: {visualIndex}");

        UpdateStackUI();

        isCooldownRunning = false;

        if (currentStacks == currentStackBars.Length)
        {
            visualCooldownIndex = 0;
            Debug.Log("[RESET] ��� ���� ������, �ð� �ε��� �ʱ�ȭ");
        }

        TryStartNextCooldown();
    }

    private void UpdateStackUI()
    {
        int filled = 0;

        // �ð������� ���ʺ��� ä�� (0 -> 1 -> 2)
        for (int i = 0; i < currentStackBars.Length; i++)
        {
            if (!isCoolingDown[i] && filled < currentStacks)
            {
                currentStackBars[i].fillAmount = 1f;
                filled++;
            }
            else if (!isCoolingDown[i])
            {
                currentStackBars[i].fillAmount = 0f;
            }
            // ��ٿ� ���� �� �ǵ帮�� ���� (Coroutine�� ó�� ��)
        }
        DebugPrintStackStatus();
    }

    void DebugPrintStackStatus()
    {
        string status = "";
        for (int i = 0; i < currentStackBars.Length; i++)
        {
            status += $"[{i}] fill: {currentStackBars[i].fillAmount:F2} | ";
        }
        Debug.Log($"[���� ����] {status}");
    }
}
