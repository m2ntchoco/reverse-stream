using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// �κ��丮 �������� ���콺�� ���� �ٴϴ� ��ɸ� ����ϴ� Ŭ����
/// </summary>
public class InventoryDrag_Drop
{
    // UI ��ü ȭ��
    private VisualElement root;
    // ���콺 ����ٴϴ� ������ �̹���
    private VisualElement dragIcon;

    // �巡�� ������ �ƴ��� �˷��ִ� ����
    public bool isDragging { get; private set; } = false;
    // �巡�� ���� �������� ����
    public WeaponPrefabData currentItem;
    // �������� ���� ������ ���� ��ȣ
    public int fromSlotIndex;

    /// <summary>
    /// Ŭ������ �����ڸ��� �ϴ� �ϵ��� ��Ƶ� ������ �Լ�
    /// </summary>
    /// <param name="root"></param>
    public InventoryDrag_Drop(VisualElement root)
    {
        this.root = root;

        dragIcon = new VisualElement();
        dragIcon.style.position = Position.Absolute;
        dragIcon.style.width = 120;
        dragIcon.style.height = 142;
        dragIcon.style.visibility = Visibility.Hidden;

        root.Add(dragIcon);
        root.RegisterCallback<MouseMoveEvent>(OnMouseMove);
    }

    /// <summary>
    /// ���콺�� ������ ������ ����Ǵ� �Լ�
    /// </summary>
    /// <param name="evt"></param>
    void OnMouseMove(MouseMoveEvent evt)
    {
        if (isDragging)
        {
            // �������� ���콺 ������ �߽� ��ó�� ������ ��ġ ����
            dragIcon.style.left = evt.mousePosition.x - 32;
            dragIcon.style.top = evt.mousePosition.y - 32;
        }
    }

    /// <summary>
    /// �巡�� ������ �� ȣ���ϴ� �Լ�
    /// </summary>
    /// <param name="item"></param>
    /// <param name="fromIndex"></param>
    public void BeginDrag(WeaponPrefabData data, int fromIndex)
    {
        if (data == null) return;
        Debug.Log("�巡�� ���۵�");
        isDragging = true;
        currentItem = data;
        fromSlotIndex = fromIndex;
        dragIcon.style.backgroundImage = new StyleBackground(data.icon);
        dragIcon.style.visibility = Visibility.Visible;
    }

    /// <summary>
    /// �巡�� ���� �� ȣ���ϴ� �Լ�
    /// </summary>
    public void EndDrag()
    {
        Debug.Log("�巡�� �����");
        isDragging = false;
        dragIcon.style.visibility = Visibility.Hidden;
        currentItem = null;
    }
}
