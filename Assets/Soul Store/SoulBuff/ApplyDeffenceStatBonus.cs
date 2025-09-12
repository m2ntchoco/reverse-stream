using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using Unity.VisualScripting;

public class ApplyDeffenceStatBonus
{
    private MonoBehaviour mono;
    private HpGenerate hp;
    //ü�� ui
    public HealthBarUIController hpUI;  // �ν����Ϳ��� ���� ������ ��
    public static bool MaxHpChange = false;

    public void Init()
    {
        mono = mono.GetComponent<MonoBehaviour>();
    }

    public static float heal = 0;
    public static void applyDeffenceStatBonus()
    {
        UnityEngine.Debug.Log($"���� �� �ִ� ü�� : {PlayerHealth.maxHP}");
        UnityEngine.Debug.Log("[applyDeffenceStatBonus] �Լ� ȣ���");
        if (SoulBuffManager.IsButtonUnlocked("deffence1"))
        {
            UnityEngine.Debug.Log("deffence1�� ������ �����");
            PlayerHealth.maxHP = 200;
            PlayerHealth.currentHP = PlayerHealth.maxHP;
            MaxHpChange = true;
            UnityEngine.Debug.Log($"���� �� �ִ� ü�� : {PlayerHealth.maxHP}");
            var ui = UnityEngine.Object.FindFirstObjectByType<HealthBarUIController>();
            ui?.RefreshFromPlayerHealth();
            SaveManager.Instance.SaveNow();
        }
    }

    public void MaxHpChangeNow()
    {
        if (MaxHpChange)
        {
            if (hpUI != null)
            {
                hpUI.SetHP((int)PlayerHealth.currentHP, (int)PlayerHealth.maxHP);
                MaxHpChange = false;

            }
        }
        else
        {
            UnityEngine.Debug.Log("�ִ�ü���� ��ȭ���� �����ϴ�.");
            return;
        }

    }
}

