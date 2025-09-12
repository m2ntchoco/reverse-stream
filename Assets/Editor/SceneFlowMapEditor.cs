// SceneFlowMapEditor.cs (Ŀ���� ������)
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// SceneFlowMap�� ���� Ŀ���� �ν�����
[CustomEditor(typeof(SceneFlowMap))]
public class SceneFlowMapEditor : Editor
{
    SerializedProperty flowEntries;

    private void OnEnable()
    {
        flowEntries = serializedObject.FindProperty("flowEntries");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("\uD83C\uDF1F �� �帧 ���̺�", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        for (int i = 0; i < flowEntries.arraySize; i++)
        {
            var entry = flowEntries.GetArrayElementAtIndex(i);
            var current = entry.FindPropertyRelative("currentSceneName");
            var useRandom = entry.FindPropertyRelative("useRandomNext");
            var next = entry.FindPropertyRelative("nextSceneName");
            var randomList = entry.FindPropertyRelative("randomNextScenes");

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(current, new GUIContent("���� �� �̸�"));
            EditorGUILayout.PropertyField(useRandom, new GUIContent("���� ���� �� ���"));

            if (useRandom.boolValue)
            {
                EditorGUILayout.PropertyField(randomList, new GUIContent("���� ���� ����"), true);
            }
            else
            {
                EditorGUILayout.PropertyField(next, new GUIContent("���� �� �̸�"));
            }

            if (GUILayout.Button("����"))
            {
                flowEntries.DeleteArrayElementAtIndex(i);
                break;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("�� �帧 �߰�"))
        {
            flowEntries.InsertArrayElementAtIndex(flowEntries.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
