// Assets/Scripts/EdgeToPolygonThickener.cs
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ����: ������ ��ȯ + ��Ÿ�� �ڵ� ��ȯ
/// - ������: �޴� â(Edge �� Polygon)���� ���� ������Ʈ ��ȯ
/// - ��Ÿ��/����: ������Ʈ�� �ٿ��θ� Awake���� �ڵ� ��ȯ
/// �ֽ� Unity(2023+)������ usedByComposite ��� compositeOperation ���
/// </summary>
[DefaultExecutionOrder(-1000)]
[DisallowMultipleComponent]
[RequireComponent(typeof(EdgeCollider2D))]
public class EdgeToPolygonThickener : MonoBehaviour
{
    [Header("Thickness (world units)")]
    [Min(0.001f)] public float thickness = 0.1f;

    [Header("When to convert (Runtime)")]
    public bool convertOnAwake = true;   // ��Ÿ��/���忡�� Awake �� �ڵ� ��ȯ

    [Header("Edge handling")]
    public bool disableOriginalEdge = true;
    public bool destroyOriginalEdge = false;

    [Header("Polygon Collider options")]
    public bool copyIsTrigger = true;
    public PhysicsMaterial2D physicsMaterial;  // �ʿ�� ���� (null ����)
    public bool useComposite = false;          // CompositeCollider2D�� ��ĥ��

    void Awake()
    {
        if (convertOnAwake)
        {
            ConvertNow();
        }
    }

    [ContextMenu("Convert Now (Runtime)")]
    public void ConvertNow()
    {
        var edge = GetComponent<EdgeCollider2D>();
        if (edge == null)
        {
            Debug.LogWarning("[EdgeToPolygonThickener] EdgeCollider2D�� �����ϴ�.", this);
            return;
        }

        bool ok = ConvertEdgeToPolygon(
            gameObject, edge, thickness,
            disableOriginalEdge, destroyOriginalEdge,
            copyIsTrigger, physicsMaterial, useComposite,
            useUndo: false // ��Ÿ���� Undo �Ұ�
        );

        if (ok)
            Debug.Log($"[EdgeToPolygonThickener] ��Ÿ�� ��ȯ �Ϸ�: {name}, thickness={thickness}", this);
    }

