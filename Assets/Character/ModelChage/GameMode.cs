/// <summary>
/// �÷��̾ ������ ��带 �����ϴ� enum
/// </summary>
public enum GameMode
{
    Steampunk,  // ������ũ �׸�
    Rune        // �� ���� �׸�
}
public class GameType
{
    public bool type(GameMode type)
    {
        if(type == GameMode.Steampunk)
            return true;
        else
            return false;
    }
}

/*public static class CritChanceCal
{
    public static bool CritChanceOK = false;
    public static void CritChanceCalculate(GameMode type)
    {
        if (type == GameMode.Steampunk)
        {
            if (SteamPunk_Attack.Critrand <= Attack_Damage.SoulBuffCritChance)
            {
                //Debug.Log($"ũ��Ƽ�� Ȯ�� ���� ���� {Attack.Critrand}, ũ��Ƽ�� �������� ����˴ϴ�.");
                CritChanceOK = true;
            }
            else
            {
                //Debug.Log($"ũ��Ƽ�� Ȯ�� ���� ���� {Attack.Critrand}, ũ��Ƽ�� �������� ���� �ȵ˴ϴ�.");
            }
        }
        else
        {
            if (Magic_Attack.Critrand <= Attack_Damage.SoulBuffCritChance)
            {
                //Debug.Log($"ũ��Ƽ�� Ȯ�� ���� ���� {Attack.Critrand}, ũ��Ƽ�� �������� ����˴ϴ�.");
                CritChanceOK = true;
            }
            else
            {
                //Debug.Log($"ũ��Ƽ�� Ȯ�� ���� ���� {Attack.Critrand}, ũ��Ƽ�� �������� ���� �ȵ˴ϴ�.");
            }
        }
    }
}*/