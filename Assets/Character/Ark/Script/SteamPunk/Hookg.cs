using UnityEngine;

public class Hookg : MonoBehaviour
{
    private Hook Grappling;
    // public DistanceJoint2D joint2D;  // ���� ��� �� �ϹǷ� ���� ����

    private void Start()
    {
        Grappling = GameObject.Find("Ark/SteamPunk").GetComponent<Hook>();
        // joint2D = GetComponent<DistanceJoint2D>();
        // joint2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RING"))
        {
            // ������ Ring ��ġ�� �ɷ����� Hook���� �˸�
            Grappling.isAttachReady = true;
            Grappling.isHookActive = false;
            Grappling.isLineMax = false;

            // joint2D Ȱ��ȭ�� �� �̻� ������� �ʽ��ϴ�.
        }
    }
}
