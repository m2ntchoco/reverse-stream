using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject interactTarget;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && interactTarget != null)
        {
            string tag = interactTarget.tag;

            switch (tag)
            {
                case "Weapon":
                    GroundItem groundItem = interactTarget.GetComponent<GroundItem>();
                    if (groundItem != null)
                    {
                        bool success = InventoryUIController.Instance.TryAddItem(groundItem.data);
                        if (success)
                        {
                            Destroy(interactTarget);
                            Debug.Log("������ ȹ�� �Ϸ�!");
                        }
                        else
                        {
                            Debug.Log("�κ��丮�� ���� á���ϴ�.");
                        }
                    }
                    break;

                case "NPC":
                    Debug.Log("NPC ��ȭ ����!");
                    break;

                case "Store":
                    StoreUIController.Instance.OpenStoreUI();
                    break;

                case "Object":
                    Debug.Log("��ȣ�ۿ� ������ ������Ʈ!");
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon") || other.CompareTag("NPC") || other.CompareTag("Object") || other.CompareTag("Store"))
        {
            interactTarget = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == interactTarget)
        {
            interactTarget = null;
        }
    }
}
