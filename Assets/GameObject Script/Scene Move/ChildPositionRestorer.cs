using UnityEngine;
using System.Collections.Generic;

public class ChildPositionRestorer : MonoBehaviour
{
    private Dictionary<Transform, Vector3> originalLocalPositions = new();

    void Awake()
    {
        // �ڽĵ��� �ʱ� localPosition ����
        foreach (Transform child in transform)
        {
            originalLocalPositions[child] = child.localPosition;
        }
    }

    public void RestoreLocalPositions()
    {
        // ����� localPosition���� ����
        foreach (var kvp in originalLocalPositions)
        {
            if (kvp.Key != null)
                kvp.Key.localPosition = kvp.Value;
        }
    }
}