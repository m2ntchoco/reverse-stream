using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth player;
    public Image hpFillImage;
    public Image guardFillImage;

    void Update()
    {
        if (player == null) return;

        // ü�� fillAmount ����
        hpFillImage.fillAmount = Mathf.Clamp01((float)player.CurrentHP / player.MaxHP);

        // ���� fillAmount ����
        guardFillImage.fillAmount = Mathf.Clamp01(player.GuardRatio);
    }
}
