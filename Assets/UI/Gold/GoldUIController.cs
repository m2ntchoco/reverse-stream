//using UnityEngine;
//using UnityEngine.UIElements;

//public class GoldUIController : MonoBehaviour
//{
//    private Label goldLabel;

//    private void Start()
//    {
//        Debug.Log("[UI] GoldUIController Ȱ��ȭ��");
//        var root = GetComponent<UIDocument>().rootVisualElement;

//        goldLabel = root.Q<Label>("GoldText");

//        if (goldLabel == null)
//        {
//            Debug.LogError("GoldText ���� ã�� �� �����ϴ�.");
//            return;
//        }

//        // ��� ���� �� UI ����
//        PlayerGoldManager.Instance.OnGoldChanged += UpdateGoldText;
//        UpdateGoldText(PlayerGoldManager.Instance.Gold);
//    }

//    private void OnDisable()
//    {
//        PlayerGoldManager.Instance.OnGoldChanged -= UpdateGoldText;
//    }

//    private void UpdateGoldText(int gold)
//    {
//        Debug.Log($"[UI] ��� UI ������Ʈ: {gold}");

//        if (goldLabel != null)
//            goldLabel.text = $"{gold}G";
//        else
//            Debug.LogError("GoldText ���� ã�� �� �����ϴ�");
//    }

//}
