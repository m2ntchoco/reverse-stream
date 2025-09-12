using UnityEngine;
using System.Collections;

public class SlashEffect : MonoBehaviour
{
    public float duration = 0.3f;      // ����Ʈ ��� �ð�
    public int damage = 20;            // ������ ��

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var hp = other.GetComponent<MonsterHP>();
        if (hp != null)
        {
            hp.Getdamage(damage);
        }
    }

}
