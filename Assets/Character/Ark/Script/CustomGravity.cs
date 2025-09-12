using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    
    public float gravity = -30f; // �߷� ���ӵ�
    public float maxFallSpeed = -20f; // ���� �ӵ� ����

    public Rigidbody2D rb;
    private Ground ground;
    private Player_move move;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        move = GetComponent<Player_move>();
    }

    private void FixedUpdate()
    {
        if (move.dash.IsDashing)
            return;
        
        // �����ӵ� ���� (�߷� ����)
        if (!ground.isGrounded) // �ٴڿ� ���� ���� �������� ����
        {
            float newYVelocity = rb.linearVelocity.y + gravity * Time.fixedDeltaTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(newYVelocity, maxFallSpeed));
        }
    }
        
}