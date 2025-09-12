using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundEffect : MonoBehaviour
{
    public BoxCollider2D spitdamagearea;
    public LayerMask playerLayer;

    public HashSet<PlayerHealth> recentlyDamaged = new HashSet<PlayerHealth>();
    public void Awake()
    {
            StartCoroutine(SpitDamage());
            
    }

    public IEnumerator SpitDamage()
    {
        float timer = 0f;
        while (timer < 5f)
        {
            Vector2 center = spitdamagearea.bounds.center;
            Vector2 size = new Vector2(
            spitdamagearea.size.x * spitdamagearea.transform.lossyScale.x,
               spitdamagearea.size.y * spitdamagearea.transform.lossyScale.y
            );
            Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, playerLayer);
            Debug.Log($"������ ���� ��� ��: {hits.Length}");
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player") && hit.TryGetComponent(out PlayerHealth player))
                {
                    if (!recentlyDamaged.Contains(player))
                    {
                        player.TakeDamage(10,transform , 0.5f , 0.5f);
                        Debug.Log("���� ����");
                        recentlyDamaged.Add(player);
                    }
                }
            }

            yield return new WaitForSeconds(1f);
            recentlyDamaged.Clear(); // ���� ���ݿ� �ٽ� ������ ����
            timer += 1f;
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
    }
}
