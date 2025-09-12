using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SuspendedData
{
    public int playerLevel;
    public int playerExp;
    public int totalExp;
    public int expToNextLevel;
    public float currentHP;
    public string sceneName;
    public string savedTime;
    public List<string> unlockedButtons = new(); // ���� ������ �״�� ����
                                                 // ���� ���� ����Ʈ
    public List<SerializableKeyValue> appliedBuffList = new();

    // ��Ÿ�� ���� ��ųʸ� (������� ����)
    [System.NonSerialized]
    public Dictionary<string, bool> appliedBuffs = new();
    private static string SavePath =>
        Path.Combine(Application.persistentDataPath, "Suspended_Run.json");

    public static void Save(SuspendedData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[SuspendedData] ���� �Ϸ�: {SavePath}");
    }

    public static SuspendedData Load()
    {
        if (!Exists()) return null;


        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<SuspendedData>(json);
    }

    public static void Delete()
    {
        if (Exists()) File.Delete(SavePath);
        Debug.Log("[SuspendedData] ���� �Ϸ�");
    }

    public static bool Exists()
    {
        return File.Exists(SavePath);
    }
}