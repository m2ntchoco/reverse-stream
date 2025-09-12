// LoadingTipsDatabase.cs
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadingTips", menuName = "Game/Loading Tips")]
public class LoadingTipsDatabase : ScriptableObject
{
    [Serializable]
    public class Tip
    {
        [TextArea(2, 5)] public string text;
        [Range(0f, 5f)] public float weight = 1f; // ����ġ(���� ���ü��� ũ��)
        public string tag;   // "controls","world","boss" ��
        public SystemLanguage language = SystemLanguage.Korean; // �ٱ����
    }

    public List<Tip> tips = new();
}