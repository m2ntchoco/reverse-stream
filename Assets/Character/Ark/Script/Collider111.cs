using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Collider111 : MonoBehaviour
{
    [Tooltip("������ �ݶ��̴��� ���� �±�, ����θ� ��� �ݶ��̴� ����")]
    public string targetTag;

    // ���� �� �ִ� �ݶ��̴� ���
    private readonly HashSet<Collider2D> _current = new HashSet<Collider2D>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag)) return;
        if (_current.Add(other))
            Debug.Log($"[Enter]  ��  {other.name}");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag)) return;
        // Stay �̺�Ʈ�� �ʹ� ���� ������ �αװ� ������ ���Ѵٸ� �ּ� ó���ϼ���.
        Debug.Log($"[Stay]   ��  {other.name}");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag)) return;
        if (_current.Remove(other))
            Debug.Log($"[Exit]   ��  {other.name}");
    }

    private void Update()
    {
        if (_current.Count > 0)
        {
            var names = _current.Select(c => c.name);
            Debug.Log($"[Current] ��  {string.Join(", ", names)}");
        }
        else
        {
            Debug.Log("[Current] ��  (����)");
        }
    }
}
