using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // �ߺ� ����
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // UIManager ��ü ����
    }
}