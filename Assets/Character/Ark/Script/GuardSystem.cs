using UnityEngine;
using System.Collections;

public class GuardSystem : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] public float maxGuardPower = 20f;    // �ִ� ���� �Ŀ�
    [SerializeField] public float guardDisableTime = 3f;  // ���� �Ŀ� 0 ���� ������� �ɸ��� �ð�
    public float currentGuardPower;                       // ���� ���� �Ŀ�
    public bool isGuardDisabled = false;                  // ���� ���� �Ҵ� ���� ����
    public float guardDisabledTimer = 0f;                 // ���� �Ҵ� Ÿ�̸�
    public bool isGuarding;   // PlayerHealth �ʿ��� �����ϴ� ���� ����

    //private PlayerAnimatorController animController;
    private PlayerAnimationSync sync;
    void Awake()
    {
        //animController = GetComponentInParent<PlayerAnimatorController>();
        sync = GetComponentInParent<PlayerAnimationSync>();
    }

    public void Update() 
    {
        bool guardingNow = !isGuardDisabled && Input.GetKey(KeyCode.S);

        //���� �Է� ���� (S Ű�� ������ ���� Ȱ��ȭ)
        //Debug.Log(guardingNow);
        if (Input.GetKeyDown(KeyCode.S))
        {
            isGuarding = true;
            //animController.Guard();
            sync.Guard();
            StartCoroutine(Guarding());
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            isGuarding = false;
            //animController.NotGuard();
            sync.NotGuard();
        }

        // [2] ���� ��Ȱ��ȭ ������ ��� Ÿ�̸ӷ� ������
        if (isGuardDisabled)
        {
            guardDisabledTimer += Time.deltaTime;
            if (guardDisabledTimer >= guardDisableTime)
            {
                currentGuardPower = maxGuardPower;     // ���� �Ŀ� ���� ȸ��
                isGuardDisabled = false;               // ���� ���� ���·� ��ȯ
                guardDisabledTimer = 0f;
            }
        }
    }
    private IEnumerator Guarding()
    {
        //animController.Guarding();
        sync.Guarding();
        yield return new WaitForSeconds(1f);
    }
}
