using UnityEngine;
using UnityEngine.UIElements;

// �κ��丮/���� â�� ESC ���ÿ� �����ϱ� ���� BackPanelBase ���
[RequireComponent(typeof(UIDocument))]
public class UIController : BackPanelBase
{
    // (�ǹ̻�) �κ��丮�� �ٸ� �ͺ��� ���� ������ ���� �� ? ���� ����̶� ���� ������ ������ ����
    public override int BackPriority => 100;

    // UI ��ҵ�
    private Label remainingPointLabel;
    private Label hpLabel, strLabel, dexLabel, intLabel, LevelLabel;
    private Button hpPlus, strPlus, dexPlus, intPlus;

    private int prevLevel = -1;

    // base.Awake()�� viewRoot/doc ������ ����
    protected override void Awake()
    {
        base.Awake();

        // 1) UI ��� ã��
        LevelLabel = viewRoot.Q<Label>("Level");
        hpLabel = viewRoot.Q<Label>("hpLabel");
        strLabel = viewRoot.Q<Label>("strLabel");
        dexLabel = viewRoot.Q<Label>("dexLabel");
        intLabel = viewRoot.Q<Label>("intLabel");
        remainingPointLabel = viewRoot.Q<Label>("remainingPoint");

        hpPlus = viewRoot.Q<Button>("hpPlus");
        strPlus = viewRoot.Q<Button>("strPlus");
        dexPlus = viewRoot.Q<Button>("dexPlus");
        intPlus = viewRoot.Q<Button>("intPlus");

        // 2) ��ư �̺�Ʈ
        if (hpPlus != null) hpPlus.clicked += () => { if (Ark_stat.IncreaseStat(Ark_stat.StatType.Health)) RefreshUI(); };
        if (strPlus != null) strPlus.clicked += () => { if (Ark_stat.IncreaseStat(Ark_stat.StatType.Strength)) RefreshUI(); };
        if (dexPlus != null) dexPlus.clicked += () => { if (Ark_stat.IncreaseStat(Ark_stat.StatType.Dexterity)) RefreshUI(); };
        if (intPlus != null) intPlus.clicked += () => { if (Ark_stat.IncreaseStat(Ark_stat.StatType.Intelligence)) RefreshUI(); };

        // 3) ������ ���� + �׺�����Ϳ� ���(�Ͻ����� �ƴ�)
        Hide();
        UINavigator.Instance.Register(this);

        // ���� 1ȸ ����
        RefreshUI();
    }

    // �κ��丮 �г��� ������ ������ ������ �ʴ´�(Time.timeScale �ǵ帮�� ����)
    // BackPanelBase.Show/Hide�� �ڵ����� ���ÿ� �����ϹǷ� override �ʿ� ����.
    // ��Ŀ�� �ְ� ������ Show�� ��¦ Ȯ��
    public override void Show()
    {
        base.Show();
        PauseManager.Pause();   // �κ��丮 ���� ����
        hpPlus?.Focus();        // �ʿ� �� ��Ŀ��
    }

    public override void Hide()
    {
        base.Hide();
        PauseManager.Resume();  // �κ��丮 ������ ���� ����(��ø�̸� ������)
    }


    public void RefreshUI()
    {
        // PlayerExpManager.PlayerData�� �ʱ�ȭ ���� �� ������ ���
        if (PlayerExpManager.PlayerData != null && LevelLabel != null)
            LevelLabel.text = PlayerExpManager.PlayerData.playerLevel.ToString();

        if (hpLabel != null) hpLabel.text = Ark_stat.health.ToString();
        if (strLabel != null) strLabel.text = Ark_stat.strength.ToString();
        if (dexLabel != null) dexLabel.text = Ark_stat.dexterity.ToString();
        if (intLabel != null) intLabel.text = Ark_stat.intelligence.ToString();
        if (remainingPointLabel != null) remainingPointLabel.text = Ark_stat.remainingStatPoints.ToString();
    }

    void Update()
    {
        // i Ű�� ��� ? ���� ������ ����
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (IsOpen) Hide();
            else Show();
        }

        // �׽�Ʈ�� ����
        if (Input.GetKeyDown(KeyCode.R))
        {
            Ark_stat.ResetStats();
            SaveManager.Instance.SaveNow();
            RefreshUI();
            Debug.Log("�׽�Ʈ : ������ �ʱ�ȭ�Ǿ����ϴ�.");
        }

        // ���� ��ȭ ���� �� UI ����
        if (PlayerExpManager.PlayerData != null)
        {
            int currentLevel = PlayerExpManager.PlayerData.playerLevel;
            if (currentLevel != prevLevel)
            {
                prevLevel = currentLevel;
                RefreshUI();
            }
        }
    }
}
