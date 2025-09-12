using UnityEngine;

public static class PauseManager
{
    private static int _pauseCount = 0; // ��ø ī��Ʈ
    public static bool IsPaused => _pauseCount > 0;

    public static System.Action<bool> OnToggleGameplayInput; // true=�Է�Ȱ��, false=��Ȱ��

    public static void Pause()
    {
        _pauseCount++;
        if (_pauseCount == 1)
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            OnToggleGameplayInput?.Invoke(false);
        }
    }

    public static void Resume()
    {
        if (_pauseCount <= 0) { _pauseCount = 0; return; }
        _pauseCount--;
        if (_pauseCount == 0)
        {
            Time.timeScale = 1f;
            OnToggleGameplayInput?.Invoke(true);
        }
    }

    // �ʿ��ϸ� ���� ī��Ʈ ���¿�(�����)
    public static void ForceResumeAll()
    {
        _pauseCount = 0;
        Time.timeScale = 1f;
        OnToggleGameplayInput?.Invoke(true);
    }
}
