using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HookUI : MonoBehaviour
{
    [SerializeField] private Image HookcooldownMaskImage;
    [SerializeField] private TextMeshProUGUI cooldownText;
    private static float HookCooldown = 0;
    private float cooldownTimer = 0f;
    private bool isCoolingDown = false;

    private Hook hook;

    private void Awake()
    {
        hook = GetComponent<Hook>();
    }

    void Start()
    {
        HookcooldownMaskImage.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(false);
    }


    void Update()
    {
        if (isCoolingDown)
        {
            HookcooldownMaskImage.gameObject.SetActive(true);
            cooldownTimer -= Time.deltaTime;
            float ratio = Mathf.Clamp01(cooldownTimer / HookCooldown);
            HookcooldownMaskImage.fillAmount = ratio;

            // �ؽ�Ʈ ����
            cooldownText.text = Mathf.CeilToInt(cooldownTimer).ToString();
            cooldownText.gameObject.SetActive(true);
            cooldownText.color = Color.white; // Ȥ�� �����ϱ�� ������ ����

            if (cooldownTimer <= 0f)
            {
                isCoolingDown = false;
                HookcooldownMaskImage.fillAmount = 0f;
                cooldownText.gameObject.SetActive(false);  // �ؽ�Ʈ �Ⱥ��̰�
            }
        }
    }

    public void StartHookCooldown()
    {
        isCoolingDown = true;
        HookCooldown = Hook.hookCooldown;
        cooldownTimer = HookCooldown;
        HookcooldownMaskImage.fillAmount = 3f;
    }
}
