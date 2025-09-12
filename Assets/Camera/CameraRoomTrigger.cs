using UnityEngine;

public class CameraRoomTrigger : MonoBehaviour
{
    // �÷��̾ �� Ʈ���ſ� �����ϸ� �ش� ���� ī�޶� ���� ������ ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������Ʈ�� "Player" �±׸� ������ �ִٸ�
        if (other.CompareTag("Player"))
        {
            // ���� ī�޶󿡼� CameraFollow ������Ʈ ��������
            CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
            // �� ������Ʈ�� �پ��ִ� BoxCollider2D �������� (���� ����)
            BoxCollider2D col = GetComponent<BoxCollider2D>();

            // �� �� ��ȿ�ϸ� ī�޶� ���� �� ������ ����
            if (cam && col)
            {
                cam.SetBounds(col);
            }
        }
    }
}
