using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DashCooldownUI : MonoBehaviour
{
    public float cooldownTime = 5f;  // ��Ÿ�� (�� ����)

    private float cooldownTimer = 0f;
    private bool isCoolingDown = false;
    [SerializeField] private Image cooldownMaskImage;
    [SerializeField] private TextMeshProUGUI cooldownText;
    void Start()
    {
        if (cooldownMaskImage == null)
            Debug.LogError("Cooldown mask image is not assigned.");
        cooldownMaskImage.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(false);  // ���� �� �Ⱥ��̰�
    }
    void Update()
    {
        if (isCoolingDown)
        {
            cooldownMaskImage.gameObject.SetActive(true);
            cooldownTimer -= Time.deltaTime;
            float ratio = Mathf.Clamp01(cooldownTimer / cooldownTime);
            cooldownMaskImage.fillAmount = ratio;

            // �ؽ�Ʈ ����
            cooldownText.text = Mathf.CeilToInt(cooldownTimer).ToString();
            cooldownText.gameObject.SetActive(true);
            cooldownText.color = Color.white; // Ȥ�� �����ϱ�� ������ ����

            if (cooldownTimer <= 0f)
            {
                isCoolingDown = false;
                cooldownMaskImage.fillAmount = 0f;
                cooldownText.gameObject.SetActive(false);  // �ؽ�Ʈ �Ⱥ��̰�
            }
        }

    }

    public void StartCooldown()
    { 
        isCoolingDown = true;
        cooldownTimer = cooldownTime;
        cooldownMaskImage.fillAmount = 1f;
       
    }
}
