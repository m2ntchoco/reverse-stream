using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FinalBossUI : MonoBehaviour
{
    [SerializeField] FinalBossUI FinalbossUI;
    [SerializeField] private GameObject BossUIController;
    [SerializeField] private GameObject BossObjects;
    [SerializeField] private Boss1_FSM Boss1_fsm;
    private float BossMaxHP;
    public Image BossHPfillImage;
    public TextMeshProUGUI BossNameText;
    public TextMeshProUGUI BossHPText;
    public Image GrogyBarFillImage;
    public float grogyGaugeMax;
    private bool prevGroggyState = false;

    void Start()
    {
        if (BossUIController != null)
        {
            BossUIController.SetActive(false);
        }
        BossMaxHP = hp.GetMaxHPByTag(gameObject.tag);
        string objectTag = BossObjects.tag;
        BossNameText.text = $"�ٸ��� ��� ����";
        grogyGaugeMax = Boss1_fsm.groggyGauge;

    }

    void Update()
    {
        if (BossObjects != null && BossObjects.activeInHierarchy)
        {
            BossUIController.SetActive(true);
            UpdateBossUI();

            if (prevGroggyState && !Boss1_fsm.isGroggy)
            {
                GrogyBarFillImage.fillAmount = 0f;
            }

            prevGroggyState = Boss1_fsm.isGroggy;
        }
        else
        {
            BossUIController.SetActive(false);
        }
        if (!Boss1_fsm.isGroggy)
        {
            GrogyBarFillImage.fillAmount = Mathf.Clamp01(Boss1_fsm.damageAccumulator / grogyGaugeMax);
        }
        else
        {
            // �׷α� ������ ���� �׻� �ٸ� �� ä��
            GrogyBarFillImage.fillAmount = 1f;
        }
    }

    public void UpdateBossUI()
    {
        if (BossUIController.activeInHierarchy)
        {
            Debug.Log($"[UI] ü�� ǥ��: {Boss1_FSM.currentHP} / {BossMaxHP}");
            BossHPfillImage.fillAmount = Boss1_FSM.currentHP / BossMaxHP;
        }
    }

}
