// LoadingTipRotator.cs (drop-in ��ü)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingTipRotator : MonoBehaviour
{
    [Header("Data & UI")]
    [SerializeField] private LoadingTipsDatabase db;
    [SerializeField] private LoadingUI ui;
    [SerializeField] private Typewriter typewriter;     // (����) ������ Ÿ�ڱ� ȿ���� ���

    [Header("Behaviour")]
    [SerializeField] private float intervalSec = 3f;    // ��ü �ֱ�
    [SerializeField] private bool unscaledTime = true;  // �ε� �߿� ���� true
    [SerializeField] private string requiredTag = "";   // Ư�� �±׸�(�� ���̸� ��ü)
    [SerializeField] private bool showImmediately = true;       // Begin() �� ��� 1ȸ ǥ��
    [SerializeField] private bool avoidImmediateRepeat = true;  // ���� ���� �ٷ� �ݺ� ����

    readonly List<int> _bag = new();     // ����ġ ��÷ ����
    Coroutine _co;
    int _lastIdx = -1;
    string _activeTag = "";

    void Awake()
    {
        if (!ui) ui = FindObjectOfType<LoadingUI>(true);
        if (!typewriter) typewriter = FindObjectOfType<Typewriter>(true); // ������ null�̾ OK
    }

    /// <summary>
    /// �������� ����. tagOverride�� Ư�� �±׸� ������ �� ����(������ �� requiredTag ���)
    /// </summary>
    public void Begin(string sceneName = null, string tagOverride = null)
    {
        _activeTag = string.IsNullOrEmpty(tagOverride) ? requiredTag : tagOverride;
        Debug.Log("[Tip] Begin");
        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(Run());
    }

    public void End()
    {
        if (_co != null) StopCoroutine(_co);
        _co = null;
        _bag.Clear();
        _lastIdx = -1;
        Debug.Log("[Tip] End");
        // �ε� ���� �� �� ���� ����(���ϸ� �����ص� ��)
        if (typewriter != null) typewriter.Play("");
        else ui?.SetTip("");
    }

    IEnumerator Run()
    {
        if (!db || db.tips == null || db.tips.Count == 0)
        {
            Debug.LogWarning("[LoadingTipRotator] Tips database is empty.");
            yield break;
        }

        RefillBag();
        if (_bag.Count == 0)
        {
            Debug.LogWarning("[LoadingTipRotator] No tips matched filters.");
            yield break;
        }

        // ���� ��� 1ȸ ����
        if (showImmediately)
        {
            ShowNextTip();
        }

        // �ֱ������� ��ü
        float t = 0f;
        while (true)
        {
            float dt = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;
            if (t >= intervalSec)
            {
                t = 0f;
                ShowNextTip();
            }
            yield return null;
        }
    }

    void ShowNextTip()
    {
        if (_bag.Count == 0) RefillBag();
        if (_bag.Count == 0) return;

        int idx = Draw();
        if (idx < 0) return;

        string text = db.tips[idx].text;

        if (typewriter != null) typewriter.Play(text);
        else ui?.SetTip(text);

        _lastIdx = idx;
    }

    void RefillBag()
    {
        _bag.Clear();
        if (!db) return;

        var lang = Application.systemLanguage;

        // ����(���/�±�) �´� �׸� ����ġ��ŭ ä���
        for (int i = 0; i < db.tips.Count; i++)
        {
            var tip = db.tips[i];

            if (!string.IsNullOrEmpty(_activeTag) && tip.tag != _activeTag)
                continue;

            // ��� ��Ī: �ý��� ��� �켱, ������ ���� ���
            bool langOK = (tip.language == lang) ||
                          (tip.language == SystemLanguage.English) ||
                          (lang == SystemLanguage.Unknown);

            if (!langOK) continue;

            int count = Mathf.Max(1, Mathf.RoundToInt(tip.weight * 10f));
            for (int c = 0; c < count; c++) _bag.Add(i);
        }

        // ����
        for (int i = 0; i < _bag.Count; i++)
        {
            int j = Random.Range(i, _bag.Count);
            (_bag[i], _bag[j]) = (_bag[j], _bag[i]);
        }
    }

    int Draw()
    {
        if (_bag.Count == 0) return -1;

        // ������ ���� ������ ������ �ʵ��� �� �� �� �õ�
        if (avoidImmediateRepeat && _bag.Count > 1)
        {
            for (int tries = 0; tries < 3; tries++)
            {
                int pick = Random.Range(0, _bag.Count);
                int idx = _bag[pick];
                if (idx != _lastIdx)
                {
                    // ����(pop) �� ��ȯ
                    _bag[pick] = _bag[_bag.Count - 1];
                    _bag.RemoveAt(_bag.Count - 1);
                    return idx;
                }
            }
        }

        // �Ϲ� ��÷
        int p = Random.Range(0, _bag.Count);
        int result = _bag[p];
        _bag[p] = _bag[_bag.Count - 1];
        _bag.RemoveAt(_bag.Count - 1);
        return result;
    }
}
