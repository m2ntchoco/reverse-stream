using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "SceneSystem/SceneData", order = 0)]
public class SceneData : ScriptableObject
{
    public string sceneName;        // Build Settings�� ��ϵ� �� �̸�
    public string displayName;      // UI � ǥ���� �̸�
    public int buildIndex;          // ���� �ε��� (���� ����)
    public Sprite previewImage;     // �� ����� (���� ����)
    public bool isRandomCandidate;  // ���� �̵� �ĺ����� ����
}