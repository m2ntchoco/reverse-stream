using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoxColliderGizmo : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public Boss1_Phasechange phasechange;
    public bool DdalGGak = true;

    public void Awake()
    {
        phasechange = GetComponent<Boss1_Phasechange>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (DdalGGak)
        {
            boxCollider.isTrigger = true;
        }
        else
        {
            boxCollider.isTrigger = false;
        }

    }
    private void OnDrawGizmos()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            // Gizmo ���� ���� (���ϴ� ������ ���� ����)
            Gizmos.color = new Color(0, 1, 0, 0.3f); // ������ �ʷϻ�

            // �ڽ� �ݶ��̴� ��ġ�� ũ�� ��� (���� -> ���� ��ǥ ��ȯ)
            Vector3 colliderCenter = transform.position + (Vector3)boxCollider.offset;
            Vector3 colliderSize = new Vector3(boxCollider.size.x * transform.lossyScale.x, boxCollider.size.y * transform.lossyScale.y, 0f);

            // Gizmo�� �ڽ� �ݶ��̴� ũ�⸸ŭ �ڽ� �׸���
            Gizmos.DrawCube(colliderCenter, colliderSize);
        }
    }
}
