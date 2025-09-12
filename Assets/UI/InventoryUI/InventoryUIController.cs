using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

/// <summary>
/// �κ��丮 UI�� ������ ���ӿ�����Ʈ�� ����ϰ��ϴ� Ŭ����
/// ��� ���
/// 1. UXML/USS�� ���� UI�� ȭ�鿡 ���
/// 2. �κ��丮 ���� 25��(slot-x-y)�� �ڵ忡�� ã��
/// 3. ��� ���� 3��(equipslot-1, 2, 3)�� �ڵ忡�� ã��
/// 4. �� ���� Ŭ�� �� � ���� �Ͼ���� ��� -> ���Ŀ� ��������
/// 5. �׽�Ʈ ������ ������ �ϳ��� ù ��° ���Կ� ǥ��
/// 6. drag&drop ���
/// </summary>
public class InventoryUIController : MonoBehaviour
{
    public VisualTreeAsset inventoryUXML;       // UXML ������ �����Ϳ��� ����� ���� �� �ְ� ���ִ� ����
    public StyleSheet inventoryUSS;             // USS ������ '' ���ִ� ����
    public InventoryDrag_Drop drag_Drop;        // Drag_Drop Ŭ���� �ν��Ͻ� 
    public static InventoryUIController Instance;

    private int draggingSlotIndex = -1;                         // �巡�� ������ ���Թ�ȣ�� ����ϴ� ���� �����ڸ����� �������� ���� �� ���
    private VisualElement root;                                 // UXML���� ���� UI ��ü�� ��Ÿ���� �ֻ�� ���
    private List<InventorySlotData> inventorySlots = new();         // �κ��丮 ���� 25���� �����ϴ� ����Ʈ.
    private List<VisualElement> slotElements = new();           // ���콺 ��ġ ��� Ž����
    private List<EquipSlotData> equipSlots = new();             // ��� ���� 3���� �����ϴ� ����Ʈ.

    private TooltipController tooltipController = new();

    //[Header("���� UI")]
    public VisualTreeAsset tooltipUXML;

    [SerializeField] private ChooseOne chooseone;

    private void Awake()
    {
        // ���� �ε�Ǳ�� ������, ������Ʈ ����(OnEnable ��)�� ������� �ʵ��� ��Ȱ��ȭ
        this.enabled = false;

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // ù ���� �ν��Ͻ��� �ڽ��� ����ϰ� ����
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

    }

    /// <summary>
    /// ������ ������ ��, ������Ʈ�� Ȱ��ȭ �� ����Ǵ� �Լ�
    /// UI�� �ҷ����� ���Ե��� ã�Ƽ� ����, �׽�Ʈ �������� ��ġ, �巡�� ó���� �غ���
    /// </summary>
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();    // UIDocument ������Ʈ�� ������
        root = uiDocument.rootVisualElement;
        root.styleSheets.Add(inventoryUSS);             // ��Ÿ�� ��Ʈ �߰�
        Instance = this;

        // ���Ե��� �ʱ�ȭ�ϰ� ã�� �Լ�
        InitInventorySlots();
        drag_Drop = new InventoryDrag_Drop(root);       // �巡�� �ý��� �غ�
        InitEquipSlots();

        LoadInventory();

        tooltipController.Initialize(root, tooltipUXML);


        // UIâ ��𿡼� ���콺�� ���� ������ �巡�� ���� ó���� �ϰ� ����� ���� �κ�


