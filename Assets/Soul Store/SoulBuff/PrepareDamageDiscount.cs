using UnityEngine;
using UnityEngine.Rendering;

public class PrepareDamageDiscount
{
    
    public static void PreDamageDiscount()
    {
        Debug.Log("PreDamageDiscount �����");
        PlayerHealth.discountDamage += 0.05f;
        Debug.Log($"������ ������ : {PlayerHealth.discountDamage}");
    }

   
}
