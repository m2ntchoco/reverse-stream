using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(UIDocument))]
public class PauseMenuController : BackPanelBase
{
    public override int BackPriority => 10; // �ǹ̻� ����

    Button btnResume, btnNew, btnControls, btnSettings, btnQuit;

    protected override void Awake()
    {
        base.Awake();
        var r = doc.rootVisualElement;

        btnResume = r.Q<Button>("BtnResume");
        btnNew = r.Q<Button>("BtnNew");
        btnControls = r.Q<Button>("BtnControls");
        btnSettings = r.Q<Button>("BtnSettings");
        btnQuit = r.Q<Button>("BtnQuit");

        btnResume.clicked += () => { Hide(); PauseManager.Resume(); };
        btnNew.clicked += () => { PauseManager.Resume(); SceneManager.LoadScene(SceneManager.GetActiveScene().name); };
        btnControls.clicked += () => Debug.Log("TODO: ��Ʈ�� �г� ����");
        btnSettings.clicked += () => Debug.Log("TODO: ���� �г� ����");
        btnQuit.clicked += OnQuit;

        // ������ ���� + �Ͻ����� �гη� ���
        Hide();
        UINavigator.Instance.Register(this, isPauseMenu: true);
    }

    public override void Show()
    {
        base.Show();
        // ��Ŀ�� ����
        btnResume?.Focus();
    }

    void OnQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
