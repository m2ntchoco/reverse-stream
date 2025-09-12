using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    PlayerData playerdata;
    [SerializeField] int maxHp2 = 50;
    int currentHp2;

    void Awake2()
    {
        currentHp2 = maxHp2;
    }

    private void Start()
    { 
        //Die2();
    }

    public void TakeDamage2(int damage)
    {
        currentHp2 -= damage;
        Debug.Log($"{name} took {damage} damage, remains {currentHp2} HP");

        if (currentHp2 <= 0)
            Die2();
    }

    void Die2()
    {
        // ��� ����Ʈ, ���� ��� �� �߰� ����
        Destroy(gameObject);
        Debug.Log("���");
        PlayerExpManager.AddExp(90);

    }


}
