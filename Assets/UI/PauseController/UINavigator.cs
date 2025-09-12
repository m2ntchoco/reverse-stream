using System.Collections.Generic;
using UnityEngine;

public class UINavigator : MonoBehaviour
{
    public static UINavigator Instance { get; private set; }

    // ���� ������ ����ϴ� ���� (�� ���� ���� �ֱٿ� �� �г�)
    private readonly List<IBackPanel> openStack = new();
    private IBackPanel pauseMenu; // �Ͻ����� �г�(Ư��)

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // �г� ���(�Ͻ����� ���θ� �˷��ָ� ��)
    public void Register(IBackPanel panel, bool isPauseMenu = false)
    {
        if (isPauseMenu) pauseMenu = panel;
    }

    // �г��� ���� ��/���� �� �ڵ� ȣ�� (BackPanelBase���� ȣ��)
    public void NotifyOpened(IBackPanel panel)
    {
        // �ߺ� ���� �� �� ��(Top)��
        openStack.Remove(panel);
        openStack.Add(panel);
    }
    public void NotifyClosed(IBackPanel panel)
    {
        openStack.Remove(panel);
    }

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        var kb = UnityEngine.InputSystem.Keyboard.current;
        if (kb != null && kb.escapeKey.wasPressedThisFrame) OnEsc();
#else
        if (Input.GetKeyDown(KeyCode.Escape)) OnEsc();
#endif
    }

    void OnEsc()
    {
        // 1) �����ִ� �г��� ������ "���� �ֱٿ� �� ��" �ϳ��� �ݱ�
        //    (�Ͻ����� �г��� ���� �ְ� �� ���� �ٸ� �г�(����/�κ��丮)�� ������ �װ� ���� ����)
        if (openStack.Count > 0)
        {
            var top = openStack[openStack.Count - 1];

            // ���� top�� pauseMenu�� �� �Ͻ����� �ݰ� �����簳
            if (pauseMenu != null && top == pauseMenu)
            {
                pauseMenu.Hide();
                PauseManager.Resume();
                return;
            }

            // �Ϲ� �г��̸� �׳� �װŸ� �ݰ� �� (������ ���)
            top.Hide();
            return;
        }

        // 2) �ƹ� �͵� �� ���������� �� �Ͻ����� ����/���
        if (pauseMenu != null)
        {
            // �������� ������ Show + Pause
            pauseMenu.Show();
            PauseManager.Pause();
        }
    }
}
