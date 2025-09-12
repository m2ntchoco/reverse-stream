using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// IHealth �������̽��� ����� ���� ü�°� �ִ� ü���� �����ϴ� ��� ��ü�� ���ε��մϴ�.
/// </summary>
public class EnemyHealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image enemyHealthBar;
    [SerializeField] private TextMeshProUGUI enemyHealthBarText;

    private IHealth healthSource;
    private Transform target;

    [Header("Offset and Target")]
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);

    private RectTransform rectTransform;
    private Canvas canvas;
    private float maxHP;

    private void Awake()
    {
        // RectTransform�� Canvas�� �̸� ĳ��
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    /// <summary>
    /// IHealth ����ü, Ÿ�� Transform, �������� �����ϰ� �ʱ� UI�� �����մϴ�.
    /// </summary>
    public void SetTarget(IHealth hpSource, Transform targetTransform, Vector3 offset)
    {
        healthSource = hpSource;
        target = targetTransform;
        this.offset = offset;
        maxHP = hpSource.maxHP;

        // �ʱ� UI�� ��ġ ����
        UpdateHealthUI();
        UpdatePositionAndRotation();
    }

    private void Update()
    {
        if (healthSource == null)
            return;

        float current = healthSource.currentHP;
        if (current <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        UpdateHealthUI();
    }

    private void LateUpdate()
    {
        // Null üũ: SetTarget ���� ȣ����� �ʵ��� ���
        if (healthSource == null || target == null || canvas == null)
            return;

        if (canvas.renderMode == RenderMode.WorldSpace)
            UpdatePositionAndRotation();
    }

    private void UpdatePositionAndRotation()
    {
        rectTransform.position = target.position + offset;

        var cam = Camera.main;
        if (cam == null)
            return;

        Vector3 forward = cam.transform.forward;
        forward.y = 0f;
        if (forward.sqrMagnitude > 0.001f)
            rectTransform.rotation = Quaternion.LookRotation(forward);
    }

    /// <summary>
    /// ü�¹ٿ� �ؽ�Ʈ�� �ֽ� ������ �����մϴ�.
    /// </summary>
    public void UpdateHealthUI()
    {
        if (healthSource == null || enemyHealthBar == null || enemyHealthBarText == null)
            return;

        float current = healthSource.currentHP;
        enemyHealthBar.fillAmount = current / maxHP;
        enemyHealthBarText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(maxHP)}";
    }
}
