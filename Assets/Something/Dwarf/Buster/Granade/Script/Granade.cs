using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Grenade : MonoBehaviour
{
    [Header("Explosion Settings")]
    public GameObject explosionEffectPrefab;  // ���� ����Ʈ ������
    public float fuseTime = 3f;        // �߻� �� �ڵ� ���߱��� �ð� (�ν����Ϳ��� ���� ����)

    void Start()
    {
        // ���� ��� Ÿ�̸� ����
        GameObject player = GameObject.FindWithTag("Player");
        Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
        if (Mathf.Abs(direction.x) > 0.01f)
            FaceDirection(direction.x > 0 ? 1 : -1);
        StartCoroutine(ExplosionCountdown());
    }

    IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(fuseTime);
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("�ε���");

        // �÷��̾�� �ε����� ��� ����
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� �ε���");
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        // �ٴ�(Ground)�� ���� �ε����� ����� ����Ӵϴ�.
    }
    public void FaceDirection(int dir) // �׸� ��ȯ �޼ҵ�
    {
        if (dir == 0) return;  // ���� ���¿����� ���� ����

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -dir;  // dir�� -1�̸� ����, 1�̸� ������
        transform.localScale = scale;
    }
}
