// SceneFlowMap.cs
using UnityEngine;

// ��ü �� �帧�� �����ϴ� ScriptableObject
[CreateAssetMenu(fileName = "SceneFlowMap", menuName = "SceneSystem/SceneFlowMap")]
public class SceneFlowMap : ScriptableObject
{
    public SceneFlowEntry[] flowEntries; // �� �帧 ����Ʈ

    // ���� �� �̸��� ���� ���� �� �̸� ��ȯ (���� ����)
    public string GetNextScene(string current)
    {
        foreach (var entry in flowEntries)
        {
            if (entry.currentSceneName == current)
            {
                if (entry.useRandomNext && entry.randomNextScenes.Length > 0)
                {
                    int index = Random.Range(0, entry.randomNextScenes.Length);
                    return entry.randomNextScenes[index];
                }
                else
                {
                    return entry.nextSceneName;
                }
            }
        }
        return null; // ��Ī �� �� ���
    }
}