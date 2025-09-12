using UnityEngine;

public class ChaseState : IEnemyState
{
    private EnemyAI enemy;
    private MonsterAnimatorController ani;
    private float cooldown = 1f;    //��ٿ� 1�� ����
    private float timer; // Ÿ�̸� ����

    public ChaseState(EnemyAI enemy, MonsterAnimatorController ani) // ���� ���
    {
        this.enemy = enemy;
        this.ani = ani;
    }

    public void Enter()
    {
        timer = 0f;
        if (enemy.CompareTag("Wolf"))
        {
            enemy.moveSpeed = 3.0f;
        }
        else if (enemy.CompareTag("Dwarf"))
        {
            enemy.moveSpeed = 2.3f;
        }
        else if (enemy.CompareTag("Dwarf_Hammer"))
        {
            enemy.moveSpeed = 2.3f;
        }
        else if (enemy.CompareTag("Goblin")) enemy.moveSpeed = 3f;
        else
        {
            enemy.moveSpeed = 2f;
        }
    }
    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            // �켱���� 1: �÷��̾ ���� �����ϸ� ���� ���·� ��ȯ
            if (enemy.playerAttackable && enemy.IsPlayerInRange())
            {
                enemy.ChangeState(enemy.attackState);
                Debug.Log("���� ��� ��ȯ");
                ani.SetMoving(false);
            }
            // �켱���� 2: �÷��̾ �þ� �ȿ��� ������ ���� ���� ����
            else if (enemy.IsPlayerInRange())
            {
                enemy.moveSpeed = 1f;
                ani.SetMoving(true);
                enemy.MoveTowardsPlayer();
                enemy.GetAnimator().speed = 1.5f;
                Debug.Log("���� �� (���� �Ұ�)");
            }
            // �켱���� 3: �þ� ���̸� ��� ���� ��ȯ
            else
            {
                if (enemy.currentState != enemy.idleState)
                {
                    enemy.SetDetectionRadius(8f);
                    enemy.ChangeState(enemy.idleState);
                    //Debug.Log("��� ��� ��ȯ");

                }
            }
            timer = 0f;
        }
    }



    public void Exit()
    {
        enemy.GetAnimator().speed = 1f;
    }
}
