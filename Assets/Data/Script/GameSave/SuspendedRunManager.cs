// SuspendedRunManager.cs
using System;
using System.Collections;
using UnityEngine;

public static class SuspendedRunManager
{
    public static void SaveCurrentRun()
    {
        if (PlayerExpManager.PlayerData == null) return; // NRE ����

        var data = new SuspendedData
        {
            playerLevel = PlayerExpManager.PlayerData.playerLevel,
            playerExp = PlayerExpManager.PlayerData.playerExp,
            totalExp = PlayerExpManager.PlayerData.playerTotalExp,
            expToNextLevel = PlayerExpManager.PlayerData.expToNextLevel,
            currentHP = PlayerHealth.currentHP,
            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            savedTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
        };

        SuspendedData.Save(data);
    }


    public static void ResumeIfExists()
    {
        // 1) ��Ÿ �ε� ���� (�ҿ�/����/�ر� ��ư)
        PlayerExpSave.LoadExp();                       // PlayerExpManager.PlayerData ä����
        if (PlayerExpManager.PlayerData == null)
            PlayerExpManager.InitPlayerData();         // Ȥ�� ������ ������ �� ���

        // 2) �������(��) ������ ����
        if (SuspendedData.Exists())
        {
            var data = SuspendedData.Load();
            if (data != null)
            {
                var pd = PlayerExpManager.PlayerData;
                pd.playerLevel = data.playerLevel;
                pd.playerExp = data.playerExp;
                pd.playerTotalExp = data.totalExp;
                pd.expToNextLevel = data.expToNextLevel;

                PlayerHealth.currentHP = Mathf.Max(1, data.currentHP);
                SuspendedData.Delete();                // ��ȸ�� ���̺� ����
            }
        }

        // 3) ���� ������ (������Ʈ���� ��� ������)
        BootstrapRunner.RunAfterOneFrame(ReapplySoulBuffsOnce);
    }

    private static void ReapplySoulBuffsOnce()
    {
        // ����� �ر� ��ư �������� ���� ��ü ������
        SoulBuffManager.ApplyAllUnlockedBuffs();
    }
}



