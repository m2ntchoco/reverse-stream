using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OverHeatUI : MonoBehaviour 
{

    [SerializeField] private Image OverHeatcooldownMaskImage;
    [SerializeField] private TextMeshProUGUI cooldownText;
    public float OverHeatCooldown = 7;
    private float cooldownTimer = 0f;
    public bool isCoolingDown = false;

    void Start()
    {

        OverHeatcooldownMaskImage.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isCoolingDown)
        {
            OverHeatcooldownMaskImage.gameObject.SetActive(true);
            cooldownTimer -= Time.deltaTime;
            float ratio = Mathf.Clamp01(cooldownTimer / OverHeatCooldown);
            OverHeatcooldownMaskImage.fillAmount = ratio;

            // �ؽ�Ʈ ����
            cooldownText.text = Mathf.CeilToInt(cooldownTimer).ToString();
            cooldownText.gameObject.SetActive(true);
            cooldownText.color = Color.white; // Ȥ�� �����ϱ�� ������ ����

            if (cooldownTimer <= 0f)
            {
                isCoolingDown = false;
                OverHeatcooldownMaskImage.fillAmount = 0f;
                cooldownText.gameObject.SetActive(false);  // �ؽ�Ʈ �Ⱥ��̰�
            }
        }
    }

    public void StartOverHeatCooldown()
    {
        isCoolingDown = true;
        cooldownTimer = OverHeatCooldown;
        OverHeatcooldownMaskImage.fillAmount = 1f;

    }
}
