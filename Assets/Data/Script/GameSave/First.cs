using UnityEngine;

public class First : MonoBehaviour
{
    void Awake()
    {
        SaveSystemManager.ResumeOrInitAtStartup();// �� ���۽� �߰� ���� �����͸� �ҷ�����  �̺κ� ����
        PlayerExpManager.InitPlayerData();
        //Debug.Log(" �ʱ�ȭ �Ϸ�");
    }
}
