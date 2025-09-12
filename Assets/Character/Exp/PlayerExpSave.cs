using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using UnityEngine;

public class PlayerExpSave
{
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "Player_Data.json");

    public static void SavedExp()
    {
        var data = PlayerExpManager.PlayerData;

        data.appliedBuffList = data.appliedBuffs
       .Select(kv => new SerializableKeyValue { key = kv.Key, value = kv.Value })
       .ToList();

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SaveFilePath, json, Encoding.UTF8);
        Debug.Log("PlayerData ���� �Ϸ�: " + SaveFilePath);
    }

    public static void LoadExp()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogWarning("����� PlayerData ���� ����");
            return;
        }

        string json = File.ReadAllText(SaveFilePath, Encoding.UTF8);
        var data = JsonUtility.FromJson<PlayerData>(json);

        // List �� Dictionary ����
        data.appliedBuffs = data.appliedBuffList
            .ToDictionary(entry => entry.key, entry => entry.value);

        PlayerExpManager.PlayerData = data;

        PlayerExpManager.PlayerData = data;
        PlayerExpManager.currentExp = data.playerExp;
        PlayerExpManager.currentLevel = data.playerLevel;
        PlayerExpManager.totalExp = data.playerTotalExp;
        PlayerExpManager.expToNextLevel = data.expToNextLevel;

        //Debug.Log("PlayerData �ε� �Ϸ�");
    }


}