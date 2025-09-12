// Assets/Audio/MusicPlayer.cs
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    [Header("Mixer")]
    public AudioMixer mixer;                 // ��ü �ͼ�(�ɼ�)
    public AudioMixerGroup musicGroup;       // Music �׷�
    [Tooltip("AudioMixer�� Music �׷쿡 ������ �Ķ���� �̸� (��: MusicVolume)")]
    public string exposedMusicParam = "MusicVolume";

    [Header("Crossfade")]
    [Range(0f, 5f)] public float crossfadeSec = 1.5f;

    private AudioSource _a, _b;
    private AudioSource _current;
    private float _targetVolume = 1f; // 0~1 linear

    void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _a = gameObject.AddComponent<AudioSource>();
        _b = gameObject.AddComponent<AudioSource>();

        foreach (var s in new[] { _a, _b })
        {
            s.playOnAwake = false;
            s.loop = true;
            s.spatialBlend = 0f; // 2D
            if (musicGroup) s.outputAudioMixerGroup = musicGroup;
        }

        _current = _a;

        // �ʱ� ���� �ݿ�(���� �Ķ���Ͱ� ������ dB�� ����)
        SetMusicLinearVolume(1f);
    }

    // �ܺο���: �� �̸��� �޾� �ش� ������ ���
    public void PlayForScene(SceneMusicMap map, string sceneName)
    {
        if (!map) return;
        if (map.TryGet(sceneName, out var e))
        {
            Play(e.clip, e.loop, e.volume);
        }
    }

    // �ܺο���: Ư�� Ŭ�� ���
    public void Play(AudioClip clip, bool loop = true, float clipVolume = 1f)
    {
        if (!clip) return;

        _targetVolume = Mathf.Clamp01(clipVolume);

        var next = (_current == _a) ? _b : _a;
        next.clip = clip;
        next.loop = loop;
        next.volume = 0f;
        next.Play();

        StopAllCoroutines();
        StartCoroutine(Crossfade(_current, next, crossfadeSec, _targetVolume));
        _current = next;
    }

    System.Collections.IEnumerator Crossfade(AudioSource from, AudioSource to, float sec, float toVolume)
    {
        float t = 0f;
        float fromStart = from ? from.volume : 0f;
        float toStart = to.volume;

        while (t < sec)
        {
            t += Time.unscaledDeltaTime; // �Ͻ����� �߿��� ���̵��Ϸ��� unscaled
            float k = sec > 0f ? (t / sec) : 1f;

            if (from) from.volume = Mathf.Lerp(fromStart, 0f, k);
            to.volume = Mathf.Lerp(toStart, toVolume, k);

            yield return null;
        }

        if (from)
        {
            from.volume = 0f;
            from.Stop();
        }
        to.volume = toVolume;
    }

    // UI �����̴�(0~1) �� �ͼ� dB�� �ݿ�
    public void SetMusicLinearVolume(float v01)
    {
        v01 = Mathf.Clamp01(v01);
        if (!string.IsNullOrEmpty(exposedMusicParam) && mixer)
        {
            mixer.SetFloat(exposedMusicParam, Linear01ToDb(v01));
        }
        else
        {
            // mixer �Ķ���� ���̵� �鸮�� �Ϸ��� ���� �ҽ� ���� ����(����)
            if (_current) _current.volume = v01;
            if (_current == _a && _b) _b.volume = v01;
            if (_current == _b && _a) _a.volume = v01;
        }
    }

    // �Ͻ����� ������
    public void Pause(bool pause)
    {
        foreach (var s in new[] { _a, _b })
        {
            if (!s) continue;
            if (pause) s.Pause();
            else s.UnPause();
        }
    }

    // 0~1 ���� �� dB(-80~0)
    public static float Linear01ToDb(float v)
    {
        if (v <= 0.0001f) return -80f; // ��ǻ� ���Ұ�
        return Mathf.Log10(v) * 20f;   // 0~1 �� -inf~0dB
    }
}
