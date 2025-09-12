using UnityEngine;

public class ObjectDontDestroy : MonoBehaviour
{
    public static ObjectDontDestroy Instance;
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
