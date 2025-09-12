using UnityEngine;

public class CameraDontDestroy : MonoBehaviour
{
    public static CameraDontDestroy Instance { get; private set; }
    public Transform CameraTransform => transform;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // �ߺ� ����
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // �� �̵��ص� ����
    }
}
