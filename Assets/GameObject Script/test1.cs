using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class test1 : MonoBehaviour
{

    private Transform playerT;

    void Start()
    {
        var go = GameObject.FindWithTag("Player");
        if (go != null) playerT = go.transform;
        else Debug.LogError("Player�� ã�� ����");
    }

    void Update()
    {
        /*if (playerT != null)
            Debug.Log("Player ��ġ: " + playerT.position);*/
    }


}
