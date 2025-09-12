using System.Collections.Generic;
using UnityEngine;

public class ChainSpawner : MonoBehaviour
{
    [Header("��ũ ����")]
    public GameObject chainLinkPrefab;    // ü�� ��ũ ������
    public float linkSpacing = 0.2f;      // ��ũ �� ����
    public int maxLinks = 20;

    [Header("���� ���")]
    public Transform startPoint; // �÷��̾�(�Ǵ� ��)
    public Transform endPoint;   // ���� ��

    private List<GameObject> linkPool = new List<GameObject>();

    private void Awake()
    {
        // Ǯ���� ���� �ִ� maxLinks��ŭ �̸� ����
        for (int i = 0; i < maxLinks; i++)
        {
            GameObject link = Instantiate(chainLinkPrefab, Vector3.zero, Quaternion.identity);
            link.SetActive(false);
            linkPool.Add(link);
        }
    }

    private void Update()
    {
        // ������ ��Ȱ��ȭ�� ���¶�� Ǯ�� �ִ� ��� ��ũ�� �� �ΰ� ����
        if (!endPoint.gameObject.activeInHierarchy)
        {
            for (int i = 0; i < linkPool.Count; i++)
                if (linkPool[i].activeSelf)
                    linkPool[i].SetActive(false);
            return;
        }

        // 1) startPoint�� endPoint ������ ���� (Vector3)
        Vector3 delta = endPoint.position - startPoint.position;
        float chainLength = delta.magnitude;

        // 2) neededLinks: �ʿ��� ��ũ ����
        int neededLinks = Mathf.FloorToInt(chainLength / linkSpacing);
        neededLinks = Mathf.Clamp(neededLinks, 0, maxLinks);

        // 3) ���⺤�� dir (Vector3, ���� 1)
        Vector3 dir = delta.normalized;

        // 4) �� ��ũ�� ��ġ
        for (int i = 0; i < neededLinks; i++)
        {
            GameObject link = linkPool[i];
            if (!link.activeSelf) link.SetActive(true);

            // ** �� �� ���� ���� ������ �ڵ忴�� �κ� **
            //Vector3 pos = (Vector2)startPoint.position + dir * (i * linkSpacing);

            // �� �Ʒ�ó�� �����մϴ�.
            Vector3 pos = startPoint.position + dir * (i * linkSpacing);

            link.transform.position = pos;

            // ��ũ �ϳ��ϳ��� dir ������ �ٶ󺸵��� ȸ��
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            link.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        // 5) ���� ��ũ Ǯ�� ��� ��Ȱ��ȭ
        for (int i = neededLinks; i < linkPool.Count; i++)
        {
            if (linkPool[i].activeSelf)
                linkPool[i].SetActive(false);
        }
    }
}
