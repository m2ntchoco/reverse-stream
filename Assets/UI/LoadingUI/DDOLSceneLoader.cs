using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOLSceneLoader : MonoBehaviour
{
    public static DDOLSceneLoader I { get; private set; }

    [Header("Refs")]
    [SerializeField] private LoadingUI loadingUI;   // LoadingUIRoot�� ������Ʈ
    [SerializeField] private CanvasGroup fader;     // FaderPanel�� CanvasGroup

    [Header("Timing")]
    [SerializeField] private float minDisplaySeconds = 2f;
    [SerializeField] private float fadeDuration = 0.25f;

    [Header("Debug")]
    [SerializeField] private bool autoTestOnStartup = true;
    [SerializeField] private string testSceneName = ""; // F10 �ε��

    [SerializeField] LoadingTipRotator tipRotator;
    public bool IsBusy { get; private set; }

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        if (!loadingUI) loadingUI = FindObjectOfType<LoadingUI>(true);
        if (!fader) fader = GameObject.Find("FaderPanel")?.GetComponent<CanvasGroup>();

        if (!loadingUI || !fader)
        {
            Debug.LogError("[Loader] Missing refs. loadingUI or fader not found.");
        }

        if (fader)
        {
            fader.alpha = 0f;
            fader.blocksRaycasts = false; // ������ �� Ŭ�� ���
        }
        if (loadingUI) loadingUI.Hide();
    }

    void Start()
    {
        //if (autoTestOnStartup) StartCoroutine(DebugShow2s());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9)) StartCoroutine(DebugShow2s());
        if (Input.GetKeyDown(KeyCode.F10) && !string.IsNullOrEmpty(testSceneName))
            LoadLevel(testSceneName);
    }

    public void LoadLevel(string nextScene)
    {
        if (IsBusy) { Debug.Log("[Loader] Busy, ignored."); return; }
        Debug.Log($"[Loader] LoadLevel({nextScene})");
        StartCoroutine(LoadRoutine(nextScene));
    }

    IEnumerator LoadRoutine(string sceneName)
    {
        IsBusy = true;
        LockInput(true);

        // 1) �ε� UI ���� ����
        loadingUI.Show();
        loadingUI.SetProgress(0f);
        loadingUI.SetSubtext("Preparing...");
        tipRotator?.Begin(sceneName);
        Canvas.ForceUpdateCanvases();
        yield return null; // ȭ�鿡 ������ �׷�����

        // 2) ���̵�ƿ�(��������)
        yield return Fade(1f);

        // 3) ǥ�� ���� �ð� ����
        float showStart = Time.realtimeSinceStartup;

        // 4) �񵿱� �ε�
        var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        op.allowSceneActivation = false;

        while (true)
        {
            float progress01 = Mathf.Clamp01(op.progress / 0.9f); // 0~0.9 �� 0~1
            float visual = Mathf.Lerp(loadingUI.Progress, progress01 * 0.9f, 0.25f);
            loadingUI.SetProgress(visual);

            if (op.progress < 0.9f) loadingUI.SetSubtext("Loading...");
            else loadingUI.SetSubtext("Almost there...");

            bool loadReady = op.progress >= 0.9f;
            bool minTime = (Time.realtimeSinceStartup - showStart) >= minDisplaySeconds;

            if (loadReady && minTime) break;
            yield return null;
        }

        // 5) Ȱ��ȭ
        loadingUI.SetSubtext("Activating...");
        loadingUI.SetProgress(0.98f);
        op.allowSceneActivation = true;
        while (!op.isDone) yield return null;

        // 6) ��ó��
        yield return DoPostActivateSetup();

        // 7) ������
        loadingUI.SetProgress(1f);
        yield return Fade(0f);
        loadingUI.Hide();
        tipRotator?.End();
        LockInput(false);
        IsBusy = false;
        Debug.Log("[Loader] Done");
    }

    IEnumerator DoPostActivateSetup()
    {
        yield return null; // �� ������Ʈ ��Ƴ� �� 1������
        // PlayerSpawner.SpawnAtEntrance();
        // RunSaveSystem.InjectToPlayer();
        // CameraRoomBinder.BindCurrentScene();
    }

    IEnumerator Fade(float target)
    {
        if (!fader) yield break;
        fader.gameObject.SetActive(true);
        fader.blocksRaycasts = target > 0f; // ����� �� Ŭ�� ����, ���������� ���

        float start = fader.alpha;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fader.alpha = Mathf.Lerp(start, target, t / fadeDuration);
            yield return null;
        }
        fader.alpha = target;

        if (target <= 0f) fader.blocksRaycasts = false;
    }

    IEnumerator DebugShow2s()
    {
        Debug.Log("[Loader] DebugShow2s start");
        loadingUI.Show();
        Canvas.ForceUpdateCanvases();
        yield return null;
        yield return Fade(1f);
        yield return new WaitForSecondsRealtime(2f);
        yield return Fade(0f);
        loadingUI.Hide();
        Debug.Log("[Loader] DebugShow2s end");
    }

    void LockInput(bool on)
    {
        // PlayerInput.enabled = !on; ���� ������Ʈ ���� �߰�
    }
}
