using UnityEngine;

public class hp : MonoBehaviour
{
    public static float boss1_health = 3000;
    public static float boss2_health = 100;
    public static float SandWorm = 100;

    // �±׿� ���� MaxHP�� ��ȯ
    public static float GetMaxHPByTag(string tag)
    {
        switch (tag)
        {
            case "Boss1":
                return boss1_health;
            case "boss2":
                return boss2_health;
            case "SandWorm":
                return SandWorm;
            default:
                Debug.LogWarning($"���ǵ��� ���� �±�: {tag}");
                return 0;
        }
    }
}