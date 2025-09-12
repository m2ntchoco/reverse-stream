using UnityEngine;
using UnityEngine.UIElements;

public abstract class BackPanelBase : MonoBehaviour, IBackPanel
{
    public bool IsOpen { get; private set; }
    public abstract int BackPriority { get; } // ���� ��Ŀ����� �� �� ���� �� ������ ����

    protected UIDocument doc;
    protected VisualElement viewRoot; // name="Root"�� ������ �װ�, ������ ��ü

    protected virtual void Awake()
    {
        doc = GetComponent<UIDocument>();
        var r = doc.rootVisualElement;
        viewRoot = r.Q<VisualElement>("Root") ?? r;
    }

    public virtual void Show()
    {
        viewRoot.style.display = DisplayStyle.Flex;
        if (!IsOpen)
        {
            IsOpen = true;
            UINavigator.Instance?.NotifyOpened(this);
        }
    }

    public virtual void Hide()
    {
        viewRoot.style.display = DisplayStyle.None;
        if (IsOpen)
        {
            IsOpen = false;
            UINavigator.Instance?.NotifyClosed(this);
        }
    }
}
