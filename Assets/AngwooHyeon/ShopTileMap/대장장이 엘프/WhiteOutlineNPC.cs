using UnityEngine;

public class WhiteOutlineNPC : MonoBehaviour
{
    public SpriteRenderer mainRenderer;
    private SpriteRenderer outlineRenderer;

    void Start()
    {
        // �ڽ� ������Ʈ ����
        GameObject outlineObj = new GameObject("Outline");
        outlineObj.transform.parent = transform;
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localScale = Vector3.one * 1.05f;

        // SpriteRenderer ���� �� ����
        outlineRenderer = outlineObj.AddComponent<SpriteRenderer>();
        outlineRenderer.sprite = mainRenderer.sprite;
        outlineRenderer.sortingLayerID = mainRenderer.sortingLayerID;
        outlineRenderer.sortingOrder = mainRenderer.sortingOrder - 1;

        // ��Ƽ���� ����
        Material mat = Resources.Load<Material>("WhiteSilhouetteMat");
        if (mat != null)
        {
            outlineRenderer.material = mat;
        }
        else
        {
            Debug.LogError("WhiteSilhouetteMat�� Resources ������ �־����� Ȯ��!");
        }
    }

    void LateUpdate()
    {
        if (mainRenderer != null && outlineRenderer != null)
        {
            outlineRenderer.sprite = mainRenderer.sprite;
            outlineRenderer.flipX = mainRenderer.flipX;
        }
    }
}
