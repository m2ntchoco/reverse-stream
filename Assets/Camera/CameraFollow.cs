using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform steampunktarget;                   // ����ٴ� ��� (���� �÷��̾�)
    public Transform magictarget;                   // ����ٴ� ��� (���� �÷��̾�)
    private Vector3 target;

    public float smoothSpeed = 0.125f;         // ī�޶� �̵� �ε巯�� ����
    public float airYOffset = 1f;              // ���߿� ���� �� ���� �̵��� ������
    public float groundYOffset = -1f;          // ���� ���� �� �Ʒ��� �̵��� ������
    private float camHalfWidth;                // ī�޶� �� �ʺ� (ȭ�� ���� ����)
    private float camHalfHeight;               // ī�޶� �� ���� (ȭ�� ���� ����)
    private Vector2 minBounds;                 // ī�޶� �̵� �ּ� ��ǥ
    private Vector2 maxBounds;                 // ī�޶� �̵� �ִ� ��ǥ
    private float currentYOffset;              // ���� Y�� ������

    private bool Steampunk = false;
    private bool Magic = false;
    private bool Select = false;
        
    [SerializeField] private float CamaraYmaius = 0f;
    [HideInInspector] public bool isGrounded = true; // �÷��̾��� ���� ��Ҵ��� ���� (�ܺο��� ����)
    [SerializeField] private ChooseOne chooseone;

    void Start()
    {
        // ī�޶� ũ�� ��� (Orthographic ����)
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * Screen.width / (float)Screen.height;
    }

    void LateUpdate()
    {
        //if (target == null) return;

        if(!Select) return;

        // ���� ��Ҵ��� ���ο� ���� Y �������� �ε巴�� ����
        float desiredYOffset = isGrounded ? groundYOffset : airYOffset;
        currentYOffset = Mathf.Lerp(currentYOffset, desiredYOffset, Time.deltaTime * 3f);

        // Ÿ�� ��ġ + ������ ���
        if(Steampunk) target = steampunktarget.position;
        else if (Magic) target = magictarget.position;

        Vector3 targetPos = target + new Vector3(0, currentYOffset - CamaraYmaius, 0);

        // ���� ���� ������ ��ġ ���� (Clamp)
        float clampedX = Mathf.Clamp(targetPos.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(targetPos.y, minBounds.y, maxBounds.y);

        // ���� ��ġ�� �ε巴�� �̵�
        Vector3 finalPos = new Vector3(clampedX, clampedY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, finalPos, smoothSpeed);
    }

    // �� ����(BoxCollider2D ����)�� �޾� ī�޶� �̵� ���� ����
    public void SetBounds(BoxCollider2D roomBounds)
    {
        Bounds bounds = roomBounds.bounds;

        // ī�޶� �� �ٱ����� ������ �ʵ��� ������ �ּ�/�ִ� ��ǥ ���
        minBounds = new Vector2(bounds.min.x + camHalfWidth, bounds.min.y + camHalfHeight);
        maxBounds = new Vector2(bounds.max.x - camHalfWidth, bounds.max.y - camHalfHeight);
    }

    public void SteamPunktype()
    {
        Steampunk = true;
        target = steampunktarget.position;
        Select = true;
    }

    public void Magictype()
    {
        Magic = true;
        target = magictarget.position;
        Select = true;
    }
}
