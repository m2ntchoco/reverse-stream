// Assets/Audio/SceneMusicMap.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SceneMusicMap", menuName = "Audio/Scene Music Map")]
public class SceneMusicMap : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
#if UNITY_EDITOR
        public UnityEditor.SceneAsset sceneAsset; // �����Ϳ��� �� �巡�׿�(����)
#endif
        public string sceneName;                  // ��Ÿ�ӿ��� ������ ����� �̸�
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop = true;
    }

    public List<Entry> entries = new List<Entry>();

    public bool TryGet(string sceneName, out Entry e)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].sceneName == sceneName)
            {
                e = entries[i];
                return true;
            }
        }
        e = null;
        return false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var e in entries)
        {
            if (e == null) continue;
            if (e.sceneAsset)
            {
                string name = e.sceneAsset.name;
                if (e.sceneName != name)
                    e.sceneName = name;
            }
        }
    }
#endif
}
