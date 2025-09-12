using System.Collections;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public Boss1_FSM FSM;
    [SerializeField] public GameObject Phase1_Ground;
    public Boss1_Pillar targetPillar;
    public GameObject Circle;
    public void Awake()
    {
     FSM = GetComponent<Boss1_FSM>();   
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Phase1_Ground)
        {
            // Phase1_Ground�� ������ phase1Ground ������Ʈ ����
            Destroy(Circle);
            Debug.Log("Phase1_Ground�� �浹: ���ŵ�");
        }
        else if (collision.CompareTag("Player"))
        {
            // �÷��̾� �±׿� ������ TakeDamage ȣ��
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(5,transform, 0f,0f);
                StartCoroutine(punk());
                Debug.Log("�÷��̾ �������� ����");
                Destroy(Circle);
            }
        }
    }
    private IEnumerator punk()
    {
        yield return new WaitForSeconds(2f);
        Destroy(Circle);
    }








}
    