using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class FullscreenStretch : MonoBehaviour
{
    RectTransform rt;
    int sw, sh;

    void Awake()
    {
        rt = (RectTransform)transform;
        Apply();
        sw = Screen.width; sh = Screen.height;
    }

    void OnEnable() => Apply();   // Ȱ��ȭ�� �� �� ��

    void Update()                 // �ػ�/â ũ�� �ٲ�鸸 �ٽ� ����
    {
        if (sw != Screen.width || sh != Screen.height)
        {
            sw = Screen.width; sh = Screen.height;
            Apply();
        }
    }

    void Apply()
    {
        rt.anchorMin = Vector2.zero; // (0,0)
        rt.anchorMax = Vector2.one;  // (1,1)
        rt.offsetMin = Vector2.zero; // L,B = 0
        rt.offsetMax = Vector2.zero; // R,T = 0
    }
}