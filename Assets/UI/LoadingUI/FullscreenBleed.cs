using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class FullscreenBleed : MonoBehaviour
{
    [SerializeField] Canvas targetCanvas;   // FaderCanvas �־�θ� ����
    [SerializeField] float bleedPixels = 1f;

    RectTransform rt;

    void Awake() { rt = (RectTransform)transform; Apply(); }
    void OnEnable() { Apply(); }
    void Update()
    {
        // â ũ�� �ٲ�� �ٽ� ����
        if (Screen.width != _w || Screen.height != _h) Apply();
    }

    int _w, _h;
    void Apply()
    {
        _w = Screen.width; _h = Screen.height;

        if (!targetCanvas) targetCanvas = GetComponentInParent<Canvas>();
        float scale = targetCanvas ? Mathf.Max(0.0001f, targetCanvas.scaleFactor) : 1f;
        float bleed = bleedPixels / scale; // ĵ���� ������ ����

        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(-bleed, -bleed); // ���Ϸ� 1px Ȯ��
        rt.offsetMax = new Vector2(bleed, bleed); // ������� 1px Ȯ��
    }
}
