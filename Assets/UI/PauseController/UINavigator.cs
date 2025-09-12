using System.Collections.Generic;
using UnityEngine;

public class UINavigator : MonoBehaviour
{
    public static UINavigator Instance { get; private set; }

    // 열린 순서를 기록하는 스택 (맨 위가 가장 최근에 연 패널)
    private readonly List<IBackPanel> openStack = new();
    private IBackPanel pauseMenu; // 일시정지 패널(특수)

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // 패널 등록(일시정지 여부만 알려주면 됨)
    public void Register(IBackPanel panel, bool isPauseMenu = false)
    {
        if (isPauseMenu) pauseMenu = panel;
    }

    // 패널이 열릴 때/닫힐 때 자동 호출 (BackPanelBase에서 호출)
    public void NotifyOpened(IBackPanel panel)
    {
        // 중복 제거 후 맨 뒤(Top)로
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
        // 1) 열려있는 패널이 있으면 "가장 최근에 연 것" 하나만 닫기
        //    (일시정지 패널이 열려 있고 그 위에 다른 패널(설정/인벤토리)이 있으면 그걸 먼저 닫음)
        if (openStack.Count > 0)
        {
            var top = openStack[openStack.Count - 1];

            // 만약 top이 pauseMenu면 → 일시정지 닫고 게임재개
            if (pauseMenu != null && top == pauseMenu)
            {
                pauseMenu.Hide();
                PauseManager.Resume();
                return;
            }

            // 일반 패널이면 그냥 그거만 닫고 끝 (게임은 계속)
            top.Hide();
            return;
        }

        // 2) 아무 것도 안 열려있으면 → 일시정지 열기/토글
        if (pauseMenu != null)
        {
            // 열려있지 않으니 Show + Pause
            pauseMenu.Show();
            PauseManager.Pause();
        }
    }
}
