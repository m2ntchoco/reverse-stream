using UnityEngine;

/// <summary>
/// ���� ��ü ������ �����ϴ� �̱���
/// �� ��ȯ �ÿ��� �ı����� �ʰ� CurrentMode�� �����մϴ�.
/// </summary>
[DefaultExecutionOrder(20)]  // Awake�� �ٸ� ��ũ��Ʈ���� ���� ȣ��ǵ��� ����
public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    /// <summary>
    /// �÷��̾ ������ ���
    /// </summary>
    public GameMode CurrentMode { get; set; } = GameMode.Steampunk;

    private void Awake()
    {
        // �̱��� �ν��Ͻ� �ʱ�ȭ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
