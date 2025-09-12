using UnityEngine;

public class Soul_Store_Npc : MonoBehaviour
{
    [SerializeField] private SoulStoreManager soulStoreManager;
    [SerializeField] private Transform steamPlayer;
    [SerializeField] private Transform magicPlayer;
    [SerializeField] private ChooseOne chooseOne;

    [SerializeField] private float interactDistance = 4f;

    void Update()
    {
        // 1) ���� ��Ʈ�� ���� �÷��̾� ��ġ
        Vector2 playerPos;
        if (chooseOne.SystemSteamPunk) playerPos = steamPlayer.position;
        else if (chooseOne.SystemMagic) playerPos = magicPlayer.position;
        else return;

        // 2) ���� üũ
        bool inRange = Vector2.Distance(playerPos, (Vector2)transform.position) < interactDistance;

        // 3) Z Ű�� UI ���
        if (inRange && Input.GetKeyDown(KeyCode.F) && soulStoreManager != null)
        {
            if (!soulStoreManager.IsOpen) soulStoreManager.Show();
            else soulStoreManager.Hide();
        }
    }
}
