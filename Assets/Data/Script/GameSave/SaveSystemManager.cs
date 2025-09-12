using UnityEngine;

public static class SaveSystemManager
{

    // �� �̵�/�Ͻ����� ��: ���� ���� ����ؼ� ��� ����
    public static void SaveOnSceneTransition()
    {
        // ��(��ȸ��) ����
        SuspendedRunManager.SaveCurrentRun();

        // ��Ÿ(�ҿ�/����/�ر� ��) ����
        PlayerExpManager.SavePlayerData();

        // �� ��(����/���� �� ������Ʈ ���� ����)
        SaveManager.Instance.SaveNow();

        // �κ��丮 �� ���� �ý���
        InventoryUIController.Instance.SaveInventory();
    }

    // ����ڰ� �ɼ�â���� ������/���θ޴��� ���ư���/Alt+F4 ���
    public static void SaveOnExitLike()
    {
        SuspendedRunManager.SaveCurrentRun();
        PlayerExpManager.SavePlayerData();
        if (SaveManager.Instance != null) SaveManager.Instance.SaveNow();
        if (InventoryUIController.Instance != null) InventoryUIController.Instance.SaveInventory();
    }

    // ��� ��: �� ���(�߰����̺� ���� ����), ��Ÿ�� ����
    public static void SaveOnDeath()
    {
        // ���� SaveCurrentRun() ȣ������ �ʱ�
        PlayerExpManager.SavePlayerData();
        if (SaveManager.Instance != null) SaveManager.Instance.SaveNow();
        if (InventoryUIController.Instance != null) InventoryUIController.Instance.SaveInventory();
    }

    // ���� ���� ��: Resume ������ �ݿ�, ������ Init
    // ���� ����: ��Ÿ �� ������� �� ���� ������ ���� ������
    public static void ResumeOrInitAtStartup()
    {
        // �ҿ�/����(��Ÿ) ���� �ε�
        PlayerExpSave.LoadExp();
        if (PlayerExpManager.PlayerData == null) PlayerExpManager.InitPlayerData();

        // ��(�������) ��������
        SuspendedRunManager.ResumeIfExists(); // ���ο��� ���� ������ �ݿ�/����

        // ���� �����ӿ� �ҿ� ���� �ϰ� ������
        BootstrapRunner.RunAfterOneFrame(SoulBuffManager.ApplyAllUnlockedBuffs);
    }
}
