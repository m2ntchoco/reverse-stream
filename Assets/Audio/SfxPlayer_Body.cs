using UnityEngine;
using UnityEngine.Audio;

public class SfxPlayer_Body : MonoBehaviour
{
    [ContextMenu("Check Entries Count")]
    private void _CheckEntries()
    {
        Debug.Log($"[SfxPlayer_Body] {gameObject.name} entries count: " +
                  (entries != null ? entries.Length : 0));
    }

    [System.Serializable]
    public struct SfxEntry
    {
        public string key;     // "dash", "hit", "foot_L" ��
        public AudioClip clip; // �ش� Ŭ��
        [Range(0f, 1f)] public float volume; // �� �׸�(Ŭ��)���� ����
    }

    [Header("Output (Optional)")]
    public AudioMixerGroup outputGroup; // SFX �׷� ����(������ ����� ��)

    [Header("Clips")]
    public SfxEntry[] entries;          // Ű�� ã�� �� ���
    public AudioClip[] randomSet;       // ���� ���� Play() ȣ�� �� ����

    [Header("Playback")]
    [Range(0f, 1f)] public float volume = 1f;
    public Vector2 pitchRandom = new Vector2(1.0f, 1.0f);
    [Tooltip("UI�� ������=2D, ���尨 �ִ� �Ҹ�=3D")]
    public bool play2D = false;

    private AudioSource _src;

    void Awake()
    {
        _src = GetComponent<AudioSource>();
        if (!_src) _src = gameObject.AddComponent<AudioSource>();

        _src.playOnAwake = false;
        _src.loop = false;
        _src.dopplerLevel = 0f;
        _src.spatialBlend = play2D ? 0f : 1f; // 0=2D, 1=3D,  1f�� ������ ��
        if (outputGroup) _src.outputAudioMixerGroup = outputGroup;
    }

    // �ִϸ��̼� �̺�Ʈ: ���� ���� �� randomSet���� �� ��
    public void Play()
    {
        if (randomSet == null || randomSet.Length == 0) return;
        var clip = randomSet[Random.Range(0, randomSet.Length)];
        PlayOneShot(clip);
    }

    // �ִϸ��̼� �̺�Ʈ: int ���� �� entries[index]
    public void PlayByIndex(int index)
    {
        if (entries == null || index < 0 || index >= entries.Length) return;
        var e = entries[index];
        if (e.clip) PlayOneShot(e.clip, e.volume); // �� ��Ʈ�� ���� ����
    }

    // �ִϸ��̼� �̺�Ʈ: string ���� �� key�� ã��
    public void PlayByKey(string key)
    {
        if (entries == null) return;
        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].key == key)
            {
                var e = entries[i];
                if (e.clip)
                {
                    PlayOneShot(e.clip, e.volume); // �� ��Ʈ�� ���� ����
                }
                else
                {
                    Debug.LogWarning($"[SfxPlayer_Body] Clip is missing for key={key} in {gameObject.name}");
                }
                return;
            }
        }
        Debug.LogWarning($"[SfxPlayer_Body] No matching entry found for key={key} in {gameObject.name}");
    }

    private void PlayOneShot(AudioClip clip, float entryVolume = 1f)
    {
        _src.pitch = Random.Range(pitchRandom.x, pitchRandom.y);
        // ���� ���� = ������(volume) �� ��Ʈ��(entryVolume)
        _src.PlayOneShot(clip, Mathf.Clamp01(volume * (entryVolume <= 0f ? 1f : entryVolume)));
    }

    // �ʿ� �� ��Ÿ�� ��ȯ
    public void Set2D(bool is2D)
    {
        play2D = is2D;
        if (_src) _src.spatialBlend = is2D ? 0f : 1f;
    }
}

