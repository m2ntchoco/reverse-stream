using UnityEngine;

public class Spit : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("������ ����")]
    public GameObject groundEffect;
    public LayerMask groundLayer;
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
    
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // other�� groundLayer�� ���ϴ��� Ȯ��
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            Debug.Log("����");
            SpawnEffectAndDestroy();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            //Debug.Log("����");
            SpawnEffectAndDestroy();
        }
    }
    private void SpawnEffectAndDestroy()
    {
        // ����Ʈ ���� (��ġ��ȸ���� �ʿ信 �°� ����)
        Instantiate(
            groundEffect,
            transform.position,    // �浹�� ������Ʈ ��ġ
            Quaternion.identity
        );
        // ���� ������Ʈ �ı�
        Destroy(gameObject);
    }
}
