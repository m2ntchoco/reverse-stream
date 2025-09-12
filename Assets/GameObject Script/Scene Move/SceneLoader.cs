using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneTable sceneTable;

    public void LoadSceneByName(string name)
    {
        var data = sceneTable.GetSceneByName(name);
        if (data != null)
        {
            SceneManager.LoadScene(data.sceneName);
        }
        else
        {
            Debug.LogError($"�� �̸� '{name}'�� �ش��ϴ� �����Ͱ� �����ϴ�.");
        }
    }

    public void LoadSceneByIndex(int index)
    {
        if (index >= 0 && index < sceneTable.scenes.Length)
        {
            SceneManager.LoadScene(sceneTable.scenes[index].sceneName);
        }
        else
        {
            Debug.LogError("��ȿ���� ���� �� �ε���");
        }
    }

    public void LoadRandomScene()
    {
        var candidates = sceneTable.GetRandomCandidates();
        if (candidates == null || candidates.Length == 0)
        {
            Debug.LogWarning("���� ��� ���� �����ϴ�.");
            return;
        }

        int randomIndex = Random.Range(0, candidates.Length);
        SceneManager.LoadScene(candidates[randomIndex].sceneName);
    }
}
