using UnityEngine;
using System.Collections;
public class Boss1_Phasechange: MonoBehaviour
{
    /*public static bool Is2phase = false;
    [Header("Wiring (���� ����)")]
    public Renderer Renderer;
    public GameObject boss;
    [SerializeField] public Transform Phase1_Ground;

    [Header("Auto-Refs")]
    public Boss1_FSM fsm;
    public BoxColliderGizmo gizmo;

    void Awake()
    {
        // 1) FSM ã��: boss ������Ʈ �� ���� �� ��ü ��(��Ȱ�� ����)
        if (fsm == null && boss != null)
            fsm = boss.GetComponentInChildren<Boss1_FSM>(true);
        if (fsm == null)
            fsm = Object.FindAnyObjectByType<Boss1_FSM>(FindObjectsInactive.Include);

        if (fsm == null)
            Debug.LogWarning("[Boss1_Phasechange] Boss1_FSM�� ã�� ���߽��ϴ�. Boss1�� ���� ���ų� ��ũ��Ʈ�� ��Ȱ���� �� �ֽ��ϴ�.");

        // 2) Gizmo
        if (gizmo == null)
            gizmo = Object.FindAnyObjectByType<BoxColliderGizmo>(FindObjectsInactive.Include);
        if (gizmo == null)
            Debug.LogWarning("[Boss1_Phasechange] BoxColliderGizmo�� ã�� ���߽��ϴ�.");

        // 3) Renderer: �������̸� �ڱ� �ڽ�/�θ� �ʿ��� �ڵ� Ž��
        if (Renderer == null)
            Renderer = GetComponentInChildren<Renderer>(true) ?? GetComponent<Renderer>();
        if (Renderer == null)
            Debug.LogWarning("[Boss1_Phasechange] Renderer�� �����ϴ�. ���� ������ ������� �ʽ��ϴ�.");

        // 4) Phase1_Ground �ڵ� ����(�ɼ�): �̸� ���(�̹��� Ʈ���� �̸� ������ �õ�)
        if (Phase1_Ground == null)
        {
            var t = transform.root.Find("BossMap/Phase1_Ground");
            if (t == null) t = GameObject.Find("Phase1_Ground")?.transform;
            Phase1_Ground = t;
            if (Phase1_Ground == null)
                Debug.LogWarning("[Boss1_Phasechange] Phase1_Ground�� ã�� ���߽��ϴ�. �ı��� �����˴ϴ�.");
        }
    }
    void Start()
    {
        fsm = Object.FindAnyObjectByType<Boss1_FSM>();
        gizmo = Object.FindAnyObjectByType<BoxColliderGizmo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fsm.hit == true)
        {
            StartCoroutine(Phasechange());
            Debug.Log("������");
            fsm.hit = false;
            
        }
    }



    public IEnumerator Phasechange()
    {
    
        Is2phase = true;
        gizmo.DdalGGak = false;
        Renderer.sortingOrder = 20;
        Destroy(Phase1_Ground.gameObject);
        yield return new WaitForSeconds(2f);
        Renderer.sortingOrder = -5;
        

    }*/
}
