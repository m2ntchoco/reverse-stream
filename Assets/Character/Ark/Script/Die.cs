using UnityEditor;
using UnityEngine;

public class Die : MonoBehaviour
{
    public bool isDead = false;
    private PlayerHealth health;
    private PlayerAnimatorController animController;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
        animController = GetComponent<PlayerAnimatorController>();
    }

    public void die()
    {
        if (PlayerHealth.currentHP <= 0 && !isDead)
        {
            isDead = true;
            animController.Die();
        }
        // 체력 감소 후 UI 갱신
        if (health.hpUI != null)
        {
            health.hpUI.SetHP((int)PlayerHealth.currentHP, (int)PlayerHealth.maxHP);
        }
    }
}

