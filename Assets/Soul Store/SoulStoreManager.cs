using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// �ҿ� ���� ��ü ���� + ESC ���� ����(BackPanelBase ���)
/// </summary>
[RequireComponent(typeof(UIDocument))]
public class SoulStoreManager : BackPanelBase
{
    // ESC ���� �켱����(���� ����̶� �ǹ��� ��) - ����/�κ��丮 �迭�� ����
    public override int BackPriority => 100;

    [Header("�ܺ� ����")]
    [SerializeField] private UIDocument soulToolTipDocument; // ���� ���� ����(����)

    // UI ��Ʈ(BackPanelBase�� ����� viewRoot�� ��)
    private VisualElement root;
    private Label soulLabel;

    // ����
    private VisualElement soulToolTip;
    private Label soulToolTipLabel;

    // ���� �ؽ�Ʈ (���� �ذ�: Ŭ���� ���ο� �ݵ�� ����)
    private readonly string[] attackDescriptions = new[]
    {
        "������ ����\n\n �� ���� + 1",
        "������ �ڵ�\n\n �̵� �ӵ� +10%",
        "������ ��մ� ��\n\n ġ��Ÿ Ȯ�� +5%",
        "������ Ŭ���� ���ϴ�\n\n �ִ� ü�� ��� �߰� ����",
        "�� ���� �ݸ�\n\n ġ��Ÿ ������ +10%",
        "������ �ϰ�\n\n �ǰ� �� 3�� �̳� ���� ������ +30%\n(��Ÿ�� 10��)",
        "���ֹ��� ����\n\n ���ݸ��� �߰� ���� �ӵ� ȹ��\n(�ִ� 5ȸ ��ø)"
    };

    private readonly string[] defenceDescriptions = new[]
    {
        "ö�� ����\n\n �޴� ���� 5% ����",
        "��ö�� ����\n\n �ִ� ü�� +100",
        "������ ȸ�� ����\n\n �ʴ� ü�� ��� +2%",
        "��� ����\n\n 30�ʸ��� �ڵ� �ߵ� ���� ��ȿȭ",
        "������ ������\n\n �߰� ���� ���� 5%",
        "������ ��ȣ��\n\n ��ų ��� �� �� +10\n(��Ÿ�� 30��)",
        "������ ���ο���\n\n ü�� 1�� �� 5�ʰ� ���� �ߵ�\n(�� �÷��̸��� 1ȸ ���)"
    };

    private readonly string[] specialDescriptions = new[]
    {
        "ȥ���� ����\n\n ������ ȿ�� ȹ��\n(��, ����, ���� �� �ϳ�)",
        "Ž���� ����\n\n �ҿ� ȹ�淮 +20%",
        "����ǰ �ߵ���\n\n ������ ����� ���",
        "�ŷ��� ������\n\n ���� �ɼ� 1ȸ ���� ����",
        "������ �ູ\n\n ������ �ɷ�ġ ��·� +1",
        "�ٶ�ó��\n\n ��� Ƚ�� +1ȸ",
        "������ ����\n\n �� ���ȿ� �´� ���� ��� Ȯ�� ���"
    };

