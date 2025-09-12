using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GenericPanelAdapter : BackPanelBase
{
    [Header("Back(ESC) �켱���� (�������� ���� ����)")]
    [SerializeField] private int priority = 50;
    public override int BackPriority => priority;

    [Header("���� �� ���� ������ (����/�ҿ�â�� ���� false)")]
    [SerializeField] private bool pauseGameOnOpen = false;

    [Header("PauseMenu�� ����� �г�����")]
    [SerializeField] private bool registerAsPauseMenu = false;

    [Header("�ɼ�: ���� ��/���� �� �̺�Ʈ")]
    public UnityEvent onShown;
    public UnityEvent onHidden;

    protected override void Awake()
    {
        base.Awake();
        // ������ ����
        Hide();
        // �׺������ ���
        //UINavigator.Instance.Register(this, isPauseMenu: registerAsPauseMenu);
    }

    public override void Show()
    {
        base.Show();
        onShown?.Invoke();

        if (pauseGameOnOpen)
            PauseManager.Pause();     // �ʿ� UI�� ���� �� ���� ����
        // ��Ŀ���� �ʿ��ϸ� UXML ���� ù ��ư name���� �����ͼ� Focus()
        // var firstBtn = viewRoot.Q<Button>("FirstButtonName");
        // firstBtn?.Focus();
    }

    public override void Hide()
    {
        base.Hide();
        onHidden?.Invoke();

        if (pauseGameOnOpen && PauseManager.IsPaused)
            PauseManager.Resume();    // �ڽ� ������ ����ٸ� ���� �� ����
    }

    // �ܺο��� ���� ���� �� �ְ� ���� �޼���
    public void Open() => Show();
    public void Close() => Hide();
}

