
public interface IBackPanel
{
    bool IsOpen { get; }
    int BackPriority { get; }   // ���� Ŭ���� ���� ���� (�κ��丮 > ���� > ... > �Ͻ�����)
    void Show();
    void Hide();
}