    // ���� ���
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, "Player_Data.json");

    // ===== �����ֱ� =====
    protected override void Awake()
    {
        base.Awake();
        root = viewRoot; // BackPanelBase�� name="Root" or ��Ʈ�� �����

    }

    private void Start()
    {
        Hide();
        UINavigator.Instance.Register(this);
        // ���� ���� �غ�
        if (soulToolTipDocument != null)
        {
            var tipRoot = soulToolTipDocument.rootVisualElement;
            soulToolTip = tipRoot.Q<VisualElement>("SoulToolTip");
            soulToolTipLabel = tipRoot.Q<Label>("SoulToolTipLabel");

            if (soulToolTip != null)
            {
                soulToolTip.style.display = DisplayStyle.None; // visible ��� display
                soulToolTip.style.position = Position.Absolute;
                soulToolTip.pickingMode = PickingMode.Ignore;
            }
            if (soulToolTipLabel != null)
            {
                soulToolTipLabel.style.whiteSpace = WhiteSpace.Normal;
                soulToolTipLabel.style.maxWidth = 380;
            }
        }

        soulLabel = root.Q<Label>("soulLabel");
        if (soulLabel != null && PlayerExpManager.PlayerData != null)
            soulLabel.text = $"Soul: {PlayerExpManager.PlayerData.soulExp}";

        SetupUnlockButtons("attack", 7, 50);
        SetupUnlockButtons("deffence", 7, 50); // ��ư name�� 'deffence'�� �Ǿ�����(���� ����)
        SetupUnlockButtons("special", 7, 100);

        UpdateAllUnlockStatus();
        LoadSoulStoreData();

        // ��Ʈ���� ���콺�� ������ ���� ���� ����
        root.RegisterCallback<MouseLeaveEvent>((_) =>
        {
            if (soulToolTip != null)
                soulToolTip.style.display = DisplayStyle.None;
        });
    }

    // ===== ESC ���� ����(����/�ݱ�) =====
    public override void Show()
    {
        base.Show();           // display:flex + ���� Open
        PauseManager.Pause();  // ���� ������ ���� ����
        UpdateAllUnlockStatus();
    }

    public override void Hide()
    {
        SaveSoulStoreData();
        base.Hide();           // display:none + ���� Close
        if (PauseManager.IsPaused)
            PauseManager.Resume();
    }

    // ===== ��ư/����/��� =====
    void SetButtonImage(Button btn, string imageName)
    {
        var tex = Resources.Load<Texture2D>(imageName);
        if (tex != null)
        {
            btn.style.backgroundImage = new StyleBackground(tex);
            btn.style.backgroundColor = Color.clear;
        }
    }

    void SetupUnlockButtons(string prefix, int count, int costPerUpgrade)
    {
        for (int i = 0; i < count; i++)
        {
            var btn = root.Q<Button>($"{prefix}{i}");
            if (btn == null)
            {
                Debug.LogWarning($"{prefix}{i} ��ư�� �� ã�Ҿ�!");
                continue;
            }

            int upgradeCost = costPerUpgrade + (i * 100);
            btn.text = $"�� ({upgradeCost})";
            btn.SetEnabled(false);
            int currentIndex = i;

            btn.clicked += () =>
            {
                if (PlayerExpManager.PlayerData == null) return;

                if (PlayerExpManager.PlayerData.soulExp < upgradeCost)
                {
                    Debug.LogWarning($"�ҿ��� ������! {upgradeCost} �ʿ���!");
                    return;
                }

                // �ҿ� ���� + ��� ó��
                PlayerExpManager.PlayerData.soulExp -= upgradeCost;
                btn.userData = true;
                btn.SetEnabled(false);

                string key = $"{prefix}{currentIndex}";
                if (!PlayerExpManager.PlayerData.unlockedButtons.Contains(key))
                    PlayerExpManager.PlayerData.unlockedButtons.Add(key);

                // ���� ����
                SoulBuffManager.ApplyBuffByButtonId($"{prefix}{currentIndex}");

                // UI ���� (FindFirstObjectByType�� ��� �ذ�)
                var ui = Object.FindFirstObjectByType<UIController>();
                if (ui != null) ui.RefreshUI();

                if (soulLabel != null)
                    soulLabel.text = $"Soul: {PlayerExpManager.PlayerData.soulExp}";

                UpdateAllUnlockStatus();
            };

            // Tooltip On
            btn.RegisterCallback<MouseEnterEvent>((evt) =>
            {
                if (soulToolTip != null && soulToolTipLabel != null)
                {
                    string desc = prefix switch
                    {
                        "attack" => attackDescriptions[currentIndex],
                        "deffence" => defenceDescriptions[currentIndex],
                        "special" => specialDescriptions[currentIndex],
                        _ => "���� ����"
                    };
                    soulToolTipLabel.text = desc;

                    Vector2 mousePos = root.WorldToLocal(evt.mousePosition);
                    float tooltipWidth = soulToolTip.resolvedStyle.width > 0 ? soulToolTip.resolvedStyle.width : 400f;
                    float tooltipHeight = soulToolTip.resolvedStyle.height > 0 ? soulToolTip.resolvedStyle.height : 500f;
                    float viewWidth = root.resolvedStyle.width;
                    float viewHeight = root.resolvedStyle.height;

                    float xOffset = 20f, yOffset = 20f;
                    float x = (mousePos.x + tooltipWidth + xOffset > viewWidth)
                        ? mousePos.x - tooltipWidth - xOffset
                        : mousePos.x + xOffset;
                    float y = Mathf.Clamp(mousePos.y + yOffset, 0, viewHeight - tooltipHeight);

                    soulToolTip.style.left = x;
                    soulToolTip.style.top = y;

                    soulToolTip.style.display = DisplayStyle.None;
                    root.schedule.Execute(() =>
                    {
                        if (soulToolTip.style.display == DisplayStyle.None)
                            soulToolTip.style.display = DisplayStyle.Flex;
                    }).ExecuteLater(1);
                }
            });

            // Tooltip Off
            btn.RegisterCallback<MouseLeaveEvent>((_) =>
            {
                if (soulToolTip != null)
                    soulToolTip.style.display = DisplayStyle.None;
            });
        }

        UpdateUnlockStatus(prefix, count, costPerUpgrade);
    }

    void UpdateUnlockStatus(string prefix, int count, int costPerUpgrade)
    {
        int currentSoul = PlayerExpManager.PlayerData != null ? PlayerExpManager.PlayerData.soulExp : 0;

        for (int i = 0; i < count; i++)
        {
            var btn = root.Q<Button>($"{prefix}{i}");
            if (btn == null) continue;

            int upgradeCost = costPerUpgrade + (i * 10);
            bool wasClicked = (btn.userData is bool clickedFlag) && clickedFlag;

            // ���� �ܰ� ��� ����
            bool isPrevUnlocked = (i == 0);
            if (i > 0)
            {
                var prevBtn = root.Q<Button>($"{prefix}{i - 1}");
                if (prevBtn != null && prevBtn.userData is bool prevClicked && prevClicked)
                    isPrevUnlocked = true;
            }

            bool canUnlock = !wasClicked && currentSoul >= upgradeCost && isPrevUnlocked;

            if (wasClicked)
            {
                btn.SetEnabled(false);
                SetButtonImage(btn, "button_unlocked");
            }
            else if (canUnlock)
            {
                btn.SetEnabled(true);
                SetButtonImage(btn, "button_active");
            }
            else
            {
                btn.SetEnabled(false);
                SetButtonImage(btn, "button_locked");
            }
        }
    }

    void UpdateAllUnlockStatus()
    {
        UpdateUnlockStatus("attack", 7, 50);
        UpdateUnlockStatus("deffence", 7, 50);
        UpdateUnlockStatus("special", 7, 100);
    }

    public void AddSoul(int amount)
    {
        if (PlayerExpManager.PlayerData == null) return;
        PlayerExpManager.PlayerData.soulExp += amount;

        if (soulLabel != null)
            soulLabel.text = $"Soul: {PlayerExpManager.PlayerData.soulExp}";

        UpdateAllUnlockStatus();
    }

    public void SaveSoulStoreData()
    {
        if (PlayerExpManager.PlayerData == null) return;

        var saveData = PlayerExpManager.PlayerData;

        foreach (var category in new[] { "attack", "deffence", "special" })
        {
            for (int i = 0; i < 9; i++)
            {
                var btn = root.Q<Button>($"{category}{i}");
                if (btn != null && btn.userData is bool clicked && clicked)
                {
                    string key = $"{category}{i}";
                    if (!saveData.unlockedButtons.Contains(key))
                        saveData.unlockedButtons.Add(key);
                }
            }
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SaveFilePath, json, Encoding.UTF8);
        // Debug.Log($"�ҿ� ���� ���� �Ϸ�: {SaveFilePath}");
    }

    public void LoadSoulStoreData()
    {
        if (!File.Exists(SaveFilePath)) return;

        string json = File.ReadAllText(SaveFilePath, Encoding.UTF8);
        var saveData = JsonUtility.FromJson<PlayerData>(json);
        if (saveData == null) return;

        if (PlayerExpManager.PlayerData != null)
            PlayerExpManager.PlayerData = saveData;

        if (soulLabel != null)
            soulLabel.text = $"Soul: {saveData.soulExp}";

        foreach (var category in new[] { "attack", "deffence", "special" })
        {
            for (int i = 0; i < 9; i++)
            {
                var btn = root.Q<Button>($"{category}{i}");
                if (btn == null) continue;

                string key = $"{category}{i}";
                bool isUnlocked = saveData.unlockedButtons.Contains(key);
                btn.userData = isUnlocked;
            }
        }

        UpdateAllUnlockStatus();
    }
}