        root.RegisterCallback<PointerUpEvent>(evt =>
        {
            if (!drag_Drop.isDragging) return;

            VisualElement dropTarget = GetSlotUnderMouse(evt.position);
            int fromIndex = drag_Drop.fromSlotIndex;

            if (dropTarget != null)
            {
                int toIndex = slotElements.IndexOf(dropTarget);
                if (toIndex >= 0 && toIndex != fromIndex)
                {
                    var temp = inventorySlots[toIndex].weaponData;
                    inventorySlots[toIndex].SetItem(drag_Drop.currentItem);
                    inventorySlots[fromIndex].SetItem(temp);
                }
                else
                {
                    var equipSlot = equipSlots.Find(slot => slot.slotElement == dropTarget);
                    if (equipSlot != null)
                    {
                        WeaponStatPackage stats = EquipItem(equipSlot, drag_Drop.currentItem);
                        if (stats != null)
                        {
                            // ���� ���� ����
                            var player = GameObject.FindWithTag("Player");
                            if (player != null && player.TryGetComponent<Attack_Damage>(out var attack))
                            {
                                attack.SetWeaponStats(stats);
                            }
                            inventorySlots[fromIndex].Clear();
                        }
                        else
                            inventorySlots[fromIndex].SetItem(drag_Drop.currentItem);
                    }
                    else
                    {
                        inventorySlots[fromIndex].SetItem(drag_Drop.currentItem);
                    }
                }
            }
            else
            {
                inventorySlots[fromIndex].SetItem(drag_Drop.currentItem); // ����ġ ����
            }

            drag_Drop.EndDrag();
            draggingSlotIndex = -1;
        });

    }

    void InitInventorySlots()
    {
        var itemSlotRoot = root.Q<VisualElement>("itemslot");     // UXML���� name="itemslot"�� �θ� ��Ҹ� ������

        // 5x5 ������ ���鼭 slot-0-0 ~ slot-4-4 ������ �̸��� ����
        for (int y = 0; y < 5; y++) // row
        {
            for (int x = 0; x < 5; x++) // col
            {
                string slotName = $"slot-{x}-{y}";
                var slot = itemSlotRoot.Q<VisualElement>(slotName);         // ������� �̸����� ���� VisualElement�� ã��
                if (slot != null)
                {
                    var slotData = new InventorySlotData(slot);                               // ã�� ������ ����Ʈ�� ����
                    int index = inventorySlots.Count;
                    inventorySlots.Add(slotData);
                    slotElements.Add(slot);

                    int capturedIndex = index;


                    // �巡�� �̺�Ʈ ����
                    slot.RegisterCallback<PointerDownEvent>(evt =>
                    {
                        if (slotData.weaponData != null)
                        {
                            draggingSlotIndex = capturedIndex;
                            slotData.slotElement.Clear();
                            drag_Drop.BeginDrag(slotData.weaponData, capturedIndex);
                        }
                    });

                    // �巡�� �̺�Ʈ ����
                    root.schedule.Execute(() =>
                    {
                        slot.RegisterCallback<PointerUpEvent>(evt =>
                        {
                            if (drag_Drop.isDragging)
                            {
                                // ��ü ��� ����
                                int toIndex = capturedIndex;
                                // ��ü �� ����s
                                int fromIndex = drag_Drop.fromSlotIndex;

                                if (fromIndex != toIndex)
                                {
                                    // ������ ��ü
                                    var temp = inventorySlots[toIndex].weaponData;
                                    inventorySlots[toIndex].SetItem(drag_Drop.currentItem);
                                    inventorySlots[fromIndex].SetItem(temp);
                                }
                                else
                                {
                                    inventorySlots[fromIndex].SetItem(drag_Drop.currentItem);
                                }

                                drag_Drop.EndDrag();
                                draggingSlotIndex = -1;
                            }
                        });
                    });

                    slot.RegisterCallback<MouseEnterEvent>(evt =>
                    {
                        if (!drag_Drop.isDragging)
                            tooltipController.Show(slotData.weaponData, evt.mousePosition);
                    });
                    slot.RegisterCallback<MouseLeaveEvent>(evt =>
                    {
                        tooltipController.Hide();
                    });
                }
            }
        }
    }


    void InitEquipSlots()
    {
        var equipRoot = root.Q<VisualElement>("equipslot");

        // ��� ���� 3���� ��ȸ��.
        for (int i = 1; i <= 3; i++)
        {
            var slotElement = equipRoot.Q<VisualElement>($"equipslot-{i}");             // UXML���� �̸����� �ش� ������ ã��.
            if (slotElement != null)
            {
                var slotData = new EquipSlotData(slotElement);
                equipSlots.Add(slotData);

                slotElement.RegisterCallback<MouseEnterEvent>(evt =>
                {
                    if (!drag_Drop.isDragging)
                        tooltipController.Show(slotData.weaponData, evt.mousePosition);
                });
                slotElement.RegisterCallback<MouseLeaveEvent>(evt =>
                {
                    tooltipController.Hide();
                });
            }
        }
    }



    private VisualElement GetSlotUnderMouse(Vector2 mousePos)
    {
        foreach (var slot in slotElements)
        {
            if (slot.worldBound.Contains(mousePos))
                return slot;
        }
        foreach (var equipSlot in equipSlots)
        {
            if (equipSlot.slotElement.worldBound.Contains(mousePos))
                return equipSlot.slotElement;
        }
        return null;
    }

    private WeaponStatPackage EquipItem(EquipSlotData equipSlot, WeaponPrefabData data)
    {
        if (!Ark_stat.MeetsRequirement(data))
        {
            Debug.Log($"[���� ����] �䱸 ���� ���� : ü�� {data.requiredHp}, �� {data.requiredStr}, ��ø{data.requiredDex}, ����{data.requiredInt} ");
            return null;
        }

        var oldWeapon = equipSlot.weaponData;
        if (oldWeapon != null)
        {
            bool returned = TryAddItem(oldWeapon);
            if (!returned)
            {
                Debug.LogWarning("���� ���⸦ �κ��丮�� �ǵ��� �� �����ϴ�.");
            }
        }

        equipSlot.SetItem(data);
        Debug.Log($"��� ������ : {data.grade} {data.type} ");
        return new WeaponStatPackage(
            data.attackPower,
            data.attackSpeed,
            data.critChance,
            data.critDamage,
            data.range
            );
    }
    /// <summary>
    /// �������� �κ��丮 �� ĭ�� �߰��Ϸ��� �õ��Ѵ�.
    /// �����ϸ� true, ����(�� ��)�ϸ� false
    /// </summary>
    public bool TryAddItem(WeaponPrefabData data)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].weaponData == null)
            {
                inventorySlots[i].SetItem(data);
                Debug.Log($"[������ ����] {data.grade} {data.type} �κ��丮 {i}�� ĭ�� �߰���");
                return true;
            }
        }

        Debug.Log("�κ��丮�� ���� á���ϴ�!");
        return false;
    }


    /// <summary>
    /// �κ��丮 ���� �Լ�
    /// </summary>
    public void SaveInventory()
    {
        List<WeaponSaveData> saveList = new();
        foreach (var slot in inventorySlots)
        {
            if (slot.weaponData != null)
            {
                saveList.Add(ConvertToSaveData(slot.weaponData));
            }
        }

        List<WeaponSaveData> equipList = new();
        foreach (var slot in equipSlots)
        {
            if (slot.weaponData != null)
            {
                equipList.Add(ConvertToSaveData(slot.weaponData));
            }
        }


        string json = JsonUtility.ToJson(new SaveWrapper { list = saveList, equippedList = equipList }, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "inventory.json"), json);
        Debug.Log("�κ��丮 ���� �Ϸ�");
    }

    private WeaponSaveData ConvertToSaveData(WeaponPrefabData weapon)
    {
        return new WeaponSaveData
        {
            weaponName = weapon.weaponName,
            grade = weapon.grade,
            type = weapon.type,
            level = weapon.level,
            description = weapon.description,

            sellprice = weapon.sellprice,

            attackPower = weapon.attackPower,
            attackSpeed = weapon.attackSpeed,
            critChance = weapon.critChance,
            critDamage = weapon.critDamage,
            range = weapon.range,

            iconPath = $"Weapon/{weapon.grade}/{weapon.type}/{weapon.grade}_{weapon.type}",

            requiredHp = weapon.requiredHp,
            requiredStr = weapon.requiredStr,
            requiredDex = weapon.requiredDex,
            requiredInt = weapon.requiredInt,
            requiredluk = weapon.requiredluk
        };
    }

    public void LoadInventory()
    {
        string path = Path.Combine(Application.persistentDataPath, "inventory.json");
        if (!File.Exists(path))
        {
            Debug.LogWarning("����� �κ��丮 ���� ����");
            return;
        }

        string json = File.ReadAllText(path);
        var wrapper = JsonUtility.FromJson<SaveWrapper>(json);

        if (wrapper == null || wrapper.list == null)
        {
            Debug.LogWarning("[Load Fail] Json �Ľ� ���� �Ǵ� ����Ʈ ����");
            return;
        }

        //Debug.Log($"[LOAD SUCCESS] ���� ����: {wrapper.list.Count}");

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].Clear(false);
            if (i < wrapper.list.Count)
            {
                var data = wrapper.list[i];
                inventorySlots[i].SetItem(ConvertToWeaponData(data));
                //Debug.Log($"[LoadInventory] ���� {i}���� ���� �ֱ� �õ� ��");
            }
        }

        for (int i = 0; i < equipSlots.Count; i++)
        {
            var slot = equipSlots[i];
            slot.Clear();

            if (wrapper.equippedList != null && i < wrapper.equippedList.Count && wrapper.equippedList[i] != null)
            {
                var weapon = ConvertToWeaponData(wrapper.equippedList[i]);
                slot.SetItem(weapon);

                if (i == 0)
                {
                    var player = GameObject.FindWithTag("Player");
                    if (player != null && player.TryGetComponent<Attack_Damage>(out var attack))
                    {
                        attack.SetWeaponStats(new WeaponStatPackage(
                            weapon.attackPower,
                            weapon.attackSpeed,
                            weapon.critChance,
                            weapon.critDamage,
                            weapon.range
                            ));

                    }
                }
            }
        }


        //Debug.Log("�κ��丮 �ε� �Ϸ�");
    }

    private WeaponPrefabData ConvertToWeaponData(WeaponSaveData data)
    {
        return new WeaponPrefabData
        {
            weaponName = data.weaponName,
            grade = data.grade,
            type = data.type,
            level = data.level,
            description = data.description,

            sellprice = data.sellprice,

            attackPower = data.attackPower,
            attackSpeed = data.attackSpeed,
            critChance = data.critChance,
            critDamage = data.critDamage,
            range = data.range,

            icon = LoadIconByName(data.iconPath),
            requiredHp = data.requiredHp,
            requiredStr = data.requiredStr,
            requiredDex = data.requiredDex,
            requiredInt = data.requiredInt,
            requiredluk = data.requiredluk
        };
    }

    private Sprite LoadIconByName(string iconPath)
    {
        //Debug.Log("[LOAD START]");

        if (string.IsNullOrEmpty(iconPath))
        {
            //Debug.LogWarning("�������� �������");
            return null;
        }

        var sprite = Resources.Load<Sprite>(iconPath); // Ȯ���� ���� ��θ�
        //Debug.Log($"[LoadIconByName] ��� : {iconPath}, ��� : {(sprite != null ? sprite.name : "null")}");

        return sprite;


    }
    public void ResetInventory()
    {
        root.schedule.Execute(() =>
        {
            foreach (var slot in new List<InventorySlotData>(inventorySlots))

                slot.Clear(true); // ������ + UI ��� ����        

            foreach (var equipSlot in new List<EquipSlotData>(equipSlots))

                equipSlot.Clear(true); // EquipSlotData ���� ����
        });

        // ����� ���� ���� (����)
        string path = Path.Combine(Application.persistentDataPath, "inventory.json");
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("[ResetInventory] ����� �κ��丮 ���� ������");
        }

        Debug.Log("[ResetInventory] �κ��丮 + ��� ���� �ʱ�ȭ �Ϸ�");
    }

    void OnApplicationQuit()
    {
        SaveInventory(); //  ���� ������ �ڵ� ����
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ResetInventory();
        }
    }
    /// <summary>
    /// �÷��̾ ������ �������� ����, �κ��丮�� ���� ���� �� �ܺο��� ȣ��
    /// </summary>
    public void OpenInventory()
    {
        if (!this.enabled)
        {
            this.enabled = true;     // OnEnable()�� ȣ��Ǹ鼭 UI�� ������� �巡��&��� ���� ������ �ʱ�ȭ�˴ϴ�
        }
    }
    /// <summary>
    /// �κ��丮 ���� ��
    /// </summary>
    public void CloseInventory()
    {
        if (this.enabled)
        {
            this.enabled = false;    // OnDisable()�� ȣ��Ǹ鼭 UI�� �������, �巡�� �̺�Ʈ�� �����˴ϴ�
        }
    }

    [System.Serializable]
    private class SaveWrapper
    {
        public List<WeaponSaveData> list;           // �κ��丮 ����
        public List<WeaponSaveData> equippedList;   // ��� ����
    }


}
