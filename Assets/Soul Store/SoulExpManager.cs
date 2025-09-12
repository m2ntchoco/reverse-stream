using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class SoulExpManager
{
    private static string SavePath => Application.persistentDataPath + "Player_Data.json";

    public static void SaveSoulExp(int Soulexp)
    {
        PlayerExpManager.PlayerData.soulExp = Soulexp;

        var data = PlayerExpManager.PlayerData;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[�����] Soul Exp: {Soulexp}");

    }
    public static void CleatData()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("[������] ��ȥ ����ġ �ʱ�ȭ");
        }
    }

}