    /// <summary>
    /// ���� ��ȯ ����(������/��Ÿ�� ����)
    /// </summary>
    public static bool ConvertEdgeToPolygon(
        GameObject go,
        EdgeCollider2D edge,
        float thickness,
        bool disableEdge,
        bool destroyEdge,
        bool copyIsTrigger,
        PhysicsMaterial2D material,
        bool useComposite,
        bool useUndo // �����Ϳ����� true��, ��Ÿ���� false��
    )
    {
        if (edge == null) return false;
        var points = edge.points;
        if (points == null || points.Length < 2)
        {
            Debug.LogWarning($"[Edge �� Poly] {go.name}: ����Ʈ�� 2�� �̸��Դϴ�.", go);
            return false;
        }

        float half = Mathf.Max(0.0005f, thickness * 0.5f);

        // ��� ��� ��� (����)
        Vector2[] normals = new Vector2[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            Vector2 n = Vector2.zero;

            if (i > 0)
            {
                var dirPrev = (points[i] - points[i - 1]).normalized;
                n += new Vector2(-dirPrev.y, dirPrev.x);
            }
            if (i < points.Length - 1)
            {
                var dirNext = (points[i + 1] - points[i]).normalized;
                n += new Vector2(-dirNext.y, dirNext.x);
            }

            if (n.sqrMagnitude < 1e-6f)
            {
                // ����/���� ����
                Vector2 dir;
                if (i == 0) dir = (points[1] - points[0]).normalized;
                else if (i == points.Length - 1) dir = (points[i] - points[i - 1]).normalized;
                else dir = (points[i + 1] - points[i - 1]).normalized;

                n = new Vector2(-dir.y, dir.x);
            }
            normals[i] = n.normalized;
        }

        Vector2 edgeOffset = edge.offset;
        var right = new List<Vector2>(points.Length);
        var left = new List<Vector2>(points.Length);

        for (int i = 0; i < points.Length; i++)
        {
            var p = points[i] + edgeOffset; // ���� ��ǥ
            var n = normals[i];

            right.Add(p + n * half);
            left.Add(p - n * half);
        }

        // ������ ���(�ݽð� ����): ������ ���� �� ���� ����
        var polyPath = new List<Vector2>(right.Count + left.Count);
        polyPath.AddRange(right);
        for (int i = left.Count - 1; i >= 0; i--)
            polyPath.Add(left[i]);

        // PolygonCollider2D ����/ȹ��
        PolygonCollider2D poly = go.GetComponent<PolygonCollider2D>();
        if (!poly)
        {
#if UNITY_EDITOR
            if (useUndo) poly = (PolygonCollider2D)UnityEditor.Undo.AddComponent<PolygonCollider2D>(go);
            else poly = go.AddComponent<PolygonCollider2D>();
#else
            poly = go.AddComponent<PolygonCollider2D>();
#endif
        }

#if UNITY_EDITOR
        if (useUndo) UnityEditor.Undo.RecordObject(poly, "Set Polygon Path");
#endif
        poly.pathCount = 1;
        poly.SetPath(0, polyPath.ToArray());
        poly.sharedMaterial = material;
        if (copyIsTrigger) poly.isTrigger = edge.isTrigger;

        // Composite ��� �� ���� (���� �б�)
        if (useComposite)
        {
#if UNITY_2023_1_OR_NEWER
            poly.compositeOperation = Collider2D.CompositeOperation.Merge;
#else
            poly.usedByComposite = true;
#endif

            var rb = go.GetComponent<Rigidbody2D>();
            if (!rb)
            {
#if UNITY_EDITOR
                if (useUndo) rb = (Rigidbody2D)UnityEditor.Undo.AddComponent<Rigidbody2D>(go);
                else rb = go.AddComponent<Rigidbody2D>();
#else
                rb = go.AddComponent<Rigidbody2D>();
#endif
            }
            rb.bodyType = RigidbodyType2D.Static;

            var comp = go.GetComponent<CompositeCollider2D>();
            if (!comp)
            {
#if UNITY_EDITOR
                if (useUndo) comp = (CompositeCollider2D)UnityEditor.Undo.AddComponent<CompositeCollider2D>(go);
                else comp = go.AddComponent<CompositeCollider2D>();
#else
                comp = go.AddComponent<CompositeCollider2D>();
#endif
            }
            comp.geometryType = CompositeCollider2D.GeometryType.Polygons;
        }

        // ���� Edge ó��
        if (disableEdge) edge.enabled = false;
        if (destroyEdge)
        {
#if UNITY_EDITOR
            if (useUndo) UnityEditor.Undo.DestroyObjectImmediate(edge);
            else Object.DestroyImmediate(edge);
#else
            Object.Destroy(edge);
#endif
        }

        return true;
    }

#if UNITY_EDITOR
    // ����������������������������������������������������������������������������������������������������������������������������������������������������������
    // ������ ����: �ν����� ��ư & �޴� â
    // ����������������������������������������������������������������������������������������������������������������������������������������������������������
    [UnityEditor.CustomEditor(typeof(EdgeToPolygonThickener))]
    public class EdgeToPolygonThickenerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Convert Now (Editor)"))
            {
                var comp = (EdgeToPolygonThickener)target;
                var edge = comp.GetComponent<EdgeCollider2D>();
                if (edge)
                {
                    ConvertEdgeToPolygon(
                        comp.gameObject, edge, comp.thickness,
                        comp.disableOriginalEdge, comp.destroyOriginalEdge,
                        comp.copyIsTrigger, comp.physicsMaterial, comp.useComposite,
                        useUndo: true
                    );
                    UnityEditor.EditorUtility.SetDirty(comp.gameObject);
                }
                else
                {
                    Debug.LogWarning("EdgeCollider2D�� �����ϴ�.", comp);
                }
            }
        }
    }

    public class EdgeToPolygonThickenerWindow : UnityEditor.EditorWindow
    {
        float thickness = 0.1f;
        bool disableEdge = true;
        bool destroyEdge = false;
        bool copyIsTrigger = true;
        bool useComposite = false;
        PhysicsMaterial2D sharedMaterial;

        [UnityEditor.MenuItem("Tools/Physics2D/Edge �� Polygon (Thicken)...")]
        public static void ShowWindow()
        {
            var win = GetWindow<EdgeToPolygonThickenerWindow>("Edge �� Polygon");
            win.minSize = new Vector2(360, 230);
            win.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Edge �� Polygon (Thickener)", UnityEditor.EditorStyles.boldLabel);
            thickness = UnityEditor.EditorGUILayout.Slider("Thickness", thickness, 0.01f, 1f);
            disableEdge = UnityEditor.EditorGUILayout.Toggle("Disable Original Edge", disableEdge);
            destroyEdge = UnityEditor.EditorGUILayout.Toggle("Destroy Original Edge", destroyEdge);
            copyIsTrigger = UnityEditor.EditorGUILayout.Toggle("Copy isTrigger", copyIsTrigger);
            useComposite = UnityEditor.EditorGUILayout.Toggle("Use Composite (Static RB)", useComposite);
            sharedMaterial = (PhysicsMaterial2D)UnityEditor.EditorGUILayout.ObjectField("Physics Material 2D", sharedMaterial, typeof(PhysicsMaterial2D), false);

            UnityEditor.EditorGUILayout.Space();
            if (GUILayout.Button("Convert Selected"))
            {
                var targets = UnityEditor.Selection.gameObjects;
                if (targets == null || targets.Length == 0)
                {
                    UnityEditor.EditorUtility.DisplayDialog("Edge �� Polygon", "������Ʈ�� �����ϼ���.", "OK");
                    return;
                }

                foreach (var go in targets)
                {
                    var edge = go.GetComponent<EdgeCollider2D>();
                    if (!edge) continue;

                    bool ok = EdgeToPolygonThickener.ConvertEdgeToPolygon(
                        go, edge, thickness,
                        disableEdge, destroyEdge,
                        copyIsTrigger, sharedMaterial, useComposite,
                        useUndo: true
                    );

                    if (ok)
                    {
                        UnityEditor.EditorUtility.SetDirty(go);
                        Debug.Log($"[Edge �� Poly] '{go.name}' ��ȯ �Ϸ�. thickness={thickness}", go);
                    }
                }
            }
        }
    }
#endif
}
