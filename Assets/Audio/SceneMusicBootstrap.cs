// Assets/Audio/SceneMusicBootstrap.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicBootstrap : MonoBehaviour
{
    public SceneMusicMap map;

    void Start()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        if (MusicPlayer.Instance)
        {
            MusicPlayer.Instance.PlayForScene(map, sceneName);
        }
        else
        {
            Debug.LogWarning("[SceneMusicBootstrap] MusicPlayer�� ���� �����ϴ�. �������� ��ŸƮ���� �ΰ� DDOL�� �����ϼ���.");
        }
    }
}
