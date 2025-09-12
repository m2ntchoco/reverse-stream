using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class DropManager : MonoBehaviour
{
    public static DropManager Instance { get; private set; }

    public List<DropTableData> stageTables;

    // �ε��� SO (Resources�� ����� �� ��Ÿ�ӿ� �ε�)
    private WeaponPrefabIndex prefabIndex;

    public void Initialize(string jsonPath)
    {
        stageTables = DropTableLoader.LoadDropTables(jsonPath);
    }

    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "GreedStageData.json");
        Initialize(path);

        foreach (var table in stageTables)
        {
            //Debug.Log($"[������̺�] {table.stageName} ���� ��� Ȯ��: {table.weaponDropRate}%");

            table.allowedTypes = new List<WeaponType>();
            foreach (var str in table.allowedTypeStrings)
            {
                if (System.Enum.TryParse(str, true, out WeaponType result))
                    table.allowedTypes.Add(result);
                else
                    Debug.LogWarning($"WeaponType ��ȯ ����: {str}");
            }
        }

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // �ε��� �ε�(��Ÿ�ӿ�)
        prefabIndex = Resources.Load<WeaponPrefabIndex>("WeaponPrefabIndex");
        if (prefabIndex == null)
            Debug.LogWarning("[DropManager] WeaponPrefabIndex�� �����ϴ�. (Tools > Weapons > Rebuild Prefab Index ���� �ʿ�)");
    }

    public void SpawnDrop(Vector2 dropPosition)
    {
        string currentScene = SceneManager.GetActiveScene().name.Trim().ToLower();
        SpawnDrop(currentScene, dropPosition);
    }

    public void SpawnDrop(string stageName, Vector2 dropPosition)
    {
        string normalized = stageName.Trim().ToLower();
        var table = stageTables.Find(t => t.stageName.Trim().ToLower() == normalized);
        if (table == null)
        {
            Debug.LogWarning($"�������� {stageName}�� ���� ��� ���̺��� �����ϴ�.");
            return;
        }

        float roll = Random.Range(0f, 100f);
        if (roll > table.weaponDropRate)
        {
            Debug.Log($"���� ������� ���� (Ȯ��: {table.weaponDropRate}%, roll: {roll:F2})");
            return;
        }

        WeaponGrade grade = RollGrade(table);
        WeaponType type = RollWeaponType(table.allowedTypes);
        int level = RollWeaponLevel(table.allowedLevels);

        GameObject prefab = LoadRandomWeaponPrefab(grade, type, level);
        if (prefab == null)
        {
            Debug.LogWarning($"�������� ã�� �� �����ϴ�: {grade}_{type}_{level}");
            return;
        }
        Instantiate(prefab, dropPosition, Quaternion.identity);
    }

    private WeaponGrade RollGrade(DropTableData table)
    {
        float roll = Random.Range(0f, 100f);
        float cumulative = 0f;

        cumulative += table.legendaryRate;
        if (roll < cumulative) return WeaponGrade.Legendary;

        cumulative += table.uniqueRate;
        if (roll < cumulative) return WeaponGrade.Unique;

        cumulative += table.epicRate;
        if (roll < cumulative) return WeaponGrade.Epic;

        cumulative += table.rareRate;
        if (roll < cumulative) return WeaponGrade.Rare;

        return WeaponGrade.Common;
    }

    private WeaponType RollWeaponType(List<WeaponType> allowed)
    {
        return allowed[Random.Range(0, allowed.Count)];
    }

    private int RollWeaponLevel(List<int> allowedLevels)
    {
        int index = Random.Range(0, allowedLevels.Count);
        return allowedLevels[index];
    }

    // �� �ٽ�: �ε��� �켱, �����Ϳ��� ���� ����
    private GameObject LoadRandomWeaponPrefab(WeaponGrade grade, WeaponType type, int level)
    {
        string key = $"{grade}_{type}_{level}";
        Debug.Log($"[DropManager] ã�� Ű: {key}");

        if (prefabIndex != null)
        {
            var list = prefabIndex.Get(key);
            if (list != null && list.Count > 0)
            {
                Debug.Log($"[DropManager] �ε������� {list.Count}�� �߰�");
                return list[Random.Range(0, list.Count)];
            }
            else
            {
                Debug.LogWarning($"[DropManager] �ε����� ����: {key}");
            }
        }
        // 1) ��Ÿ��/������ ����: �ε��� �켱
        if (prefabIndex != null)
        {
            var list = prefabIndex.Get(key);
            if (list != null && list.Count > 0)
                return list[Random.Range(0, list.Count)];
        }

#if UNITY_EDITOR
        // 2) ������ ����: �ε��� ���ų� ���� ���̶�� AssetDatabase�� ��� Ž��
        string[] guids = AssetDatabase.FindAssets($"t:Prefab {key}", new[] { "Assets/PreFab/GeneratedWeaponPrefabs" });
        if (guids != null && guids.Length > 0)
        {
            string chosenPath = AssetDatabase.GUIDToAssetPath(guids[Random.Range(0, guids.Length)]);
            return AssetDatabase.LoadAssetAtPath<GameObject>(chosenPath);
        }
#endif

        return null;
    }
}
