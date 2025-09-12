// ���� ����� ���� �������� ���� (cameraOffset ���ŵ�)
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    //[SerializeField] private float pixelsPerUnit = 512f;

    public Transform steampunktarget;    // �� ������
    public Transform magictarget;        // �� ������
    private Vector3 target;     // �� ������
    [SerializeField] private float CamaraYmaius = 0f;

    public float smoothSpeed = 0.08f;

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private Vector3 velocity = Vector3.zero;
    private float camHalfWidth;
    private float camHalfHeight;

    private bool Steampunk = false;
    private bool Magic = false;
    private bool Select = false;

    [SerializeField] private ChooseOne chooseone;

    void Start()
    {

        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;
    }
    void FixedUpdate()
    {
        if (!Select) return;

        // Ÿ�� ��ġ + ������ ���
        if (Steampunk) target = steampunktarget.position;
        else if (Magic) target = magictarget.position;

        Vector3 offset = new Vector3(0f, CamaraYmaius, 0f);
        Vector3 targetWithOffset = target + offset;
        float clampedX = Mathf.Clamp(targetWithOffset.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(targetWithOffset.y, minBounds.y, maxBounds.y);
        //Vector3 targetPos = target;  // �� ������
        //float clampedX = Mathf.Clamp(targetPos.x, minBounds.x, maxBounds.x);
        //float clampedY = Mathf.Clamp(targetPos.y, minBounds.y, maxBounds.y);
        Vector3 desiredPos = new Vector3(clampedX, clampedY, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);
    }

    public void SetRoomBounds(BoxCollider2D roomBounds)
    {
        Bounds bounds = roomBounds.bounds;
        minBounds = new Vector2(bounds.min.x + camHalfWidth, bounds.min.y + camHalfHeight);
        maxBounds = new Vector2(bounds.max.x - camHalfWidth, bounds.max.y - camHalfHeight);

        // ��ġ ���� (ī�޶� ���� + �з����� sync)
        Vector3 targetPos = target;
        float clampedX = Mathf.Clamp(targetPos.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(targetPos.y, minBounds.y, maxBounds.y);
        Vector3 snappedPos = new Vector3(clampedX, clampedY, transform.position.z);
        transform.position = snappedPos;
        velocity = Vector3.zero;

        // �з����� ���̾�� �ʱ�ȭ
        /*foreach (var p in FindObjectsOfType<ParallaxLayer>())
        {
            p.ResetParallax(snappedPos);
        }*/
    }

    public void SnapToTarget()
    {
        Vector3 targetPos = target;
        float clampedX = Mathf.Clamp(targetPos.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(targetPos.y, minBounds.y, maxBounds.y);
        Vector3 snappedPos = new Vector3(clampedX, clampedY, transform.position.z);

        transform.position = snappedPos;
        velocity = Vector3.zero;

        // �з����� �ʱ�ȭ
        /*foreach (var parallax in FindObjectsOfType<ParallaxLayer>())
        {
            parallax.ForceSyncWithCamera(snappedPos);
        }*/
    }

    public void SetSteamPunktype()
    {
        Steampunk = true;
        target = steampunktarget.position;
        Select = true;
    }

    public void SetMagictype()
    {
        Magic = true;
        target = magictarget.position;
        Select = true;
    }
}
