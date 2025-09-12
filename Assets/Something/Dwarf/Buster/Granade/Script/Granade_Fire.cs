using UnityEngine;
using System.Collections;

public class DwarfBusterBullet : MonoBehaviour
{
    public GameObject bulletPrefab;          // �Ѿ� ������
    public Transform firePoint;              // �߻� ��ġ (������Ʈ ����)
    [SerializeField] private float bulletspeed = 0f;

    public void FireBullet() //enemyAI�� movetoplayer �޼ҵ带 ����ؼ� ����ź�� �ǰ� �����.
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            Vector2 direction = transform.localScale.x < 0 ? Vector2.right : Vector2.left;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1f; // �߷� ���� �ް� ���� (�ʿ� �� ����)

                // ������ ����: x�� �ӵ��� �ٶ󺸴� ����, y���� ���� �߻�
                Vector2 force = new Vector2(direction.x * bulletspeed, bulletspeed * 0.5f);
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }


}

