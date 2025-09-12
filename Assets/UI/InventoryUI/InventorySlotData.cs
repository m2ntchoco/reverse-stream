using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlotData
{
    public VisualElement slotElement;
    public WeaponPrefabData weaponData;

    public InventorySlotData(VisualElement slotElement)
    {
        this.slotElement = slotElement;
    }

    public void SetItem(WeaponPrefabData data)
    {
        weaponData = data;

        if (data == null)
            Debug.LogError("[SetItem] null ���Ⱑ �Ѿ��");
        else
            //Debug.Log($"[SetItem] ����: {(data != null ? data.type.ToString() : "����")} / ������ : {(data?.icon != null ? data.icon.name : "null")}");
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        slotElement.Clear();

        if (weaponData != null && weaponData.icon != null)
        {
            //Debug.Log($"[UpdateIcon] ������ ǥ�õ�: {weaponData.icon.name}");
            var icon = new VisualElement();
            icon.style.backgroundImage = new StyleBackground(weaponData.icon);
            icon.style.width = 120;
            icon.style.height = 142;
            icon.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            slotElement.Add(icon);
        }
        else
        {
            Debug.Log($"[UpdateIcon] ������ ����. weaponData null? : {weaponData == null}");
        }
    }
    public void Clear(bool inculdeData = true)
    {
        if (inculdeData)
            weaponData = null;
        
        slotElement.Clear();
    }

}
