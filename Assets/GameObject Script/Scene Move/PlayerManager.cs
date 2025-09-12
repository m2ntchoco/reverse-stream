using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("�÷��̾� Ʈ��������")]
    [SerializeField] private Transform magicPlayer;
    [SerializeField] private Transform steamPunkPlayer;

    // � Ÿ���� ���� �ִ��� ChooseOne ��ũ��Ʈ ��� ���⼭ �Ǵ��ص� �����ϴ�
    public enum PlayerType { Magic, SteamPunk }
    public PlayerType CurrentType { get; private set; }

    public Transform ActivePlayerTransform
        => CurrentType == PlayerType.SteamPunk
            ? steamPunkPlayer
            : magicPlayer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // �ܺο��� Ÿ�� ������ ��
    public void SetPlayerType(PlayerType type)
    {
        CurrentType = type;
        // �ʿ��ϸ� �̺�Ʈ�� �˸� ���� ����
    }
}