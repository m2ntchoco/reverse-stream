using UnityEngine;

public class Dome:MonoBehaviour
{
    public Boss1_SkillManager SkillManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss1"))  // �浹 ��� �±� Ȯ�� (�ʿ� �� ����)
        {
            //SkillManager.CantDash = true;
            Debug.Log("�뽬 ���� ���� ����");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Boss1"))
        {
            //SkillManager.CantDash = false;
            Debug.Log("�뽬 ���� ���� Ż��");
        }
    }
}
