using System.Collections.Generic;

[System.Serializable]
public class SerializableKeyValue
{
    public string key;
    public bool value;
}
[System.Serializable]
public class PlayerData
{
    public int playerLevel;
    public int playerExp;
    public int playerTotalExp;
    public int expToNextLevel;
    public int soulExp;
    public List<string> unlockedButtons = new(); // ���� ������ �״�� ����
                                                 // ���� ���� ����Ʈ
    public List<SerializableKeyValue> appliedBuffList = new();

    // ��Ÿ�� ���� ��ųʸ� (������� ����)
    [System.NonSerialized]
    public Dictionary<string, bool> appliedBuffs = new();
}
