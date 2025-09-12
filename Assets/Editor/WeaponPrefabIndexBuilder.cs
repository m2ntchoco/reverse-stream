#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class WeaponPrefabIndexBuilder
{
    // ���� �������� ����ִ� ������ (�ʿ��ϸ� �߰�)
    private static readonly string[] SearchFolders = new[]
{
    // �ʰ� ��ٰ� �� ���
    "Assets/PreFab/GeneratedWeaponPrefabs",

    // ���� ���� ö��/���������� ��� Ŀ��
    "Assets/Prefab/GeneratedWeaponPrefabs",
    "Assets/Prefabs/GeneratedWeaponPrefabs",
    "Assets/PreFab",
    "Assets/Prefab",
    "Assets/Prefabs"
};

    // �ε��� SO ���� ��ġ (��Ÿ�� �ε带 ���� Resources�� ��)
    private const string IndexAssetPath = "Assets/Resources/WeaponPrefabIndex.asset";

    [MenuItem("Tools/Weapons/Rebuild Prefab Index")]
    public static void RebuildIndex()
    {
        var index = AssetDatabase.LoadAssetAtPath<WeaponPrefabIndex>(IndexAssetPath);
        if (index == null)
        {
            var dir = Path.GetDirectoryName(IndexAssetPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            index = ScriptableObject.CreateInstance<WeaponPrefabIndex>();
            AssetDatabase.CreateAsset(index, IndexAssetPath);
        }

        var map = new Dictionary<string, List<GameObject>>(System.StringComparer.OrdinalIgnoreCase);

        int totalFound = 0;
        int skippedBadName = 0;

        foreach (var folder in SearchFolders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                Debug.LogWarning($"[IDX] ���� ����: {folder}");
                continue;
            }

            var guids = AssetDatabase.FindAssets("t:Prefab", new[] { folder });
            Debug.Log($"[IDX] {folder} ���� Prefab {guids.Length}�� �߰�");
            totalFound += guids.Length;

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (!go) continue;

                var tokens = go.name.Split('_');
                if (tokens.Length < 3)
                {
                    Debug.LogWarning($"[IDX] �̸� ��Ģ ����ġ �� ��ŵ: {go.name}  (���: {path})");
                    skippedBadName++;
                    continue;
                }

                string key = $"{tokens[0]}_{tokens[1]}_{tokens[2]}";
                if (!map.TryGetValue(key, out var list))
                {
                    list = new List<GameObject>();
                    map.Add(key, list);
                }
                list.Add(go);
            }
        }

        Debug.Log($"[IDX] �� ��ĵ ���: {totalFound}�� / ��Ģ ����ġ ��ŵ: {skippedBadName}��");

        index.entries.Clear();
        foreach (var kv in map)
        {
            index.entries.Add(new WeaponPrefabIndex.Entry { key = kv.Key, prefabs = kv.Value });
            Debug.Log($"[IDX] ��� Ű: {kv.Key} / ������ {kv.Value.Count}��");
        }

        EditorUtility.SetDirty(index);
        AssetDatabase.SaveAssets();
        Debug.Log($"[WeaponPrefabIndex] ����� �Ϸ�: {index.entries.Count}��");
    }
    // ����: �÷��� �� �ڵ� ���� (�ε��� ������ �ڵ� ����)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureIndexOnPlay()
    {
        var idx = Resources.Load<WeaponPrefabIndex>("WeaponPrefabIndex");
        if (idx == null)
        {
            RebuildIndex();
            Debug.Log("[WeaponPrefabIndex] ��� �ڵ� �����߾��.");
        }
    }
}
#endif
