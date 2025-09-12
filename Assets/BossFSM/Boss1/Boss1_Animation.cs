using UnityEngine;

public class Boss1_Animation : MonoBehaviour 
{
    public Animator ani;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    /*public void Jump()
    {
        ani.SetTrigger("IsJump");
    }*/

    public void JumpAttack()
    {
        ani.SetTrigger("IsTop");
    }
    public void KeepGoing()
    {
        ani.SetBool("KeepGoing",true);
    }
    public void KeepEnd()
    {
        ani.SetBool("KeepGoing",false);
    }
    public void PrepareJump()
    {
        ani.SetTrigger("IsPrepareJump");
    }

    public void ThrowBoomerang()
    {
        ani.SetTrigger("IsThrow");
    }

    public void CatchBoomerang()
    {
        ani.SetTrigger("IsCatch");
    }

    public void WheelPrepare()
    {
        ani.SetTrigger("IsWheel");
    }

    public void WheelStart()
    {
        ani.SetTrigger("WheelStart");
    }

    public void WheelEnd()
    {
        ani.SetTrigger("WheelEnd");
    }

    public void BackStep()
    {
        ani.SetTrigger("IsBackstep");
    }
    
    public void PhaseChange()
    {
        ani.SetTrigger("ISCHANGING");
    }

    public void EnThrow()
    {
        ani.SetTrigger("Enthrow");
    }

    public void Dash()
    {
        ani.SetTrigger("Dash");
    }
    public void Groggy()
    {
        ani.SetTrigger("Groggy");
    }
    public void GroggyEnd()
    {
        ani.SetTrigger("GroggyEnd");
    }
    public void Nattack()
    {
        ani.SetTrigger("Nattack");
    }
    public void Nattackprepare()
    {
        ani.SetTrigger("Nattackprepare");
    }
}
