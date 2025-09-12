using UnityEngine;

public class Ground : MonoBehaviour
{
    [Header("���� �˻� (����)")]
    [SerializeField] private LayerMask groundMask;           // �Ϲ� ���� ����ũ
    [SerializeField] private string floatGroundLayerName = "FloatGround"; // �� �̸����� ����
    [SerializeField] public float rayDistance = 1.3f;
    [SerializeField] public float rayOriginOffsetY = 0f;
    [SerializeField] public float rayOriginOffsetX = 0f;

    private int floatGroundLayerIndex;   // �� �ε��� ĳ��
    private int combinedMask;            // �� groundMask + FloatGround

    private int facingDir;
    public float spacing = 0.2f;
    public bool isGrounded;
    public bool onGround;
    private bool prevGrounded = false;
    public bool isGroundedNow = false;
    public bool floatGround = false;

    private PlayerAnimationSync sync;
    private Player_move Player_move;

    private void Awake()
    {
        Player_move = GetComponentInParent<Player_move>();
        sync = GetComponentInParent<PlayerAnimationSync>();

        // FloatGround ���̾� �ε��� Ȯ��
        floatGroundLayerIndex = LayerMask.NameToLayer(floatGroundLayerName);
        if (floatGroundLayerIndex < 0)
            Debug.LogError($"���̾� '{floatGroundLayerName}'�� ã�� �� �����ϴ�. Project Settings > Tags and Layers���� Ȯ���ϼ���.");

        // ���̿� ����ũ: �Ϲ� groundMask + FloatGround�� ��ħ
        combinedMask = groundMask.value | (1 << floatGroundLayerIndex);
    }

    private void FixedUpdate()
    {
        if (Player_move.isjump) return;

        facingDir = transform.localScale.x > 0 ? 1 : -1;

        const int rayCount = 3;
        float halfSpacing = spacing * (rayCount - 1) / 2f;

        // ������ ���ۿ� ����
        isGroundedNow = false;
        floatGround = false;

        for (int i = 0; i < rayCount; i++)
        {
            float offsetX = ((i * spacing) - halfSpacing) * facingDir;
            Vector2 rayOrigin = (Vector2)transform.position
                              + new Vector2(rayOriginOffsetX * facingDir, -rayOriginOffsetY)
                              + new Vector2(offsetX, 0f);

            // �� FloatGround�� ���߱� ���� combinedMask ���
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, combinedMask);

            if (!hit.collider)
            {
                Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, Color.green);
                continue;
            }

            isGroundedNow = true;

            // �� ���� FloatGround�� ���� true
            if (hit.collider.gameObject.layer == floatGroundLayerIndex)
                floatGround = true;

            Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, floatGround ? Color.cyan : Color.red);
        }

        isGrounded = isGroundedNow;
        sync.IsGround(isGroundedNow);

        if (isGroundedNow)
            Player_move.ResetJumpCount();

        prevGrounded = isGroundedNow;
    }
}
