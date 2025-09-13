using UnityEngine;
using System;

[System.Serializable]
public class BossSkill
{
    public string skillName;
    public Action skillAction;
    public float cooldown;
    public float lastUsedTime;
    public float castTime;
    public bool IsReady()
    {
        return Time.time >= lastUsedTime + cooldown;
    }

    public void Use()
    {
        skillAction?.Invoke();
        lastUsedTime = Time.time; //plz commit
    }
}