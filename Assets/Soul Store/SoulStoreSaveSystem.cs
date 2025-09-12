using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public static class PlayerDataSaveSystem
{
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "Player_Data.json");
    public static void Save(VisualElement root)
    {
        var data = PlayerExpManager.PlayerData;

        foreach (var category in new[] { "attack", "deffence", "luck" })
        {
            for (int i = 0; i < 9; i++)
            {
                var btn = root.Q<Button>($"{category}{i}");
                if (btn != null && btn.userData is bool clicked && clicked)
                {
                    string btnId = $"{category}{i}";

                    // �̹� �߰��� ��ư���� �˻� ��, ������ �߰� (�ߺ� ����)
                    if (!data.unlockedButtons.Contains(btnId))
                    {
                        data.unlockedButtons.Add(btnId);
                    }
                }
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SaveFilePath, json, Encoding.UTF8);
        Debug.Log($"SoulStore �����: {SaveFilePath}");
    }

    public static PlayerData Load()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogWarning("����� �ҿ� ���� �����Ͱ� �����ϴ�.");
            return null;
        }
        //Debug.Log("[�ε�] unlockedButtons: " + string.Join(", ", PlayerExpManager.PlayerData.unlockedButtons));
        string json = File.ReadAllText(SaveFilePath, Encoding.UTF8);
        return JsonUtility.FromJson<PlayerData>(json);
    }
}
