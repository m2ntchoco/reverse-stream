#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO;

public class WeaponPrefabGenerator : EditorWindow
{
    [MenuItem("Tools/���� ������ ����")]
    public static void ShowWindow()
    {
        GetWindow<WeaponPrefabGenerator>("������ ������");
    }

    // �⺻ ���� ��
    string weaponName = "������ â";
    Sprite icon;
    WeaponGrade grade = WeaponGrade.Epic;
    WeaponType type = WeaponType.Spear;
    int level = 30;
    string description = "�� ����� �ſ� ��ī�Ӵ�.";

    int price = 1000;
    int sellprice = 500;

    int reqHp, reqStr, reqDex, reqInt, reqLuk = 0;

    float power = 100f;
    float speed = 1.0f;
    float critChance = 8f;
    float critDamage = 180f;
    float range = 3.0f;

    private void OnGUI()
    {

        GUILayout.Label("�⺻ ����", EditorStyles.boldLabel);
        weaponName = EditorGUILayout.TextField("���� �̸�", weaponName);
        grade = (WeaponGrade)EditorGUILayout.EnumPopup("���", grade);
        type = (WeaponType)EditorGUILayout.EnumPopup("���� ����", type);
        level = EditorGUILayout.IntSlider("����", level, 10, 50);
        icon = (Sprite)EditorGUILayout.ObjectField("���� �̹��� (Sprite)", icon, typeof(Sprite), false);
        description = EditorGUILayout.TextField("���� ����", description);

        GUILayout.Space(10);
        GUILayout.Label("���� ����", EditorStyles.boldLabel);
        price = EditorGUILayout.IntField("���Ű�", price);
        sellprice = EditorGUILayout.IntField("�ǸŰ�", sellprice);

        GUILayout.Space(10);
        GUILayout.Label("�䱸 �ɷ�ġ", EditorStyles.boldLabel);
        reqHp = EditorGUILayout.IntField("ü��", reqHp);
        reqStr = EditorGUILayout.IntField("��", reqStr);
        reqDex = EditorGUILayout.IntField("��ø", reqDex);
        reqInt = EditorGUILayout.IntField("����", reqInt);
        reqLuk = EditorGUILayout.IntField("��", reqLuk);

        GUILayout.Space(10);
        GUILayout.Label("���� �Ӽ�", EditorStyles.boldLabel);
        power = EditorGUILayout.FloatField("���ݷ�(���� ���ݷ�)", power);
        speed = EditorGUILayout.FloatField("���ݼӵ�", speed);
        critChance = EditorGUILayout.FloatField("ũ��Ƽ�� Ȯ��", critChance);
        critDamage = EditorGUILayout.FloatField("ũ��Ƽ�� ������", critDamage);
        range = EditorGUILayout.FloatField("���� ��Ÿ�", range);

        if (GUILayout.Button("������ ����"))
        {
            CreateWeaponPrefab();
        }
    }

    void CreateWeaponPrefab()
    {
        // (1) UI�� �̸� ���� ����
        if (string.IsNullOrWhiteSpace(weaponName))
        {
            Debug.LogError("���� �̸�(weaponName)�� �Է��ؾ� �մϴ�.");
            return;
        }

        // (2) ��� ������ �̸� = ���� ����
        string baseName = $"{grade}_{type}_{level}";
        string folder = "Assets/PreFab/GeneratedWeaponPrefabs";
        Directory.CreateDirectory(folder);

        // (3) ���ϸ� �ߺ� �˻� �� ���� ���� ��� Ȯ��
        string path = $"{folder}/{baseName}.prefab";
        int counter = 1;
        while (File.Exists(path))
        {
            path = $"{folder}/{baseName}_{counter}.prefab";
            counter++;
        }

        // (4) GameObject�� �׻� baseName ����
        GameObject obj = new GameObject(baseName);

        var renderer = obj.AddComponent<SpriteRenderer>();
        if (icon != null) renderer.sprite = icon;
        obj.tag = "Weapon";

        // (5) WeaponPrefabData ����
        var groundItem = obj.AddComponent<GroundItem>();
        groundItem.data = new WeaponPrefabData
        {
            weaponName = weaponName,  // <- UI/������ �̸�
            grade = grade,
            type = type,
            level = level,
            icon = icon,
            description = description,
            price = price,
            sellprice = sellprice,
            requiredHp = reqHp,
            requiredStr = reqStr,
            requiredDex = reqDex,
            requiredInt = reqInt,
            requiredluk = reqLuk,
            attackPower = power,
            attackSpeed = speed,
            critChance = critChance,
            critDamage = critDamage,
            range = range
        };

        var col = obj.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        // (6) ������ ����
        PrefabUtility.SaveAsPrefabAsset(obj, path);
        DestroyImmediate(obj);

        Debug.Log($"������ ���� �Ϸ�: {path}");
    }

}
