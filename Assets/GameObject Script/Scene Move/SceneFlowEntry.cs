// SceneFlowEntry.cs
using UnityEngine;

// �� ���� �帧�� �����ϴ� ������ Ŭ����
[System.Serializable]
public class SceneFlowEntry
{
    public string currentSceneName; // ���� �� �̸�

    public bool useRandomNext = false; // ���� �б� ����
    public string nextSceneName;       // ���� ���� �� �̸� (useRandomNext == false)
    public string[] randomNextScenes;  // ���� �б� ��� �� ����Ʈ (useRandomNext == true)
}
