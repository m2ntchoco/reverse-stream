// SceneFlowLoader.cs
using UnityEngine;
using UnityEngine.SceneManagement;

// �� �帧�� ���� �ڵ����� ���� ���� �ε��ϴ� ������Ʈ
public class SceneFlowLoader : MonoBehaviour
{
    [SerializeField] private SceneFlowMap flowMap; // ����� �帧 ���̺�

    // ���� �� �������� ���� �� �ε�
    public void LoadNextScene()
    {
        string current = SceneManager.GetActiveScene().name;
        string next = flowMap != null ? flowMap.GetNextScene(current) : null;

        if (string.IsNullOrEmpty(next))
        {
            Debug.LogWarning($"�帧�� ���ǵ��� ���� ��: {current}");
            return;
        }

        // DDOLSceneLoader�� ��� ������ �ε�â ����(�ּ� 2��, ���̵�, ��ó�� �� ó����)
        if (DDOLSceneLoader.I != null)
        {
            DDOLSceneLoader.I.LoadLevel(next);
            return; // ���� ���� �ε�� �������� ����
        }

        // �δ��� ������ ���� ��� ����(�ε�â ���� ��� ��ȯ)
        SceneManager.LoadScene(next);
    }
}
