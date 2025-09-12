using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
/// �� �ε� �� �ڵ����� \"SpawnPoint\" ��ġ�� ã�� �÷��̾ �� ��ġ�� �̵���Ŵ.
public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject player; // Player�� �ܺο��� �Ҵ�ǰų� �ڵ� Ž��

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedSpawn());
    }

    private IEnumerator DelayedSpawn()
    {
        yield return null; // �� ������ ��ٸ��� �� ���� ������Ʈ���� ��� �ʱ�ȭ �Ϸ��

        SpawnPoint spawn = FindFirstObjectByType<SpawnPoint>(); // �̸� ��� Ÿ������ ã��
        GameObject player = GameObject.FindWithTag("Player");

        Debug.Log("[SpawnTest] SpawnPoint ã��: " + (spawn != null));
        Debug.Log("[SpawnTest] Player ã��: " + (player != null));

        if (spawn != null && player != null)
        {
            player.transform.position = spawn.transform.position;
            ChildPositionRestorer restorer = player.GetComponent<ChildPositionRestorer>();
            restorer?.RestoreLocalPositions();
            Debug.Log($"[SpawnTest] �÷��̾� �̵� �Ϸ� �� {spawn.transform.position}");
        }
        else
        {
            Debug.LogWarning("[SpawnTest] ��ġ �̵� ����");
        }
    }
}