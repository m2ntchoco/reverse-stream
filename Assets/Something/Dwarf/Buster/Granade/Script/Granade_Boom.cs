using UnityEngine;
using System.Collections;

public class Grenade_Boom : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius = 2f;   // ���� �ݰ�
    public float explosionForce = 10f;  // ���� ��
    public int Damage = 50;   // �ִ� �����
    private bool hasExploded = false;

    private PlayerHealth player;

    private void Awake()
    {
        player = GetComponent<PlayerHealth>();
    }

    // 1) �÷��̾�� �浹�ϸ� ����
    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded || !other.CompareTag("Player"))
            return;

        hasExploded = true;
        Debug.Log("����ź ����");

        // �ֺ� �÷��̾�Ը� ���� �ݹ߷°� ����� ����
        if (other.CompareTag("Player"))
        {  
            // ����� ���
            /*float dist = Vector2.Distance(transform.position, other.transform.position);
            float ratio = Mathf.Clamp01((explosionRadius - dist) / explosionRadius);
            int damage = Mathf.RoundToInt(ratio * maxDamage);*/

            // �÷��̾� ü�� ���� �� �˹� �÷���
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.isknockback = true;
                player.TakeDamage(Damage,transform , 3f, 5f);
            }
            // ���� �ݹ߷�
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = rb.position - (Vector2)transform.position;
                rb.AddForce(dir.normalized * explosionForce, ForceMode2D.Impulse);
            }
        }
    }
    void SelfDestory()
    {
        // �ڱ� �ڽ� ����
        Destroy(gameObject);
    }

    // �� �信�� ���� �ݰ� �ð�ȭ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    private IEnumerator waitAddforce()
    {
        yield return null;
    }
}
