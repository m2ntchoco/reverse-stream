using UnityEngine;
using System.Collections;

public class Boss1_Pillar : MonoBehaviour
{
    [SerializeField]public int maxHealth = 10;
    public int currentHealth;

    [Header("object")]
    public GameObject dropPrefab;
    public Boss1_Pillar targetPillar;
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"������ -1 {currentHealth}");
        if (currentHealth <= 0)
        {
            Object.FindAnyObjectByType<Boss1_FSM>()?.pillars.Remove(this);
            StartCoroutine(Fallpillar());

            // �ı� �� �̺�Ʈ�� �޽��� ������ ����
        }
    }

    public void Fallenpillar()
    {
        Transform fallenPoint = targetPillar.transform.Find("FallenPoint");
        Transform fallenPoint1 = targetPillar.transform.Find("FallenPoint1");
        Transform fallenPoint2 = targetPillar.transform.Find("FallenPoint2");

        if (dropPrefab != null)
        {
            Instantiate(dropPrefab, fallenPoint.position, Quaternion.identity);
            Instantiate(dropPrefab, fallenPoint1.position, Quaternion.identity);
            Instantiate(dropPrefab, fallenPoint2.position, Quaternion.identity);
        }
    }

    public IEnumerator Fallpillar()
    {
        Debug.Log("�� �������پ�");
        yield return new WaitForSeconds(2f);
        Fallenpillar();
        yield return new WaitForSeconds(1f);
        Fallenpillar();
        Destroy(gameObject);
    }
}
