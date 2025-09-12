using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class Boss1_BackDash : MonoBehaviour
{
    //public float dashDistance = 0.001f;
    public float dashSpeed = 0.1f;
    public Rigidbody2D rb;
    public Transform player;
    public Boss1_Coroutine coroutine;
    public Boss1_Animation ani;
    public Boss1_Phase1 phase1;

    public void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Boss1_Animation>();
        coroutine = GetComponent<Boss1_Coroutine>();
        phase1 = GetComponent<Boss1_Phase1>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
