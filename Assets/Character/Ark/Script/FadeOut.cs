using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FadeOut : MonoBehaviour
{
    [Tooltip("�ܻ��� ������ ����� ������ �ɸ��� �ð�(��)")]
    public float fadeDuration = 0.3f;

    [Tooltip("0���� ��(0)�� fadeDuration�� ��(1)�� ���� ���� �")]
    public AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private SpriteRenderer sr;
    private Color startColor;
    private float timer;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startColor = sr.color;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / fadeDuration);
        float curveA = alphaCurve.Evaluate(t);
        sr.color = new Color(startColor.r, startColor.g, startColor.b, startColor.a * curveA);

        if (timer >= fadeDuration)
            Destroy(gameObject);
    }
}
