using UnityEngine;
using System.IO; // ������ �����ϰ� �ҷ����� ���� ����� ����ϱ� ���� ����

/// <summary>
/// statSaveData�� json���� �����ϰų� �ҷ����� �Ŵ��� Ŭ����
/// �� Ŭ������ ��𼭴� ���� �� �ֵ��� static Ŭ������ ������.
/// </summary>
public class SaveManager : MonoBehaviour
{
    /* 
    ������ ������ ��θ� �ڵ����� �������ִ� ���� 
    Application.persistentDataPath�� �÷����� ���� �ڵ����� ������ ���� ������ �˷���
    ��������� ex) C:/Users/user/AppData/LocalLow/ȸ���/���Ӹ�/Ark_stat.json ���� ����
    */
    private static string SavePath => Path.Combine(Application.persistentDataPath, "Ark_stat.json");

    public static SaveManager Instance { get; private set; }

    public static statSaveData StatData { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        DontDestroyOnLoad(gameObject);

        LoadOnce();
    }

    public void LoadOnce()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            StatData = JsonUtility.FromJson<statSaveData>(json);
        }
        else
        {
            StatData = new statSaveData();
        }

        Ark_stat.LoadFrom(StatData);
    }

    public void SaveNow()
    {
        Ark_stat.ApplyTo(StatData);
        string json = JsonUtility.ToJson(StatData, true);
        File.WriteAllText(SavePath, json);

        //Debug.Log($"[SAVE] ����� : {SavePath}");
    }

    

}
