// �� ������ PlayerAnimatorController�� ������ API ȣ���� ����ȭ�ؼ� ����
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationSync : MonoBehaviour
{
    private PlayerAnimatorController bodyController;
    private PlayerAnimatorController effectController;
    private PlayerAnimatorController weaponController;
    private Animator effectAnimator;

    private void Awake()
    {
        // �ڽ� ������Ʈ �̸��� ��Ȯ�ϴٸ� transform.Find �� �ٷ� ����
        bodyController = transform.Find("Body").GetComponent<PlayerAnimatorController>();
        effectController = transform.Find("Effect").GetComponent<PlayerAnimatorController>();
        weaponController = transform.Find("Weapon").GetComponent<PlayerAnimatorController>();

        effectAnimator = effectController.GetComponent<Animator>();
        if (effectAnimator == null)
            Debug.LogError("[PlayerAnimationSync] Effect Animator�� ã�� �� �����ϴ�!");

        // �Ǵ� �ڽ� ���� �ȿ��� Ÿ������ �� ���� ������ �ʹٸ�
        // var ctrls = GetComponentsInChildren<PlayerAnimatorController>();
        // bodyController  = ctrls.First(c => c.gameObject.name == "Body");
        // armorController = ctrls.First(c => c.gameObject.name == "Armor");
        // weaponController= ctrls.First(c => c.gameObject.name == "Weapon");
    }
    public AnimatorStateInfo CurrentStateInfo
        => effectAnimator.GetCurrentAnimatorStateInfo(0);

    // ����: �Ϲ� ����
    public void NomalAttack(int count)
    {
        bodyController.NomalAttack(count);
        effectController.NomalAttack(count);
        weaponController.NomalAttack(count);
    }
    public void OverHitAttack(int count)
    {
        bodyController.OverHitAttack(count);
        effectController.OverHitAttack(count);
        weaponController.OverHitAttack(count);
    }

    public void SideCommand()
    {
        bodyController.SideCommand();
        effectController.SideCommand();
        weaponController.SideCommand();
    }

    public void DownCommand()
    {
        bodyController.DownCommand();
        effectController.DownCommand();
        weaponController.DownCommand();
    }

    public void Guard()
    {
        bodyController.Guard();
        effectController.Guard();
        weaponController.Guard();
    }

    public void Guarding()
    {
        bodyController.Guarding();
        effectController.Guarding();
        weaponController.Guarding();
    }

    public void NotGuard()
    {
        bodyController.NotGuard();
        effectController.NotGuard();
        weaponController.NotGuard();
    }

    public void GuardBreak()
    {
        bodyController.GuardBreak();
        effectController.GuardBreak();
        weaponController.GuardBreak();
    }

    public void Jump()
    {
        bodyController.Jump();
        effectController.Jump();
        weaponController.Jump();
    }

    public void DoubleJump()
    {
        bodyController.DoubleJump();
        effectController.DoubleJump();
        weaponController.DoubleJump();
    }

    public void Dash()
    {
        bodyController.Dash();
        effectController.Dash();
        weaponController.Dash();
    }

    public void IsGround(bool nowGrounded)
    {
        bodyController.IsGround(nowGrounded);
        effectController.IsGround(nowGrounded);
        weaponController.IsGround(nowGrounded);
    }

    public void RunAttack()
    {
        bodyController.RunAttack();
        effectController.RunAttack();
        weaponController.RunAttack();
    }

    public void AirSpeedY(float y)
    {
        bodyController.AirSpeedY(y);
        effectController.AirSpeedY(y);
        weaponController.AirSpeedY(y);
    }

    public void IsWalking(bool moveInput)
    {
        bodyController.IsWalking(moveInput);
        effectController.IsWalking(moveInput);
        weaponController.IsWalking(moveInput);
    }

    public void Die()
    {
        bodyController.Die();
        effectController.Die();
        weaponController.Die();
    }

    public void ApplyAttackSpeed()
    {
        bodyController.ApplyAttackSpeed();
        effectController.ApplyAttackSpeed();
        weaponController.ApplyAttackSpeed();
    }
}
