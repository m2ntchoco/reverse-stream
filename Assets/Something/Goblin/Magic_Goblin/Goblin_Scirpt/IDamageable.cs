using UnityEngine;

/// <summary>
/// ������ ���� ����� �����ؾ� �� ���� ���
/// </summary>
public interface IDamageable
{
    /// <param name="damage">��������</param>
    /// <param name="source">������ �߻��� ��ġ</param>
    /// <param name="knockbackForce">���� �˹� ����</param>
    /// <param name="upwardForce">���� �˹� ����</param>
    void TakeDamage(int damage, Transform source, float knockbackForce, float upwardForce);
}